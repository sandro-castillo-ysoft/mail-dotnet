using MimeKit;
using MimeKit.Text;

namespace mail_dotnet.Commons {
    public static class Util {
        /// <param name="body">HTML Body</param>
        /// <param name="attachementFiles">List of full paths to files to attach</param>
        public static MimeMessage CreateMimeMessage(string? subject = null, string? body = null, bool htmlBody = true, string? mailFrom = null, string[]? mailTo = null, string[]? mailCc = null, string[]? mailBcc = null, string[]? attachementFiles = null) {
            MimeMessage message = new();
            Multipart bodyContent = new("mixed");

            TextPart bodyPart = new(htmlBody ? TextFormat.Html : TextFormat.Plain) {
                Text = body ?? string.Empty
            };
            bodyContent.Add(bodyPart);

            if (attachementFiles is not null)
                LoadAttachementsToMultipart(attachementFiles, bodyContent);

            if (mailTo is not null)
                message.To.AddRange(ToInternetAddressList(mailTo));

            if (mailCc is not null)
                message.To.AddRange(ToInternetAddressList(mailCc));

            if (mailBcc is not null)
                message.Bcc.AddRange(ToInternetAddressList(mailBcc));

            if (mailFrom is not null)
                message.From.Add(InternetAddress.Parse(mailFrom));

            message.Subject = subject ?? string.Empty;
            message.Body = bodyContent;

            return message;
        }

        /// <param name="decorators">Must prefix and affix text to replace.</param>
        public static string PopulateTemplate(string template, Dictionary<string, string> keyValuePairs, string decorators = "##") {
            string newBody = template;

            foreach (var entry in keyValuePairs) {
                if (entry.Key is null || entry.Value is null)
                    continue;

                string key = $"{decorators}{entry.Key}{decorators}";
                newBody = newBody.Replace(key, entry.Value);
            }

            return newBody;
        }

        private static InternetAddressList ToInternetAddressList(string[] mailList) {
            InternetAddressList addresses = new();

            foreach (string stringAddress in mailList) {
                if (InternetAddress.TryParse(stringAddress, out InternetAddress address))
                    addresses.Add(address);
            }

            return addresses;
        }

        private static void LoadAttachementsToMultipart(string[] attachementFiles, Multipart multipart) {

            foreach (string file in attachementFiles) {

                try {
                    if (!File.Exists(file))
                        continue;

                    using FileStream fileStream = File.OpenRead(file);

                    MimePart attachmentPart = new() {
                        Content = new MimeContent(fileStream),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = Path.GetFileName(file)
                    };
                    multipart.Add(attachmentPart);

                    fileStream.Close();

                } catch (Exception) {
                    // Handle exception? what do
                }

            }
        }
    }
}
