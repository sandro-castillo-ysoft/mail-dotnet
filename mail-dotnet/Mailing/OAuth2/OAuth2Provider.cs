using Microsoft.Extensions.Logging;

namespace mail_dotnet.Mailing.OAuth2 {
    public abstract class OAuth2Provider : MailingProvider {
        protected OAuth2Provider(ILogger _logger, CancellationToken _cancellationToken) : base(_logger, _cancellationToken) {
        }
    }
}
