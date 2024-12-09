using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.Utility {
    public class EmailSender : IEmailSender {
        public string SendGridSecret { get; set; }

        public EmailSender(IConfiguration _config) {
            SendGridSecret = _config["SendGrid:SecretKey"];
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage) {
            //logic to send email
            var client = new SendGridClient(SendGridSecret);
            var from = new EmailAddress("tianhuilin45@gmail.com", "Song Book");
            var to = new EmailAddress(email);
            var message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
            return client.SendEmailAsync(message);
        }
    }
}
