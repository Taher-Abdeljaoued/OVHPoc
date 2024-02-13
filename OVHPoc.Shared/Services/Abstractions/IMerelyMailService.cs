using OVHPoc.Shared.Models;
using OVHPoc.Shared.Models.MerelyMail;

namespace OVHPoc.Shared.Services.Abstractions;
public interface IMerelyMailService
{
    Task AddAsync(Mailbox mailbox);
    Task<Mailbox> GetMailboxAsync(string email);
}
