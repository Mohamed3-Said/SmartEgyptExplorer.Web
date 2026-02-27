using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using ServiceAbstraction;
using Shared.DTOS.IdentityModuleDtos;
using System.Threading.Tasks;

namespace Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(EmailMessageDto message)
        {
            // Email setup
            var email = new MimeMessage();

            // From
            var senderEmail = _configuration["MailSettings:SenderEmail"];
            var senderName = _configuration["MailSettings:SenderName"];
            email.From.Add(new MailboxAddress(senderName, senderEmail));

            // To
            email.To.Add(MailboxAddress.Parse(message.To));

            // Subject
            email.Subject = message.Subject;

            // Body
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.Body
            };

            // إرسال الإيميل عبر SMTP
            using var smtp = new SmtpClient();

            var host = _configuration["MailSettings:Host"];
            var port = _configuration.GetValue<int>("MailSettings:Port");
            var username = _configuration["MailSettings:UserName"];
            var password = _configuration["MailSettings:Password"];

            await smtp.ConnectAsync(host, port, SecureSocketOptions.Auto);
            await smtp.AuthenticateAsync(username, password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}



