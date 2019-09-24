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
    public class EmailSender {
        private const string SenderAdress = "neodpovidat@vkurzu.cz";

        public async Task<bool> SendEmail(string from, string subject, string body)
        {
            if (string.IsNullOrEmpty(from))
            {
                return false;
            }
            try
            {

                var client = new SmtpClient("smtp.forpsi.com", 587) //465
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(SenderAdress, "Aaaa1111@"),

                };
                // odeslání emailu (od koho, komu, předmět, zpráva)
                await client.SendMailAsync(SenderAdress, "janfujdiar@seznam.cz, daniel.hruska@me.com, podpora@vkurzu.cz", subject, body);
               
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

                using (var client = new SmtpClient("smtp.forpsi.com", 587)
                { EnableSsl = true, Credentials = new NetworkCredential(SenderAdress, "Aaaa1111@") })
                {

                    // odeslání emailu (od koho, komu, předmět, zpráva)
                    await client.SendMailAsync(new MailMessage()
                    {
                        From = new MailAddress(SenderAdress),
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