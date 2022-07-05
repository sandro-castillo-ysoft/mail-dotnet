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
        private readonly SmtpClient Client = new();

        public string Host { get; }
        public short Port { get; }
        public string SslOption { get; }
        public string Username { get; }
        public string Password { get; }

        // to-do: Needs a way to load configuration
        // Probably an interface load to keep it easy to read
        public SMTPProvider(
            string host, string username, string password,
            short port = 587, string? sslOption = null,
            ILogger? _logger = null, CancellationToken? _cancellationToken = null) : base(_logger, _cancellationToken) {

            Host = host;
            Port = port;
            Username = username;
            Password = password;

            SslOption = sslOption ?? SecureSocketOptions.StartTls.ToString();
        }

        public override void ConnectToServer() {
            if (!Enum.TryParse(SslOption, out SecureSocketOptions sslOption))
                sslOption = SecureSocketOptions.Auto;

            logger.LogTrace("Connecting to SMTP Server: [{0}:{1}]", Host, Port);
            Client.Connect(Host, Port, sslOption, cancellationToken);

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
            ConnectToServer();

            string response = Client.Send(message, cancellationToken);
            logger.LogInformation("Mail sent with response: {0}", response);

            Client.Disconnect(true, cancellationToken);
        }

        public override void SendMail(string subject, string body, string mailTo, string? mailFrom = null) {
            string[] mailToList = mailTo.Split(',', StringSplitOptions.RemoveEmptyEntries);

            MimeMessage message = Commons.Util.CreateMimeMessage(subject: subject, body: body, mailFrom: mailFrom ?? Username, mailTo: mailToList);

            SendMail(message);
        }

        public override bool TestProvider(out string result) {
            // to-do: how to better test?
            // 1. send mail to admin
            // 2. authenticate against server
            throw new NotImplementedException();
        }

    }
}
