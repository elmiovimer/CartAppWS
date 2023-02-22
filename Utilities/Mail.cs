using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CartAppWS.Utilities
{
    public class Mail
    {
        public static bool SendMail(
            string from, 
            string to, 
            string subject, 
            string body, 
            string host, 
            string pwd, 
            int port, 
            bool tsl)
        {
           
            MailMessage email = new();
            email.To.Add(to);
            email.From = new MailAddress(from);
            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;
            SmtpClient smtp = new ();
            smtp.Host = host;
            smtp.Port = port;
            smtp.EnableSsl = tsl;
            smtp.UseDefaultCredentials = false;
           // smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(from, pwd);

            try
            {
                smtp.Send(email);
                email.Dispose();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
