using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HandyMan_Solutions.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _fromEmail;

        public EmailService()
        {
            _smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            _smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            _smtpUser = ConfigurationManager.AppSettings["SmtpUsername"];
            _smtpPass = ConfigurationManager.AppSettings["SmtpPassword"];
            _fromEmail = ConfigurationManager.AppSettings["FromEmail"];
        }

        public async Task SendEmail(string email, string subject, string body)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.To.Add(new MailAddress(email));
                    message.From = new MailAddress(_fromEmail);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    using (var smtp = new SmtpClient(_smtpServer, _smtpPort))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(message);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                throw new Exception("An error occurred while sending the email.", ex);
            }
        }
    }
}
