namespace OVHPoc.Shared.Models;
public class MailMigrationRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email  { get; set; }
    public MigrationStatus Status { get; set; }
    public DateTime CreationDateUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateDateUtc { get; set; }
}
