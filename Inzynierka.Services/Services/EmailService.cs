using AutoMapper;
using Inzynierka.Data.DbModels;
using Inzynierka.Repository.Interfaces;
using Inzynierka.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Inzynierka.Services.Services
{
    public class EmailService:IEmailService
    {
        private readonly string EmailSender = "jaro1994elo@gmail.com";
        private readonly string fromPassword = "jaro1994";
        public void SendEmailAfterRegister(string EmailAdress, string GeneratedLink, string Subject, string Username)
        {

            MailAddress fromAddress = new MailAddress(EmailSender, fromPassword);
            MailAddress toAddress = new MailAddress(EmailAdress);
            
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword);

            MailMessage message = new MailMessage(fromAddress, toAddress);
            message.IsBodyHtml = true;
            message.Subject = Subject;
            var body = new StringBuilder();
            body.AppendFormat("Witaj, {0}!\n", Username);
            body.AppendLine(@"Dziękujemy za utworzenie konta w naszym serwisie. Poniżej przesyłamy link aktywacyjny do twojego konta.
                Jeżeli to nie ty je utworzyłeś to poprostu zignoruj wiadomość");
            string link = "http://localhost:3000/register/confirm/";

            body.AppendFormat("Link: {0}", link + GeneratedLink);
            message.Body = body.ToString();

            smtp.Send(message);
        }
    }
}
