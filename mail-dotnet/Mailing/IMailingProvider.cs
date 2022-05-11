using MimeKit;

namespace mail_dotnet.Mailing {
    public interface IMailingProvider {
        public void SendMail(MimeMessage message);
        public void SendMail(string subject, string body, string mailTo, string? mailFrom = null);
    }
}
