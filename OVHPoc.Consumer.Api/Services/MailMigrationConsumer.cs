using MassTransit;
using OVHPoc.Consumer.Api.Services.Abstractions;
using OVHPoc.Contracts.Contracts;
using OVHPoc.Shared.Models;

namespace OVHPoc.Consumer.Api.Services;

public class MailMigrationConsumer : IConsumer<MailMigrationNotification>
{
    private readonly ILogger<MailMigrationConsumer> _logger;
    private readonly IMailMigrationService _mailMigrationService;

    public MailMigrationConsumer(ILogger<MailMigrationConsumer> logger,
                                 IMailMigrationService mailMigrationService)
    {
        _logger = logger;
        _mailMigrationService = mailMigrationService;
    }

    public async Task Consume(ConsumeContext<MailMigrationNotification> context)
    {
        _logger.LogInformation($"Received migration notification for {context.Message.Email}");

        if (context is null)
        {
            return;
        }

        await _mailMigrationService.UpdateMigrationStatusAsync(context.Message.MigrationId, MigrationStatus.InProgess);

        try
        {
            var resultMigration = await _mailMigrationService.MigrateAsync(context.Message.Email, context.Message.SourceProvider);

            var migrationStatus = resultMigration switch
            {
                true => MigrationStatus.Done,
                false => MigrationStatus.Failed,
            };

            await _mailMigrationService.UpdateMigrationStatusAsync(context.Message.MigrationId, migrationStatus);
        }
        catch (Exception)
        {

            _logger.LogError($"Migration attempt for {context.Message.Email} failed");
            await _mailMigrationService.UpdateMigrationStatusAsync(context.Message.MigrationId, MigrationStatus.Failed);
        }
    }
}

