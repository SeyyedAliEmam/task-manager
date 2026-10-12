using MemberShip.Application.IEmailsender;
using System.Net;
using System.Net.Mail;

namespace MemberShip.Infrastructure.Repositories
{
        public class Emailsender : IEmailsender
        {
            private readonly string smtpHost;
            private readonly int smtpPort;
            private readonly string userName;
            private readonly string password;
            public Emailsender()
            {
                smtpHost = "smtp.gmail.com"; 
                smtpPort = 587; 
                userName = ""; 
                password = ""; 
            }

            public void SendEmail(string recipientEmail, string subject, string body)
            {
                using (var client = new SmtpClient(smtpHost, smtpPort))
                {
                    client.Credentials = new NetworkCredential(userName, password);
                    client.EnableSsl = true;

                    using var mailMessage = new MailMessage
                    {
                        From = new MailAddress(userName),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(new MailAddress(recipientEmail));

                    client.Send(mailMessage);
                }
            }
        }
    
}
