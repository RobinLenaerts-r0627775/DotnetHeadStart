namespace DotnetHeadStart.Mailing;

public interface IMailSender
{
    Task SendEmailAsync(string[] recipients, string subject, string htmlMessage);
    Task SendEmailAsync(string recipient, string subject, string htmlMessage);
}
