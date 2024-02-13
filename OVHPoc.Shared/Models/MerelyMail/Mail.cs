namespace OVHPoc.Shared.Models.MerelyMail;
public class Mail
{
    public int Id { get; set; }
    public int MailboxId { get; set; }
    public int FolderId { get; set; }
    public string? Subject { get; set; }
    public string Body { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public int Size { get; set; }

    public virtual Folder Folder { get; set; }
}
