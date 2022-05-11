using Microsoft.Extensions.Logging;
using MimeKit;

namespace mail_dotnet.Mailing.OAuth2.GoogleApis {
    public class GoogleApisProvider : OAuth2Provider {

        public GoogleApisProvider(ILogger _logger, CancellationToken _cancellationToken) : base(_logger, _cancellationToken) {

        }

        public override void SendMail(MimeMessage message) {
            throw new NotImplementedException();
        }

        public override void SendMail(string subject, string body, string mailTo, string? mailFrom = null) {
            throw new NotImplementedException();
        }

        public override bool TestProvider(out string result) {
            throw new NotImplementedException();
        }
    }
}
