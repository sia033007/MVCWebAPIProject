using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using MyMVCProject.Settings;
using System.Net;

namespace MyMVCProject.Models
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<SmtpSettings> _smtpSettings;
        public EmailSender(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings;

        }
        public async Task SendAsync(string from, string to, string subject, string body)
        {
            var message = new MailMessage(from, to, subject, body);
            using (var emailClient = new SmtpClient(_smtpSettings.Value.Host, _smtpSettings.Value.Port))
            {
                emailClient.EnableSsl = true;
                emailClient.Credentials = new NetworkCredential(
                    _smtpSettings.Value.User, _smtpSettings.Value.Password);
                await emailClient.SendMailAsync(message);

            }
        }
    }
}
