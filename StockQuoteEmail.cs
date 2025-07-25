using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace stock_quote_alert_pedro;

public class StockQuoteEmail {
    private readonly Config _config;
    private static readonly Regex _emailRegex = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}", RegexOptions.Compiled);

    public StockQuoteEmail(Config config) {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    private MailMessage? SetMessage(string body, string subject, string emailTo) {
        try {
            MailMessage email = new MailMessage();
            email.From = new MailAddress(_config.HostEmail!);

            if (!CheckEmail(emailTo))
                throw new ArgumentException("Invalid email address\n", nameof(emailTo));

            email.To.Add(emailTo);
            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = true;
            return email;
        } catch (Exception ex) {
            Console.WriteLine($"Error creating email message: {ex.Message}\n");
            return null;
        }
    }

    public void SendEmail(string emailTo, string subject, string body) {
        MailMessage? message = SetMessage(body, subject, emailTo);
        if (message is null) {
            Console.WriteLine("Unable to create the email message. Cancelling send.\n");
            return;
        }
        SendSmtpEmail(message);
    }

    private bool CheckEmail(string email) => _emailRegex.IsMatch(email);

    public SmtpClient SetSmtpClient() {
        SmtpClient smtpClient = new SmtpClient(_config.SmtpHost!);
        smtpClient.Port = _config.Port;
        smtpClient.EnableSsl = _config.UseSsl;
        smtpClient.Timeout = 50000;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(_config.HostEmail, _config.HostPassword);
        return smtpClient;
    }

    private void SendSmtpEmail(MailMessage message) {
        using SmtpClient smtpClient = SetSmtpClient();
        smtpClient.Send(message);
    }
}
