namespace OVHPoc.Contracts.Contracts;
public record MailMigrationNotification(Guid MigrationId, string Email, string SourceProvider);
