using Microsoft.EntityFrameworkCore;
using OVHPoc.Shared.Models;
using OVHPoc.Shared.Models.MerelyMail;

namespace OVHPoc.Shared;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //Get the db file from current executing assembly 
        var projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        var path = Path.Combine(projectDir, "OVHPoc.Shared\\Providers\\MerelyMail\\");

        optionsBuilder.UseSqlite($"Data Source={path}MerelyMail.db");
    }

    public DbSet<Folder> Folders { get; set; }
    public DbSet<Mailbox> Mailbox { get; set; }
    public DbSet<Mail> Mails {  get; set; }   
    public DbSet<MailMigrationRequest> MailMigrationRequests {  get; set; }
}
