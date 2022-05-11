using MimeKit;

namespace mail_dotnet.Reading {
    public interface IReadingProvider {
        /// <returns>List of message's Unique IDs</returns>
        public string[] GetInboxList(bool unreadOnly = true);
        /// <returns>Filtered Inbox List</returns>
        public string[] GetInboxList(string fromRegex = ".*", string subjectRegex = ".*", bool unreadOnly = true);
        /// <returns>List of message's Subjects in inbox</returns>
        public string[] GetInboxListSubjects();
        public MimeMessage GetMail(string id, bool markRead = true, bool deleteAfter = true);
    }
}
