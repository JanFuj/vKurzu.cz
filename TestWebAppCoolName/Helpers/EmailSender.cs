using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TestWebAppCoolName.Helpers
{
    public class EmailSender
    {

        public async Task<bool> SendEmail(string from, string subject, string body)
        {
            if (string.IsNullOrEmpty(from))
            {
                return false;
            }
            try
            {

                var client = new SmtpClient("smtp.gmail.com", 587) //465
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential("bracketstest111@gmail.com", "Aaaa1111"),

                };
                // odeslání emailu (od koho, komu, předmět, zpráva)
                await client.SendMailAsync("bracketstest111@gmail.com", "janfujdiar@seznam.cz", subject, body);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public async Task<bool> SendEmailConfirmation(string subject, string body, string recipient)
        {

            try
            {

                using (var client = new SmtpClient("smtp.gmail.com", 587)
                { EnableSsl = true, Credentials = new NetworkCredential("bracketstest111@gmail.com", "Aaaa1111") })
                {

                    // odeslání emailu (od koho, komu, předmět, zpráva)
                    await client.SendMailAsync(new MailMessage()
                    {
                        From = new MailAddress("bracketstest111@gmail.com"),
                        Subject = subject,
                        To = { recipient },
                        Body = body,
                        BodyEncoding = Encoding.UTF8,
                        IsBodyHtml = true,

                    });
                   // await client.SendMailAsync("bracketstest111@gmail.com", recipient, subject, body);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}