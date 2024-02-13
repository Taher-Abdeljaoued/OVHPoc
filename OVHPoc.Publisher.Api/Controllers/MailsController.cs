using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OVHPoc.Contracts.Contracts;
using OVHPoc.Shared.Services.Abstractions;
using OVHPoc.Publisher.Api.Services.Abstractions;

namespace OVHPoc.Publisher.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MailsController : ControllerBase
{
    private readonly IAlmostMailService _almostMailService;
    private readonly IMerelyMailService _merelyMailService;
    private readonly IMailMigrationService _mailMigrationService;

    public MailsController(IAlmostMailService almostMailService,
        IMerelyMailService merelyMailService,
        IMailMigrationService mailMigrationPublisher)
    {
        _almostMailService = almostMailService;
        _merelyMailService = merelyMailService;
        _mailMigrationService = mailMigrationPublisher;
    }

    [HttpGet]
    [Route("almostmail/")]
    public async Task<IActionResult> GetMailbox(string mail)
    {
        var mailbox = await _almostMailService.GetMailboxAsync(mail);
        if (mailbox is null)
        {
            return NotFound(mail);
        }

        return Ok(mailbox);
    }

    [HttpGet]
    [Route("merelymail/")]
    public async Task<IActionResult> GetMerelyMailbox(string mail)
    {
        var mailbox = await _merelyMailService.GetMailboxAsync(mail);
        if (mailbox is null)
        {
            return NotFound(mail);
        }

        return Ok(mailbox);
    }

    [HttpGet]
    [Route("migration/status")]
    public async Task<IActionResult> GetMigrationStatus(string email)
    {
        var migration = await _mailMigrationService.GetStatus(email);

        if (migration is null)
        {
            return NotFound();
        }

        return Ok(migration);
    }

    [HttpPost]
    [Route("migrate/")]
    public async Task<IActionResult> Migrate(string email, string sourceProvider)
    {
        var migrationRequest = await _mailMigrationService.AddAsync(email);

        await _mailMigrationService.SendNotification(new MailMigrationNotification(migrationRequest.Id, email, sourceProvider));

        return NoContent();
    }
}
