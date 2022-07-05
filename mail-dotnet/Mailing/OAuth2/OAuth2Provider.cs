using Microsoft.Extensions.Logging;

namespace mail_dotnet.Mailing.OAuth2 {
    public abstract class OAuth2Provider : MailingProvider {
        protected readonly string CacheDirectory = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "cache");
        protected string CacheFileName = "oauth";

        protected string[]? Scopes;

        protected OAuth2Provider(ILogger? _logger, CancellationToken? _cancellationToken) : base(_logger, _cancellationToken) {
        }
    }
}
