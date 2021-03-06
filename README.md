Net 6 Library for accessing email through different mailing providers.

# Usage
[Send](#sending-mails) | [Logging](#attach-a-logger) | [Complex Messages](#send-a-prepared-mimemessage)


## Sending Mails
[SMTP](#smtp-send) | [Office 365](#oauth2-send) | [Google Apis](#google-apis)

### Send Mail
Send an email using a MimeMessage class or passing string arguments as parameters

#### SMTP Send
```
MailingProvider provider = new SMTPProvider(username, password, host, port, sslOption);
  
provider.SendMail(subject, body, mailTo, mailFrom);
```
Ssl Option accepted values: None, Auto, StartTls, SslOnConnect, StartTlsWhenAvailable

#### OAuth2 Send
Allows sending emails by impersonating a user, this works only in a domain setting.


##### Microsoft Graph (aka Office365)
Requires an Office365 impersonating account and App Registration created through Azure Portal.
App Registration must be given Administrator Consent and acces to https://graph.microsoft.com/Mail.Send .
```
MailingProvider provider = new MicrosoftGraphProvider(
    TenantId, ClientId, SecretValue, UserId
  );
  
provider.SendMail(subject, body, mailTo, mailFrom);

// mailFrom and UserID must be from the same account.
```

##### Google Apis
Requires a Google Workspace service account and impesonating account.
Service Account must be given domain-wide delegation to https://www.googleapis.com/auth/gmail.send .
```
MailingProvider provider = new GoogleApisProvider(
    ServiceAccountEmail, ImpersonatedAccountEmail, CredentialPrivateKey
  );
```


## Attach a Logger
Provider constructors allow passing an ILogger parameter. Example using SMTP Provider
```
MailingProvider provider = new SMTPProvider(
  USERNAME, PASSWORD, HOST, PORT, SSL_OPTION,
  _logger: myApplicationLogger
  );
```


## Send a prepared MimeMessage
If you require to send a more complex message, create a MimeKit.MimeMessage object and pass it to the SendMail method.
```
provider.SendMail(mimeMessage);
```
