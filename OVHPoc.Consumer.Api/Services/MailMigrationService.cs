using Microsoft.EntityFrameworkCore;
using OVHPoc.Consumer.Api.Services.Abstractions;
using OVHPoc.Shared;
using OVHPoc.Shared.Extensions;
using OVHPoc.Shared.Models;
using OVHPoc.Shared.Models.MerelyMail;
using OVHPoc.Shared.Services;
using OVHPoc.Shared.Services.Abstractions;
using System.Linq.Expressions;

namespace OVHPoc.Consumer.Api.Services;

public class MailMigrationService : IMailMigrationService
{
    private readonly ILogger<MailMigrationService> _logger;
    private readonly IAlmostMailService _almostMailService;
    private readonly IMerelyMailService _merelyMailService;
    private readonly ApplicationDbContext _context;

    public MailMigrationService(IAlmostMailService almostMailService,
        IMerelyMailService merelyMailService,
        ApplicationDbContext applicationDbContext,
        ILogger<MailMigrationService> logger)
    {
        _almostMailService = almostMailService;
        _merelyMailService = merelyMailService;
        _context = applicationDbContext;
        _logger = logger;
    }

    public async Task<bool> MigrateAsync(string email, string sourceProvider)
    {
        bool result = true;
        try
        {
            switch (sourceProvider)
            {
                case "MerelyMail":
                    await MigrateToAlmostAsync(email);
                    break;
                case "AlmostMail":
                    await MigrateToMerelyAsync(email);
                    break;

                default:
                    _logger.LogInformation($"Provider {sourceProvider} unkown - Migration failed");
                    result = false;
                    break;
            }
        }
        catch (Exception)
        {
            result = false;
        }
        return result;
    }

    public async Task UpdateMigrationStatusAsync(Guid migrationId, MigrationStatus status)
    {
        var migrationRequest = await _context.MailMigrationRequests
            .FirstOrDefaultAsync(_ => _.Id == migrationId);

        if (migrationRequest is null)
            return;

        migrationRequest.Status = status;
        migrationRequest.UpdateDateUtc = DateTime.UtcNow;
        _context.MailMigrationRequests.Update(migrationRequest);
        await _context.SaveChangesAsync();
    }

    private async Task MigrateToAlmostAsync(string email)
    { 
        //Get mailbox from db
        var mailbox = await _merelyMailService.GetMailboxAsync(email);

        if (mailbox is null)
        {
            //Logic to add in case of failure
            return;
        }

        await _almostMailService.AddMailboxAsync(mailbox.ToAlmostMailbox(), email);
        //Todo -> Result pattern to implement to have a better vision on the outcome.
    }

    private async Task MigrateToMerelyAsync(string email)
    {
        var mailbox = await _almostMailService.GetMailboxAsync(email);

        if (mailbox is null)
        {
            //Logic to add in case of failure
            return;
        }

        using var transaction = _context.Database.BeginTransaction();

        try
        {
            //Add Mailbox 
            var mailboxToAdd = mailbox.ToMerelyMailbox(email);
            await _context.Mailbox.AddAsync(mailboxToAdd);
            await _context.SaveChangesAsync();

            //Add folders
            var foldersToAdd = mailbox.Mails
               .Select(m => new Folder { Name = m.FiledInto, MailboxId = mailboxToAdd.Id })
               .DistinctBy(f => f.Name)
               .ToList();
            await _context.AddRangeAsync(foldersToAdd);
            await _context.SaveChangesAsync();

            //Add mails
            List<Mail> mailsToAdd = new();
            foreach (var mail in mailbox.Mails)
            {
                Mail mailToAdd = new Mail();
                mailToAdd.MailboxId = mailboxToAdd.Id; //Id mailbox - foreign key
                mailToAdd.FolderId = foldersToAdd.Single(f => f.Name == mail.FiledInto).Id; //Id folder from its name - foreign key
                mailToAdd.From = mail.From;
                mailToAdd.To = mail.To;
                mailToAdd.Subject = mail.Subject;
                mailToAdd.Body = mail.Body;
                mailToAdd.Size = (int)mail.Size;
                mailsToAdd.Add(mailToAdd);
            }
            await _context.Mails.AddRangeAsync(mailsToAdd);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
        }
    }
}
