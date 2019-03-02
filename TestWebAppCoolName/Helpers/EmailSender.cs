using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace TestWebAppCoolName.Helpers
{
    public class EmailSender
    {

        public async Task<bool> SendEmail(string from,string subject, string body)
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
    }
}