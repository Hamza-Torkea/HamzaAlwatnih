using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace alwatnia.Areas.Cpanel.Services
{
    public class EmailSender
    {
        public string Sub { get; set; }
        public string Body { get; set; }
        public List<string> To { get; set; }

        public async Task SendEmailAsync(EmailCredintials credentials)
        {
            try
            {
                //From Address 
                var fromAddress = credentials.From;
                var fromPassword = credentials.Password;
                var fromAdressTitle = credentials.From;

                //To Address 
                var subject = Sub;
                var bodyContent = Body;

                var smtpServer = credentials.Server;
                var smtpPortNumber = credentials.Port;

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(fromAdressTitle, fromAddress));
                To.ForEach(e => mimeMessage.To.Add(new MailboxAddress(e, e)));
                mimeMessage.Subject = subject;
                mimeMessage.Body = new TextPart("html")
                {
                    Text = bodyContent
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(smtpServer, smtpPortNumber, false);
                    await client.AuthenticateAsync(credentials.Username, credentials.Password);
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception)
            {
                // ignore
            }
        }
    }

    public class EmailCredintials
    {
        public int Port { get; set; }
        public string Server { get; set; }
        public string From { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
    }
}