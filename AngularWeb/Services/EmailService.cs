using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using AngularWeb.DataRepo;

namespace AngularWeb.Services
{
    public static class EmailService
    {
        public static void SendEmailForWeek(int week)
        {
            var users = UserRepo.GetUsers();

            foreach (var user in users)
            {
                if (user.SendEmail)
                {
                    SendEmailToUser(week, user.Email);
                }
            }
        }

        private static void SendEmailToUser(int week, string email)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("fantasycollege236@gmail.com", "Actiondude2345!");

            using (var message = new MailMessage("FantasyCF@CF.com", email))
            {
                message.Subject = "Make your picks for week " + week;
                message.Body = "Games have been selected!\n\nMake your picks for the week before the deadline!";
                message.IsBodyHtml = true;
                smtp.Send(message);
            }
        }
        public static void SendForgotPasswordEmail(string email)
        {
            string hashed = EncryptService.Encrypt(email, "astrophile");
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("fantasycollege236@gmail.com", "Actiondude2345!");

            using (var message = new MailMessage("FantasyCF@CF.com", email))
            {
                message.Subject = "Forgot Password";
                message.Body = "Click the link to reset your password<Br>" +
                               "https://fantasysport-4881.nodechef.com/resetPassword?data=" + hashed;
                message.IsBodyHtml = true;
                smtp.Send(message);
            }
        }
    }
}
