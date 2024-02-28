namespace DotnetHeadStart;

public class SMTPMailSender(IConfiguration configuration, ILogger logger) : IEmailSender
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
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_configuration["SMTP_USER"]));
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
            smtp.Connect(_configuration["SMTP_HOST"], int.Parse(_configuration["SMTP_PORT"] ?? "465"));
            smtp.Authenticate(_configuration["SMTP_USER"], _configuration["SMTP_PASSWORD"]);
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
