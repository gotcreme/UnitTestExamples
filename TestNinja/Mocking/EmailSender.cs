using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace TestNinja.Mocking
{
    public interface IEmailSender
    {
        void EmailFile(string emailAdrress, string emailBody, string filename, string subject);
    }

    public class EmailSender : IEmailSender
    {
        public void EmailFile(string emailAdrress, string emailBody, string filename, string subject)
        {
            var client = new SmtpClient(SystemSettingsHelper.EmailSmtpHost)
            {
                Port = SystemSettingsHelper.EmailPort,
                Credentials = new NetworkCredential(
                    SystemSettingsHelper.EmailUsername,
                    SystemSettingsHelper.EmailPassword)
            };

            var from = new MailAddress(SystemSettingsHelper.EmailFromEmail, SystemSettingsHelper.EmaiDisplayName);
            var to = new MailAddress(emailAdrress);

            var message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = emailBody,
                BodyEncoding = Encoding.UTF8
            };

            message.Attachments.Add(new Attachment(filename));
            client.Send(message);
            message.Dispose();

            File.Delete(filename);
        }
    }

    public static class SystemSettingsHelper
    {
        public static string EmailSmtpHost = "";
        public static int EmailPort = 1;
        public static string EmailUsername = "";
        public static string EmailPassword = "";
        public static string EmailFromEmail = "";
        public static string EmaiDisplayName = "";
    }
}