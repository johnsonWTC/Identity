using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var apiKey = _configaration["SendGripkey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("test@testMail.com", "Test Mail");
            var to = new EmailAddress("test@example.com", "Example User");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
