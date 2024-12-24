using System.Net.Mail;
using System.Net;
using System.Text;

namespace HamroFashion.Api.V1.Utils
{
    /// <summary>
    /// Really simple email sender thing
    /// </summary>
    public class Mailer
    {
        /// <summary>
        /// We load these from configuration on app start
        /// </summary>
        private static NetworkCredential? Credentials = null;

        /// <summary>
        /// We load this from configuration on app start
        /// </summary>
        private static MailAddress? FromAddress = null;

        /// <summary>
        /// Reusable client, contains all the information needed by smtpserver(ms365) to send mail to client (user)
        /// </summary>
        public static SmtpClient? Client = null;

        /// <summary>
        /// Used internally to get an smtp client for which to send a message
        /// </summary>
        /// <returns></returns>
        protected static SmtpClient BuildClient()
        {
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = Credentials;
            client.Port = 587;
            client.Host = "smtp.outlook.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;

            return client;
        }

        /// <summary>
        /// Builds a message (from was configuread at app startup)
        /// </summary>
        /// <param name="to">Email address to send to</param>
        /// <param name="subject">Subject of the message</param>
        /// <param name="body">Message to send</param>
        /// <returns>MailMessage</returns>
        public static MailMessage BuildMessage(string to, string subject, string body)
        {
            var msg = new MailMessage();
            msg.To.Add(new MailAddress(to));
            Console.WriteLine(FromAddress.Address.ToString());
            msg.From = FromAddress;
            msg.Subject = subject;
            msg.Body = body;
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;

            return msg;
        }

        /// <summary>
        /// Call this in app startup to configure the sensitive bits
        /// </summary>
        /// <param name="config">appsettings condiguration</param>
        public static void Configure(IConfiguration config)
        {
            var username = config["Email:Username"];
            var password = config["Email:Password"];
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("SMTP credentials are not configured.");
            }
            var fromAddress = config["Email:FromAddress"];
            if (string.IsNullOrWhiteSpace(fromAddress))
            {
                throw new InvalidOperationException("Sender email address is not configured.");
            }

            var fromName = config["Email:FromName"];

            Credentials = new NetworkCredential(username, password);
            FromAddress = new MailAddress(fromAddress, fromName);
            Client = BuildClient();
        }

        /// <summary>
        /// Sends an email message
        /// </summary>
        /// <param name="to">Email address to send to</param>
        /// <param name="subject">Subject of the message</param>
        /// <param name="body">Message to send</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken)
        {
            var msg = BuildMessage(to, subject, body);
            return SendAsync(msg, cancellationToken);
        }

        /// <summary>
        /// Sends a MailMessage
        /// </summary>
        /// <param name="msg">The message to send</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SendAsync(MailMessage msg, CancellationToken cancellationToken)
        {
            return Client.SendMailAsync(msg, cancellationToken);
        }
    }
}
