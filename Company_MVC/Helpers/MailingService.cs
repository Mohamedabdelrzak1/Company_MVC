

using Company.DAL.Models;
using Company_MVC.Helpers;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using SendEmailsWithDotNet8.Settings;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SendEmailsWithDotNet8.Services
{
    public class MailingService(IOptions<MailSettings> options) : IMailingService
    {

        public void SendEmail(Email email)
        {
            //Build Massege
            var mail = new MimeMessage();

            mail.Subject = email.Subject;
            mail.From.Add(MailboxAddress.Parse(options.Value.Email));
            mail.To.Add(MailboxAddress.Parse(email.To));


            var builder = new BodyBuilder();
            builder.TextBody = email.Body;
            mail.Body = builder.ToMessageBody();


            //Establish Connection 

            using var smtp = new SmtpClient();
            smtp.Connect(options.Value.Host,options.Value.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(options.Value.Email, options.Value.Password);


            //Send Email 
            smtp.Send(mail);

        }    
    }
}
