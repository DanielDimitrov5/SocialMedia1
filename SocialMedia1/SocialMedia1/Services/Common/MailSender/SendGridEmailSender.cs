using SendGrid;
using SendGrid.Helpers.Mail;

namespace SocialMedia1.Services.Common.MailSender
{
    public class SendGridEmailSender : IEmailSender
    {

        private readonly SendGridClient client;

        public SendGridEmailSender(string apiKey)
        {
            client = new SendGridClient(new SendGridClientOptions
            {
                ApiKey = apiKey
            });
        }

        public async Task<bool> SendEmailAsync(string from, string fromName, string to, string subject, string htmlContent)
        {
            if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlContent))
            {
                throw new ArgumentException("Subject and message should be provided.");
            }

            var fromAddress = new EmailAddress(from, fromName);
            var toAddress = new EmailAddress(to);
            var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, null, htmlContent);

            try
            {
                var response = await this.client.SendEmailAsync(message);
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(await response.Body.ReadAsStringAsync());

                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task AutoSendEmail(string email, string name)
        {
            var fromAddress = new EmailAddress(GlobalConstants.MainEmail, GlobalConstants.FromNameEmailSender);

            var toAddress = new EmailAddress(email);

            var message = MailHelper
                .CreateSingleEmail(fromAddress, toAddress, "Feedback recieved!", null, $"Hello, {name} \r\n Thank you for yout feedback. Out team will reach you soon!");

            try
            {
                var response = await this.client.SendEmailAsync(message);
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(await response.Body.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
