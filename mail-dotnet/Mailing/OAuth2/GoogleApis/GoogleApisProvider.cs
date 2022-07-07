using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace mail_dotnet.Mailing.OAuth2.GoogleApis {
    public class GoogleApisProvider : OAuth2Provider {
        private string ServiceAccountEmail { get; }
        private string ImpersonatedAccountEmail { get; }
        private string PrivateKey { get; }
        private string ApplicationName { get; }

        private GmailService? Client;

        /// <summary>
        /// Initializes a Service Account to send emails by impersonating a user.
        /// </summary>
        /// <param name="scopes">Defaults to: https://www.googleapis.com/auth/gmail.send .</param>
        public GoogleApisProvider(
            string serviceAccountEmail, string impersonatedAccountEmail, string privateKey,
            string applicationName = "Email Daemon", string[]? scopes = null,
            ILogger? _logger = null, CancellationToken? _cancellationToken = null) : base(_logger, _cancellationToken) {
            ServiceAccountEmail = serviceAccountEmail;
            ImpersonatedAccountEmail = impersonatedAccountEmail;
            PrivateKey = privateKey;
            ApplicationName = applicationName;

            Scopes = scopes ?? new string[] { GmailService.Scope.GmailSend };
        }

        public override void ConnectToServer() {
            ServiceAccountCredential credentials = new(new ServiceAccountCredential.Initializer(ServiceAccountEmail) {
                Scopes = Scopes,
                User = ImpersonatedAccountEmail
            }.FromPrivateKey(PrivateKey));

            Client = new(new BaseClientService.Initializer() { 
                HttpClientInitializer = credentials,
                ApplicationName = ApplicationName
            });

        }

        public override void SendMail(MimeMessage message) {
            logger.LogTrace("Sending mail to [{0}] from [{1}] with subject [{2}]", message.To, message.From, message.Subject);
            ConnectToServer();

            if (Client is null) {
                logger.LogError("Client must not be null before call to send mail.");
                throw new NullReferenceException("GmailService was null at the moment of the call.");
            }

            Message gmailMessage = new() {
                Raw = Encode(message)
            };

            Client.Users.Messages
                .Send(gmailMessage, "me")
                .Execute();
        }

        public override void SendMail(string subject, string body, string mailTo, string? mailFrom = null) {
            string[] mailToList = mailTo.Split(',', StringSplitOptions.RemoveEmptyEntries);

            MimeMessage message = Commons.Util.CreateMimeMessage(subject: subject, body: body, mailFrom: mailFrom, mailTo: mailToList);

            SendMail(message);
        }

        public override bool TestProvider(out string result) {
            throw new NotImplementedException();
        }

        private static string Encode(MimeMessage mimeMessage) {
            using MemoryStream ms = new();
            mimeMessage.WriteTo(ms);
            return Convert.ToBase64String(ms.GetBuffer())
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }
}
