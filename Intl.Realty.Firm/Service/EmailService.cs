using Intl.Realty.Firm.Models.Auxiliary;
using Intl.Realty.Firm.Service.IService;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Intl.Realty.Firm.Service
{
    public class EmailService : IEmailService
    {
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
            var emailAddress = _configurationService.GetEmail();
            var password = _configurationService.GetPassword();
            var host = _configurationService.GetHost();
            var displayName = _configurationService.GetDisplayName();
            var port = _configurationService.GetPort();

            var emailToSend = new MimeMessage();
            emailToSend.From.Add(new MailboxAddress(displayName, emailAddress));
            emailToSend.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            emailToSend.Subject = mailRequest.Subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = mailRequest.Body
            };

            try
            {
                using var emailClient = new SmtpClient();
                _logger.LogInformation("Connecting to SMTP server {Host}:{Port}...", host, port);

                await emailClient.ConnectAsync(host, Convert.ToInt32(port), MailKit.Security.SecureSocketOptions.StartTls);
                await emailClient.AuthenticateAsync(emailAddress, password);

                _logger.LogInformation("Sending email to {Recipient}", mailRequest.ToEmail);
                await emailClient.SendAsync(emailToSend);
                await emailClient.DisconnectAsync(true);

                _logger.LogInformation("Email successfully sent to {Recipient}", mailRequest.ToEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {Recipient}", mailRequest.ToEmail);
                throw; // Let the caller handle it or bubble it up
            }
        }
    }
}
