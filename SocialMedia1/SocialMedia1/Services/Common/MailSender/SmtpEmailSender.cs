using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SocialMedia1.Services.Common.MailSender
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly string smtpUsername;
        private readonly string smtpPassword;

        public SmtpEmailSender(string smtpUsername, string smtpPassword, string smtpServer = "smtp-relay.brevo.com", int smtpPort = 587)
        {
            this.smtpServer = smtpServer;
            this.smtpPort = smtpPort;
            this.smtpUsername = smtpUsername;
            this.smtpPassword = smtpPassword;
        }

        public async Task<bool> SendEmailAsync(string from, string fromName, string to, string subject, string htmlContent)
        {
            if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlContent))
            {
                throw new ArgumentException("Subject and message should be provided.");
            }

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;

                var fromAddress = new MailAddress(from, fromName);
                var toAddress = new MailAddress(to);

                using (var message = new MailMessage(fromAddress, toAddress))
                {
                    message.Subject = subject;
                    message.Body = htmlContent;
                    message.IsBodyHtml = true;

                    try
                    {
                        await client.SendMailAsync(message);
                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
        }

        public async Task AutoSendEmail(string email, string name)
        {
            var fromAddress = new MailAddress(GlobalConstants.MainEmail, GlobalConstants.FromNameEmailSender);
            var toAddress = new MailAddress(email);

            var subject = "Feedback received!";
            var body = $"Hello, {name} \r\n Thank you for your feedback. Our team will reach you soon!";

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;

                using (var message = new MailMessage(fromAddress, toAddress))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = false; // Plain text email

                    try
                    {
                        await client.SendMailAsync(message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
        }
    }
}
