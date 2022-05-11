using Microsoft.Extensions.Logging;
using MimeKit;

namespace mail_dotnet.Mailing.OAuth2.GoogleApis {
    public class GoogleApisProvider : OAuth2Provider {

        public GoogleApisProvider(ILogger _logger) : base(_logger) {

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
