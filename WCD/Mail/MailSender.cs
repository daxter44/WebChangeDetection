using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace WCDWpf.Mail
{
    public static class MailSender
    {
        public static async Task sendMail(String address, String url)
        {
            try
            {

                // Send the message 
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("WebChangeDetector@interia.pl"));
                message.To.Add(new MailboxAddress(address));
                message.Subject = "Welcome! Your Website has changed";

                message.Body = new TextPart("plain")
                {
                    Text = @"Hey,

Your website has changed ! Check this out: " + url + @"

-- WCDapp"
                };

                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync("poczta.interia.pl", 465, true);

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync("WebChangeDetector@interia.pl", "pass");

                    await client.SendAsync(message);
                    client.Disconnect(true);
                }

                // Clean up
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not send e-mail. Exception caught: " + e);
            }


        }
    }
}

