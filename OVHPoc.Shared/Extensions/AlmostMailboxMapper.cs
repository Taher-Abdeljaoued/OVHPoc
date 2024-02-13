
namespace OVHPoc.Shared.Extensions;
public static class AlmostMailBoxMapper
{
    public static Models.MerelyMail.Mailbox ToMerelyMailbox(this Models.AlmostMail.Mailbox almostMailbox, string email)
    {
        var merelyMailbox = new Models.MerelyMail.Mailbox
        {
            Email = email,
            Quota = (int)almostMailbox.Quota,
            password = almostMailbox.password,
            //Mails = almostMailbox.Mails.Select(m => m.ToMerelyMailbox()).ToList(),
            //Folders = almostMailbox.Mails.Select(m => new Models.MerelyMail.Folder { Name = m.FiledInto})
            //                             .DistinctBy(f => f.Name) //Distinct folders !
            //                             .ToList(), 
        };

        return merelyMailbox;
    }


    public static Models.MerelyMail.Mail ToMerelyMail(this Models.AlmostMail.Mail almostMail)
    {
        var merelyMail = new Models.MerelyMail.Mail
        {
            Body = almostMail.Body,
            Subject = almostMail.Subject,
            To = almostMail.To,
            From = almostMail.From,
            Size = (int)almostMail.Size,
            //Folder = new Models.MerelyMail.Folder { Name = almostMail.FiledInto },
        };

        return merelyMail;
    }
}
