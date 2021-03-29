using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Services
{
    interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }

    public class SendGridMailService : IMailService
    {

    }
}
