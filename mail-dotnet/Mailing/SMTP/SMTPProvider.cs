using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace mail_dotnet.Mailing.SMTP {
    /// <summary>
    /// Powered by MailKit
    /// https://github.com/jstedfast/MailKit
    /// </summary>
    public class SMTPProvider : MailingProvider {
        private readonly SmtpClient Client;

        public string Host { get; init; }
        public short Port { get; init; }
        public SecureSocketOptions SslOption { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }

        public SMTPProvider(ILogger _logger, CancellationToken _cancellationToken) : base(_logger, _cancellationToken) {
            Client = new();
        }

        private void ConnectToSmtpServer() {
            logger.LogTrace("Connecting to SMTP Server: [{0}:{1}]", Host, Port);
            Client.Connect(Host, Port, SslOption, cancellationToken);

            try {

                logger.LogTrace("Authenticating to SMTP Server", Username);
                Client.Authenticate(Username, Password, cancellationToken);

            } catch (Exception ex) {

                if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password)) {
                    logger.LogError("Authentication to SMTP Server failed as [{0}]", Username);

                    throw new ArgumentException("Could not connect to server with provided crendetials.", ex);
                }

            }
            
        }

        public override void SendMail(MimeMessage message) {
            logger.LogTrace("Sending mail to [{0}] from [{1}] with subject [{2}]", message.To, message.From, message.Subject);
            ConnectToSmtpServer();

            string response = Client.Send(message, cancellationToken);
            logger.LogInformation("Mail sent with response: {0}", response);

            Client.Disconnect(true, cancellationToken);
        }

        public override void SendMail(string subject, string body, string mailTo) {
            string[] mailToList = mailTo.Split(',', StringSplitOptions.RemoveEmptyEntries);

            MimeMessage message = Commons.Util.CreateMimeMessage(subject: subject, body: body, mailTo: mailToList);

            SendMail(message);
        }

        public override bool TestProvider(out string result) {
            // how to better test?
            // 1. send mail to admin
            // 2. authenticate against server
            throw new NotImplementedException();
        }
    }
}
