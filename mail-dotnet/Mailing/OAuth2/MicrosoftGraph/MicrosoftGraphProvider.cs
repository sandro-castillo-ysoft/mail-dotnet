using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using MimeKit;

namespace mail_dotnet.Mailing.OAuth2.MicrosoftGraph {
    public class MicrosoftGraphProvider : OAuth2Provider {
        private string TenantId { get; }
        private string ClientId { get; }
        private string UserId { get; }
        private string ClientSecret { get; }
        private ClientSecretCredentialOptions Options { get; }
        private GraphServiceClient? Client;

        /// Initializes a Confidential Client Application for a AcquireTokenForClient workflow.
        /// <param name="userId">Account Client ID from Azure AD that will send the eMail.</param>
        /// <param name="scopes">Defaults to: https://graph.microsoft.com/.default .</param>
        public MicrosoftGraphProvider(
            string tenantId, string clientId, string clientSecret, string userId,
            string[]? scopes = null, Uri? authority = null,
            ILogger? _logger = null, CancellationToken? _cancellationToken = null) : base(_logger, _cancellationToken
                ) {

            TenantId = tenantId;
            ClientId = clientId;
            UserId = userId;
            ClientSecret = clientSecret;

            Scopes = scopes;

            Options = new() {
                AuthorityHost = authority ?? AzureAuthorityHosts.AzurePublicCloud
            };
        }

        public override void ConnectToServer() {

            ClientSecretCredential credential = new(
                TenantId,
                ClientId,
                ClientSecret,
                Options
                );

            Client = new(credential, Scopes);

        }

        public override void SendMail(MimeMessage message) {
            logger.LogTrace("Sending mail to [{0}] from [{1}] with subject [{2}]", message.To, message.From, message.Subject);
            ConnectToServer();

            if(Client is null) {
                logger.LogError("Client must not be null before call to send mail.");
                throw new NullReferenceException("GraphServiceClient was null at the moment of the call.");
            }

            try {

                Client.Users[UserId]
                    .SendMail(ToGraphMessage(message))
                    .Request()
                    .PostAsync()
                    .Wait();

            } catch (AggregateException ex) {
                foreach (var ie in ex.InnerExceptions) {
                    if (ie.InnerException is not null && ie.InnerException.Message.Contains("AADSTS9002313"))
                        logger.LogError(ie, "Could not authenticate user [Code: AADSTS9002313]. Check Azure permissions?");

                    else
                        logger.LogError(ie, "Error sending mail through GraphServiceClient using id [{0}].", UserId);
                }

                throw;
            }
        }

        public override void SendMail(string subject, string body, string mailTo, string? mailFrom = null) {
            string[] mailToList = mailTo.Split(',', StringSplitOptions.RemoveEmptyEntries);

            MimeMessage message = Commons.Util.CreateMimeMessage(subject: subject, body: body, mailFrom: mailFrom, mailTo: mailToList);

            SendMail(message);
        }

        public override bool TestProvider(out string result) {
            throw new NotImplementedException();
        }

        static Message ToGraphMessage(MimeMessage mimeMessage) {
            BodyType bodyType;
            string bodyContent;
            List<Recipient> mailTo = ToGraphRecipient(mimeMessage.To.Mailboxes);
            List<Recipient> mailCc = ToGraphRecipient(mimeMessage.Cc.Mailboxes);
            List<Recipient> mailBcc = ToGraphRecipient(mimeMessage.Bcc.Mailboxes);

            if (mimeMessage.HtmlBody is not null) {
                bodyType = BodyType.Html;
                bodyContent = mimeMessage.HtmlBody;
            } else {
                bodyType = BodyType.Text;
                bodyContent = mimeMessage.TextBody;
            }

            Message message = new() {
                Subject = mimeMessage.Subject,
                Body = new ItemBody() {
                    ContentType = bodyType,
                    Content = bodyContent
                },
                ToRecipients = mailTo,
                CcRecipients = mailCc,
                BccRecipients = mailBcc
            };


            return message;
        }

        static List<Recipient> ToGraphRecipient(IEnumerable<MailboxAddress> mailboxes) {
            List<Recipient> recipients = new();

            foreach (MailboxAddress mailbox in mailboxes) {
                Recipient recipient = new() {
                    EmailAddress = new EmailAddress {
                        Name = mailbox.Name,
                        Address = mailbox.Address
                    }
                };

                recipients.Add(recipient);
            }

            return recipients;
        }
    }
}
