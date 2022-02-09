using SendGrid;
using System.Configuration;
using System.Net.Mail;

namespace CoderDojoMKE.Web.Utilities
{
    public static class MailHelper
    {
        /// <summary>
        /// Sends an outbound email with the provided params
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fromEmail"></param>
        /// <param name="destination"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static SendGridMessage GetMailMessage(string friendlyName, string fromEmail, string destination, string subject, string message)
        {
            var mail = new SendGridMessage();
            mail.From = new MailAddress(fromEmail, friendlyName);
            mail.AddTo(destination);
            mail.Subject = subject;
            mail.Text = message;
            mail.Html = message;
            return mail;
        }

        /// <summary>
        /// Sends an "inbound" email with the provided params to info@coderdojomke.org
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static SendGridMessage GetMailMessage(string name, string email, string message)
        {
            var mail = new SendGridMessage();
            mail.From = new MailAddress(email);
            mail.AddTo(ConfigurationManager.AppSettings["InformationEmail"]);
            mail.Subject = string.Format("Message from {0}", name);
            mail.Text = message;
            mail.Html = message;
            return mail;
        }

        public static void Send(SendGridMessage message)
        {
            var transportWeb = new SendGrid.Web("");
            transportWeb.DeliverAsync(message);
        }
    }
}