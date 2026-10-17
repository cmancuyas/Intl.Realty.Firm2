using Intl.Realty.Firm.Models.Auxiliary;
using Intl.Realty.Firm.Service.IService;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Intl.Realty.Firm.Service
{
    public class EmailService : IEmailService
    {
        private const int MaxSendAttempts = 3;
        private readonly EmailSettings _mailSettings;
        private readonly IConfigurationService _configurationService;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IOptions<EmailSettings> mailSettings,
            IConfigurationService configurationService,
            ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _configurationService = configurationService;
            _logger = logger;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            if (mailRequest == null)
                throw new ArgumentNullException(nameof(mailRequest));

            if (string.IsNullOrWhiteSpace(mailRequest.ToEmail))
                throw new InvalidOperationException("Email recipient (ToEmail) is required.");

            var emailAddress = _configurationService.GetEmail()?.Trim();
            var password = _configurationService.GetPassword()?.Trim();
            var host = _configurationService.GetHost()?.Trim();
            var displayName = _configurationService.GetDisplayName()?.Trim();
            var portText = _configurationService.GetPort()?.Trim();

            if (string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(emailAddress) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(portText) ||
                !int.TryParse(portText, out var port))
            {
                throw new InvalidOperationException(
                    "Email settings are incomplete. Host, Port, Email, and Password must be configured.");
            }

            var emailToSend = new MimeMessage();
            emailToSend.From.Add(new MailboxAddress(
                string.IsNullOrWhiteSpace(displayName) ? emailAddress : displayName,
                emailAddress));

            foreach (var address in mailRequest.ToEmail
                         .Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(e => e.Trim())
                         .Where(e => !string.IsNullOrWhiteSpace(e)))
            {
                emailToSend.To.Add(MailboxAddress.Parse(address));
            }

            if (!emailToSend.To.Any())
                throw new InvalidOperationException("No valid recipient addresses were provided.");

            if (!string.IsNullOrWhiteSpace(mailRequest.ReplyToEmail))
            {
                emailToSend.ReplyTo.Add(MailboxAddress.Parse(mailRequest.ReplyToEmail.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(mailRequest.CcEmail))
            {
                foreach (var cc in mailRequest.CcEmail
                             .Split(',', StringSplitOptions.RemoveEmptyEntries)
                             .Select(e => e.Trim())
                             .Where(e => !string.IsNullOrWhiteSpace(e)))
                {
                    emailToSend.Cc.Add(MailboxAddress.Parse(cc));
                }
            }

            emailToSend.Subject = mailRequest.Subject ?? string.Empty;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = mailRequest.Body ?? string.Empty
            };

            Exception? lastException = null;

            for (var attempt = 1; attempt <= MaxSendAttempts; attempt++)
            {
                try
                {
                    using var emailClient = new SmtpClient();
                    emailClient.Timeout = 30000;

                    _logger.LogInformation(
                        "Preparing to send email (attempt {Attempt}/{MaxAttempts}). SMTP Host: {Host}, Port: {Port}, To: {To}",
                        attempt, MaxSendAttempts, host, port, mailRequest.ToEmail);

                    await emailClient.ConnectAsync(
                        host,
                        port,
                        MailKit.Security.SecureSocketOptions.StartTls);

                    await emailClient.AuthenticateAsync("apikey", password);
                    await emailClient.SendAsync(emailToSend);
                    await emailClient.DisconnectAsync(true);

                    _logger.LogInformation(
                        "Email successfully sent to {Recipient}",
                        mailRequest.ToEmail);

                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;

                    _logger.LogWarning(
                        ex,
                        "Email send attempt {Attempt}/{MaxAttempts} failed for {Recipient}",
                        attempt,
                        MaxSendAttempts,
                        mailRequest.ToEmail);

                    if (attempt < MaxSendAttempts)
                    {
                        await Task.Delay(
                            TimeSpan.FromSeconds(attempt * 2));
                    }
                }
            }

            // Only execute after ALL retries are exhausted

            _logger.LogError(
                lastException,
                "All email send attempts failed for {Recipient}",
                mailRequest.ToEmail);

            string userMessage = lastException switch
            {
                MailKit.Security.AuthenticationException =>
                    "Email authentication failed. Please verify the SMTP credentials.",

                MailKit.Net.Smtp.SmtpCommandException smtpEx =>
                    $"SMTP server rejected the request. Status: {smtpEx.StatusCode}.",

                System.Net.Sockets.SocketException =>
                    "Unable to connect to the SMTP server. Please verify the host and port.",

                TimeoutException =>
                    "The SMTP server did not respond in time.",

                _ =>
                    $"Email delivery failed. {lastException?.GetBaseException()?.Message}"
            };

            throw new InvalidOperationException(userMessage, lastException);
        }
    }
}
