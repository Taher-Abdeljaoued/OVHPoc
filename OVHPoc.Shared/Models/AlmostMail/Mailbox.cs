using Newtonsoft.Json;

namespace OVHPoc.Shared.Models.AlmostMail;
public class Mailbox
{
    [JsonProperty("mailbox_quota")]
    public decimal Quota { get; set; }

    [JsonProperty("mailbox_size")]
    public decimal Size { get; set; }
    public string password { get; set; } = string.Empty;
    public IEnumerable<Mail> Mails { get; set; }
}
