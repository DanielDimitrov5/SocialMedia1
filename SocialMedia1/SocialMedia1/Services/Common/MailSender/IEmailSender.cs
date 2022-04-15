namespace SocialMedia1.Services.Common.MailSender
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(
            string from,
            string fromName,
            string to,
            string subject,
            string htmlContent
            );

        Task AutoSendEmail(string email, string name);
    }
}
