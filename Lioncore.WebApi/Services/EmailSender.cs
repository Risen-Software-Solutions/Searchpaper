using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Lioncore.WebApi.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    public EmailSender(
        IOptions<EmailSenderOptions> optionsAccessor,
        ILogger<EmailSender> logger,
        IConfiguration configuration
    )
    {
        Options = optionsAccessor.Value;
        _logger = logger;
        _configuration = configuration;
    }

    public EmailSenderOptions Options { get; } //Set with Secret Manager.

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        await Execute(subject, message, toEmail);
    }

    public async Task Execute(string subject, string message, string toEmail)
    {
        var msg = new MimeMessage();

        msg.From.Add(new MailboxAddress("", Options.SystemEmail));
        msg.To.Add(new MailboxAddress("", toEmail));
        msg.Subject = subject;

        var builder = new BodyBuilder { HtmlBody = message };

        msg.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        client.Connect(Options.Host, Options.Port, Options.UseSsl);
        client.Send(msg);
        client.Disconnect(true);
    }
}
