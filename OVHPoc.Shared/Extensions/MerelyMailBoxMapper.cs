namespace OVHPoc.Shared.Extensions;
public static class MerelyMailBoxMapper
{
    public static Models.AlmostMail.Mailbox ToAlmostMailbox(this Models.MerelyMail.Mailbox merelyMailbox)
    {
        var almostMailbox = new Models.AlmostMail.Mailbox
        {
            Quota = merelyMailbox.Quota,
            password = merelyMailbox.password,
            Size = 0m, //Its not provided in the model
            Mails = merelyMailbox.Mails.Select(m => m.ToAlmostMail()).ToList(),
        };

        return almostMailbox;
    }


    public static Models.AlmostMail.Mail ToAlmostMail(this Models.MerelyMail.Mail merelyMail)
    {
        var almostMail = new Models.AlmostMail.Mail
        {
            Body = merelyMail.Body,
            Subject = merelyMail.Subject,
            To = merelyMail.To,
            From = merelyMail.From,
            Size = merelyMail.Size,
            FiledInto = merelyMail.Folder?.Name,
        };

        return almostMail;
    }
}
