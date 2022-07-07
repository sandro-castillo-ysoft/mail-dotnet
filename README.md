Net 6 Library for accessing email

TODO:
Create Provider for OAUTH GoogleApis
Scope of supported OAUTH:
  - Microsoft Graph
  - Google


# Usage
[Send Mail](#sending) | [Reading Mail](#receiving)

## Sending Mails
### Send Mail
Send an email using a MimeMessage class or passing string arguments as parameters

#### SMTP Send
```
MailingProvider provider = new SMTPProvider(username, password, host, port, sslOption);
  
provider.SendMail(subject, body, mailTo, mailFrom);
```

#### OAuth2 Send
Microsoft Graph (aka Office365)
App Registration must be given Administrator Consent and acces to https://graph.microsoft.com/Mail.Send .
```
MailingProvider provider = new MicrosoftGraphProvider(
    TenantId, ClientId, SecretValue, UserId
  );
  
provider.SendMail(subject, body, mailTo, mailFrom);

// mailFrom and UserID must be from the same account.
```

Google Apis
Service Account must be given domain-wide delegation to https://www.googleapis.com/auth/gmail.send .
```
MailingProvider provider = new GoogleApisProvider(
    ServiceAccountEmail, ImpersonatedAccountEmail, CredentialPrivateKey
  );
```
