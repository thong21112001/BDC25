using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace Common
{
    public class MailHelper
    {
        public void SendEmail(string toEmail,string subject,string content)
        {
            var fromEmail = ConfigurationManager.AppSettings["FromEmailAd"].ToString();
            var fromEmailDisN = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
            var fromEmailPass = ConfigurationManager.AppSettings["FromEmailPass"].ToString();
            var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
            var smtpHort = ConfigurationManager.AppSettings["SMTPHost"].ToString();

            bool enaSSL = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());

            string body = content;
            MailMessage MessM = new MailMessage(new MailAddress(fromEmail, fromEmailDisN),new MailAddress(toEmail));
            MessM.Subject = subject;
            MessM.IsBodyHtml = true;
            MessM.Body = body;

            var cilent = new SmtpClient();
            cilent.Credentials = new NetworkCredential(fromEmail, fromEmailPass);
            cilent.Host= smtpHort;
            cilent.EnableSsl= enaSSL;
            cilent.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            cilent.Send(MessM);
        }

        public void SendWelcome(string toEmail, string subject, string content)
        {
            var fromEmail = ConfigurationManager.AppSettings["FromEmailAd"].ToString();
            var fromEmailDisN = ConfigurationManager.AppSettings["FromEmailWelcome"].ToString();
            var fromEmailPass = ConfigurationManager.AppSettings["FromEmailPass"].ToString();
            var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
            var smtpHort = ConfigurationManager.AppSettings["SMTPHost"].ToString();

            bool enaSSL = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());

            string body = content;
            MailMessage MessM = new MailMessage(new MailAddress(fromEmail, fromEmailDisN), new MailAddress(toEmail));
            MessM.Subject = subject;
            MessM.IsBodyHtml = true;
            MessM.Body = body;

            var cilent = new SmtpClient();
            cilent.Credentials = new NetworkCredential(fromEmail, fromEmailPass);
            cilent.Host = smtpHort;
            cilent.EnableSsl = enaSSL;
            cilent.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            cilent.Send(MessM);
        }
    }
}
