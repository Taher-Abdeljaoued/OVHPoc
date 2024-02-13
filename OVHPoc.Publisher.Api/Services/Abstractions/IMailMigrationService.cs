using OVHPoc.Contracts.Contracts;
using OVHPoc.Shared.Models;

namespace OVHPoc.Publisher.Api.Services.Abstractions;

public interface IMailMigrationService
{
    public Task<MailMigrationRequest> AddAsync(string email);
    public Task<MailMigrationRequest> GetStatus(string email);
    public Task SendNotification(MailMigrationNotification mailMigrationNotification);
}
