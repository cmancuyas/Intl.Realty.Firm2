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

            foreach (var address in mailRequest.ToEmail
                         .Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(e => e.Trim()))
            {
                emailToSend.To.Add(MailboxAddress.Parse(address));
            }

            if (!string.IsNullOrWhiteSpace(mailRequest.CcEmail))
            {
                foreach (var cc in mailRequest.CcEmail
                             .Split(',', StringSplitOptions.RemoveEmptyEntries)
                             .Select(e => e.Trim()))
                {
                    emailToSend.Cc.Add(MailboxAddress.Parse(cc));
                }
            }

            emailToSend.Subject = mailRequest.Subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = mailRequest.Body
            };

            try
            {
                using var emailClient = new SmtpClient();

                _logger.LogInformation("Preparing to send email...");
                _logger.LogInformation("SMTP Host: {Host}, Port: {Port}", host, port);
                _logger.LogInformation("To: {To}", mailRequest.ToEmail);

                if (!string.IsNullOrWhiteSpace(password) && password.Length >= 5)
                {
                    _logger.LogInformation("Using partial API key: {Key}", password.Substring(0, 5));
                }
                else
                {
                    _logger.LogWarning("Email password is missing or too short to log safely.");
                }

                await emailClient.ConnectAsync(host, Convert.ToInt32(port), MailKit.Security.SecureSocketOptions.StartTls);
                await emailClient.AuthenticateAsync("apikey", password);

                _logger.LogInformation("Sending email to {Recipient}", mailRequest.ToEmail);
                await emailClient.SendAsync(emailToSend);
                await emailClient.DisconnectAsync(true);

                _logger.LogInformation("✅ Email successfully sent to {Recipient}", mailRequest.ToEmail);
            }
            catch (MailKit.Net.Smtp.SmtpProtocolException smtpEx)
            {
                _logger.LogError(smtpEx, "❌ SMTP Protocol Error sending email to {Recipient}. Error: {ErrorMessage}", 
                    mailRequest.ToEmail, smtpEx.Message);
                
                // Provide specific guidance based on error message
                if (smtpEx.Message.Contains("Maximum credits exceeded"))
                {
                    _logger.LogError("💳 SendGrid account has exceeded monthly email limits. Please check your SendGrid dashboard or upgrade your plan.");
                }
                else if (smtpEx.Message.Contains("Authentication failed"))
                {
                    _logger.LogError("🔑 SendGrid API key authentication failed. Please verify your API key is valid and active.");
                }
                
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Unexpected error sending email to {Recipient}", mailRequest.ToEmail);
                throw;
            }
        }

    }
}
