using Microsoft.EntityFrameworkCore;
using OVHPoc.Shared.Models.MerelyMail;
using OVHPoc.Shared.Services.Abstractions;

namespace OVHPoc.Shared.Services;
public class MerelyMailService : IMerelyMailService
{
    private readonly ApplicationDbContext _context;

    public MerelyMailService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Mailbox mailbox)
    {
        await _context.AddAsync(mailbox);
        await _context.SaveChangesAsync();
    }

    public async Task<Mailbox> GetMailboxAsync(string email)
    {
        return await _context.Mailbox
            .Include(m => m.Mails)
            .ThenInclude(f => f.Folder)
            .Include(m => m.Folders)
            .FirstOrDefaultAsync(m => m.Email == email);
    }
}
