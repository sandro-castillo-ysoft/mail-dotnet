using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace mail_dotnet {
    public abstract class MailProvider : IMailProvider {
        protected readonly ILogger logger;

        protected MailProvider(ILogger? _logger) {
            logger = _logger ?? NullLogger.Instance;
        }

        public abstract bool TestProvider(out string result);
    }
}
