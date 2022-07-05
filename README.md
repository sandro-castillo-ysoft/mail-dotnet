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
```
MailingProvider provider = new SMTPProvider() {
      Host = host,
      Port = port,
      SslOption = sslOption,
      Username = username,
      Password = password,
  };
  
provider.SendMail(subject, body, mailTo, mailFrom);
```

#### SMTP Send


#### OAuth2 Send
