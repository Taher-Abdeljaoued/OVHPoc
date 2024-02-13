using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OVHPoc.Shared.Models.AlmostMail;
using OVHPoc.Shared.Services.Abstractions;

namespace OVHPoc.Shared.Services;
public class AlmostMailService : IAlmostMailService
{

    private const string JSON_EXTENSION = ".json";
    private const string DATASET_DIRECTORY = "OVHPoc.Shared\\Providers\\AlmostMail";

    private readonly ILogger<AlmostMailService> _logger;

    public AlmostMailService(ILogger<AlmostMailService> logger)
    {
        _logger = logger;
    }

    public async Task AddMailboxAsync(Mailbox mailbox, string email)
    {
        string mailboxJson = JsonConvert.SerializeObject(mailbox, Formatting.Indented);

        var projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        var path = Path.Combine(projectDir, DATASET_DIRECTORY, email + JSON_EXTENSION);
        await File.WriteAllTextAsync(path, mailboxJson);
    }

    public async Task<Mailbox> GetMailboxAsync(string email)
    {
        var projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        var path = Path.Combine(projectDir, DATASET_DIRECTORY, email + JSON_EXTENSION);

        try
        {
            var mailbox = JsonConvert.DeserializeObject<Mailbox>(await File.ReadAllTextAsync(path));
            return mailbox;
        }
        catch(Exception)
        {
            _logger.LogError($"Mail {email} was not found in directory : {path}");
            return null; 
        }
    }
}
