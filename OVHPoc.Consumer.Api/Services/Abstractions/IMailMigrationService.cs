using OVHPoc.Shared.Models;

namespace OVHPoc.Consumer.Api.Services.Abstractions;

public interface IMailMigrationService
{
    Task<bool> MigrateAsync(string email, string sourceProvider);
    Task UpdateMigrationStatusAsync(Guid migrationId, MigrationStatus status);
}
