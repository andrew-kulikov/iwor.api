using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace iwor.core.Services
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
    }

    public interface IEmailSender
    {
        void SendEmail(string email, string subject, string message);
    }

    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public void SendEmail(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("iwor.sender@gmail.com", "123qweA!"),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress("iwor.sender@gmail.com"), Subject = subject, Body = message
            };

            mail.To.Add(email);

            client.Send(mail);
        }
    }
}