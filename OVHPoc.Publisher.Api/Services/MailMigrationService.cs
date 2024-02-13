using MassTransit;
using Microsoft.EntityFrameworkCore;
using OVHPoc.Contracts.Contracts;
using OVHPoc.Shared;
using OVHPoc.Shared.Models;
using OVHPoc.Publisher.Api.Services.Abstractions;

namespace OVHPoc.Publisher.Api.Services;

public class MailMigrationService : IMailMigrationService
{
    private readonly ILogger<MailMigrationService> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ApplicationDbContext _context;

    public MailMigrationService(ILogger<MailMigrationService> logger,
                                IPublishEndpoint publishEndpoint,
                                ApplicationDbContext context)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _context = context;
    }

    public async Task<MailMigrationRequest> AddAsync(string email)
    {
        var migrationRequest = new MailMigrationRequest
        {
            Email = email,
            Status = MigrationStatus.Created
        };

        await _context.AddAsync(migrationRequest);
        await _context.SaveChangesAsync();

        return migrationRequest;
    }

    public async Task<MailMigrationRequest> GetStatus(string email)
    {
        return await _context.MailMigrationRequests
                .OrderByDescending(m => m.CreationDateUtc)
                .FirstOrDefaultAsync(m => m.Email == email);
    }

    public async Task SendNotification(MailMigrationNotification mailMigrationNotification)
    {
        _logger.LogInformation($"Migration for {mailMigrationNotification.Email} published");

        await _publishEndpoint.Publish(mailMigrationNotification);
    }
}
