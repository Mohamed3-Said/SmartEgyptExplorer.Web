using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Service.ServiceImplemmentation.DashboardService
{ 
    public class DashEmailService
    {
        private readonly IConfiguration _config;

        public DashEmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var settings = _config.GetSection("MailSettings");

            var smtp = new SmtpClient
            {
                Host = settings["Host"],
                Port = int.Parse(settings["Port"]!),
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    settings["UserName"],
                    settings["Password"]
                )
            };

            var message = new MailMessage
            {
                From = new MailAddress(settings["SenderEmail"], settings["SenderName"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            message.To.Add(to);

            await smtp.SendMailAsync(message);
        }
    }
}
