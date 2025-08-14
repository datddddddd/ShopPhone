using System.Net;
using System.Net.Mail;

namespace ShopPhone.Services
{
    public class EmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(_config["EmailSettings:Host"])
            {
                Port = int.Parse(_config["EmailSettings:Port"]),
                Credentials = new NetworkCredential(
                    _config["EmailSettings:UserName"],
                    _config["EmailSettings:Password"]),
                EnableSsl = bool.Parse(_config["EmailSettings:EnableSSL"])
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["EmailSettings:UserName"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}