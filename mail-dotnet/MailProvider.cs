using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace mail_dotnet {
    public abstract class MailProvider : IMailProvider {
        protected readonly ILogger logger;
        protected readonly CancellationToken cancellationToken;

        protected MailProvider(ILogger? _logger, CancellationToken _cancellationToken) {
            logger = _logger ?? NullLogger.Instance;
            cancellationToken = _cancellationToken;
        }

        public abstract bool TestProvider(out string result);
    }
}
