using mail_dotnet;
using mail_dotnet.Mailing;
using Microsoft.Extensions.Configuration;
using System;

namespace MyApp {
    internal class Program {
        private static string MailTo;
        private static string Subject;
        private static string Body;

        static void Main(string[] args) {

            if (args.Length != 1 && args.Length != 3) {
                Console.WriteLine("Arguments needed: 1 or 3");
                Console.WriteLine("Mail To (Required): \"target@mail.com\"");
                Console.WriteLine("Subject: \"Test Email\"");
                Console.WriteLine("Body: \"This is a test body.\" or \"path/to/body.txt\"");

                Environment.Exit(0);
            }

            if (args.Length == 1) {
                Console.WriteLine("Sending test email to: [{0}]", args[0]);
                MailTo = args[0].Replace("\"", "");
                Subject = "Test Email";
                Body = string.Format("This mail was sent using [{0}].", typeof(MailProvider).Assembly.GetName());
            }

            if(args.Length == 3) {
                Console.WriteLine("Sending email to [{0}], with subject: [{1}] and body: [{2}]", args[0], args[1], args[2]);
                MailTo = args[0].Replace("\"", "");
                Subject = args[1].Replace("\"", "");
                Body = args[2].Replace("\"", "");
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();
            Configuration configuration = config.Get<Configuration>();


            MailingProvider? provider = null;
            switch (configuration.Provider.ToLower()) {

                case "smtp":
                    provider = new mail_dotnet.Mailing.SMTP.SMTPProvider(
                        configuration.SMTPOptions.Username,
                        configuration.SMTPOptions.Password,
                        configuration.SMTPOptions.Host,
                        configuration.SMTPOptions.Port ?? 587,
                        configuration.SMTPOptions.SslOption ?? "StartTLS"
                        );

                    break;

                case "graph":
                    provider = new mail_dotnet.Mailing.OAuth2.MicrosoftGraph.MicrosoftGraphProvider(
                        configuration.GraphOptions.TenantId,
                        configuration.GraphOptions.ClientId,
                        configuration.GraphOptions.ClientSecret,
                        configuration.GraphOptions.UserId
                        );

                    break;

                case "google":
                    provider = new mail_dotnet.Mailing.OAuth2.GoogleApis.GoogleApisProvider(
                        configuration.GoogleOptions.ServiceAccount,
                        configuration.GoogleOptions.ImpersonatedAccount,
                        configuration.GoogleOptions.PrivateKey
                        );

                    break;
                    
                default:
                    Console.WriteLine("Unknown mail provider, options: smtp, graph or google.");
                    Environment.Exit(0);
                    break;
            }
            
            provider?.SendMail(Subject, Body, MailTo);
            Console.WriteLine("Mail has been sent, check your inbox.");

        }

        private class Configuration {
            public string Provider { get; set; }
            public SMTPOptions SMTPOptions {get;set;}
            public GraphOptions GraphOptions { get; set; }
            public GoogleOptions GoogleOptions { get; set; }
        }

        public class SMTPOptions {
            public string Host { get; set; }
            public int? Port { get; set; }
            public string? SslOption { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class GraphOptions {
            public string TenantId { get; set; }
            public string ClientId { get; set; }
            public string UserId { get; set; }
            public string ClientSecret { get; set; }
        }

        public class GoogleOptions {
            public string ServiceAccount { get; set; }
            public string ImpersonatedAccount { get; set; }
            public string PrivateKey { get; set; }
            public string AppName { get; set; } = "Mail-DotNet Daemon";
        }

    }
}