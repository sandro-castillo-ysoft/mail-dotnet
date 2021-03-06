using Microsoft.VisualStudio.TestTools.UnitTesting;
using mail_dotnetTests.Mailing.SMTP;

namespace mail_dotnet.Mailing.SMTP.Tests {
    [TestClass()]
    public class SMTPProviderTests {
        [TestMethod()]
        public void SendMailTest() {
            MailingProvider provider = new SMTPProvider() {
                Host = TestVariables.HOST,
                Port = TestVariables.PORT,
                SslOption = TestVariables.SSL_OPTION,
                Username = TestVariables.USERNAME,
                Password = TestVariables.PASSWORD,
            };

            provider.SendMail(
                subject: "Test Subject",
                body: "Test Body",
                mailTo: mail_dotnetTests.Mailing.TestVariables.ADDRESS_WORKING_1
                );

            Assert.IsTrue(true);
        }
    }
}