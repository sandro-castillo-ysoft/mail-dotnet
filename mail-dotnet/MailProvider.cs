using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace mail_dotnet {
    public abstract class MailProvider : IMailProvider {
        protected readonly ILogger logger;
        protected readonly CancellationToken cancellationToken;

        protected MailProvider(ILogger? _logger = null, CancellationToken? _cancellationToken = null) {
            logger = _logger ?? NullLogger.Instance;
            cancellationToken = _cancellationToken ?? CancellationToken.None;
        }

        public abstract bool TestProvider(out string result);
    }
}
