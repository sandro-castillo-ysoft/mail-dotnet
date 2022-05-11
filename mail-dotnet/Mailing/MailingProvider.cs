using Microsoft.Extensions.Logging;
using MimeKit;

namespace mail_dotnet.Mailing {
    public abstract class MailingProvider : MailProvider, IMailingProvider {
        protected MailingProvider(ILogger _logger, CancellationToken _cancellationToken) : base(_logger, _cancellationToken) {
        }

        public abstract void SendMail(MimeMessage message);
        public abstract void SendMail(string subject, string body, string mailTo);
    }
}
