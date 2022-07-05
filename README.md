Net 6 Library for accessing email

TODO:
Create Provider for SMTP, OAUTH
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
```
MailingProvider provider = new MicrosoftGraphProvider(
    TenantId, ClientId, SecretValue, UserId
  };
  
provider.SendMail(subject, body, mailTo, mailFrom);

// mailFrom and UserID must be from the same account.
```
