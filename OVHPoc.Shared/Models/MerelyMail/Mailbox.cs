namespace OVHPoc.Shared.Models.MerelyMail;
public class Mailbox
{
    public int Id { get; set; }
    public string Email { get; set; }
    public int Quota { get; set; }
    public string password { get; set; } = string.Empty;
    public IEnumerable<Mail> Mails { get; set; }
    public IEnumerable<Folder> Folders { get; set; }
}
