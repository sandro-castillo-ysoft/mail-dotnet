using Microsoft.Extensions.Logging;
using MimeKit;

namespace mail_dotnet.Reading {
    public abstract class ReadingProvider : MailProvider, IReadingProvider {
        protected ReadingProvider(ILogger _logger, CancellationToken _cancellationToken) : base(_logger, _cancellationToken) {
        }

        public abstract string[] GetInboxList(bool unreadOnly = true);
        public abstract string[] GetInboxList(string fromRegex = ".*", string subjectRegex = ".*", bool unreadOnly = true);
        public abstract string[] GetInboxListSubjects();
        public abstract MimeMessage GetMail(string id, bool markRead = true, bool deleteAfter = true);
    }
}
