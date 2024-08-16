using DotnetHeadStart.Exceptions;

namespace DotnetHeadStart.Mailing;

public class SMTPMailSender(IConfiguration configuration, ILogger logger) : IMailSender
{
    public readonly IConfiguration _configuration = configuration;
    public readonly ILogger _logger = logger;

    /// <summary>
    /// Send email using SMTP
    /// </summary>
    /// <param name="recipients">Array of recipients for the email.</param>
    /// <param name="subject">Subject of the email.</param>
    /// <param name="htmlMessage">Body of the email.</param>
    /// <returns> </returns>
    /// <exception cref="HeadStartException">Thrown when an error occurs while sending the email.</exception>
    public Task SendEmailAsync(string[] recipients, string subject, string htmlMessage)
    {

        var smtpUser = _configuration["SMTP_USER"];
        if (string.IsNullOrWhiteSpace(smtpUser))
        {
            Log.Error("SMTP_USER not set in configuration");
            throw new HeadStartException("SMTP_USER not set in configuration");
        }
        var smtpHost = _configuration["SMTP_HOST"];
        if (string.IsNullOrWhiteSpace(smtpHost))
        {
            Log.Error("SMTP_HOST not set in configuration");
            throw new HeadStartException("SMTP_HOST not set in configuration");
        }

        var smtpPort = _configuration["SMTP_PORT"];

        var smtpPassword = _configuration["SMTP_PASSWORD"];
        if (string.IsNullOrWhiteSpace(smtpPassword))
        {
            Log.Error("SMTP_PASSWORD not set in configuration");
            throw new HeadStartException("SMTP_PASSWORD not set in configuration");
        }

        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(smtpUser));
        foreach (var recipient in recipients)
        {
            message.To.Add(MailboxAddress.Parse(recipient));
        }
        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };
        try
        {
            //send email
            using var smtp = new SmtpClient();
            smtp.Connect(smtpHost, int.Parse(smtpPort ?? "465"));
            smtp.Authenticate(smtpUser, smtpPassword);
            var resp = smtp.Send(message);
            smtp.Disconnect(true);
        }
        catch (Exception ex)
        {
            throw new HeadStartException("Error sending email", ex);
        }
        return Task.CompletedTask;
    }

    public Task SendEmailAsync(string recipient, string subject, string htmlMessage)
    {
        return SendEmailAsync([recipient], subject, htmlMessage);
    }


}
