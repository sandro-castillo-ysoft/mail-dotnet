using Microsoft.Extensions.Logging;
using MimeKit;

namespace mail_dotnet.Mailing.SMTP {
    public class SMTPProvider : MailingProvider {

        public SMTPProvider(ILogger _logger) : base(_logger) {
        }

        public override void SendMail(MimeMessage message) {
            throw new NotImplementedException();
        }

        public override void SendMail(string subject, string body, string mailTo) {
            throw new NotImplementedException();
        }

        public override bool TestProvider(out string result) {
            throw new NotImplementedException();
        }
    }
}
