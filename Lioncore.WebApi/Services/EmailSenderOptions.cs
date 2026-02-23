namespace Lioncore.WebApi.Services;

public class EmailSenderOptions
{
    public const string EmailSender = "EmailSender";
    public int Port { get; set; }
    public string Host { get; set; } = string.Empty;
    public string SystemEmail { get; set; } = string.Empty;
    public bool UseSsl { get; set; }
}
