using OVHPoc.Shared.Models.AlmostMail;

namespace OVHPoc.Shared.Services.Abstractions;
public interface IAlmostMailService
{
    Task AddMailboxAsync(Mailbox mailbox, string email);
    Task<Mailbox> GetMailboxAsync(string email);
}
