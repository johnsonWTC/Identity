using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace WebApplication4.Services
{
   public  interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }

    public class SendGridMailService : IMailService
    {
        private IConfiguration _configaration;

        public SendGridMailService(IConfiguration configaration)
        {
            _configaration = configaration;
        }



        public  async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress("Admin",
            "admin@johnsondubula.com");
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress("User",
            "jdubula@student.wethinkcode.co.za");
            message.To.Add(to);

            message.Subject = "This is email subject";
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = "Hello World!";

            message.Body = bodyBuilder.ToMessageBody();

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smtpClient.Connect("smtp.gmail.com", 587);
                smtpClient.Authenticate("johnsondubula@gmail.com", "Jamgod1234554321!!!!!");
                smtpClient.Send(message);
                smtpClient.Disconnect(true);
            }





            //var apiKey = _configaration["SendGripkey"];
            //var client = new SendGridClient(apiKey);
            //var from = new EmailAddress("test@testMail.com", "Test Mail");
            //var to = new EmailAddress("test@example.com", "Example User");
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            //var response = await client.SendEmailAsync(msg);
        }
    }
}
