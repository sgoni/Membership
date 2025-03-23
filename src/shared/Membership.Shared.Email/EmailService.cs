using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Membership.Shared.Email;

public interface IEmailService
{
    Task SendEmailAsync(string recipient, string subject, string templatePath, string body);
}

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> options)
    {
        _emailSettings = options.Value;
    }

    public async Task SendEmailAsync(string recipient, string subject, string templatePath, string body)
    {
        //  string content = LoadTemplate(templatePath, body);
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("", _emailSettings.SenderEmail));
        email.To.Add(new MailboxAddress("", recipient));
        email.Subject = subject;

        email.Body = new TextPart("plain")
        {
            Text = body
        };

        using (var client = new SmtpClient())
        {
            // Conenct to SMTP server
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, false);
            await client.AuthenticateAsync(_emailSettings.SenderUser, _emailSettings.SenderPassword);

            // Send email
            await client.SendAsync(email);

            // Disconnect
            await client.DisconnectAsync(true);
        }
    }

    private string LoadTemplate(string templatePath, string content)
    {
        if (!File.Exists(templatePath))
            throw new FileNotFoundException("No se encontró la plantilla de correo.", templatePath);

        var templateContent = File.ReadAllText(templatePath);
        return templateContent.Replace("{{content}}", content);
    }
}