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

        public EmailService(IOptions<EmailSettings> mailSettings, IConfigurationService configurationService)
        {
            _mailSettings = mailSettings.Value;
            _configurationService = configurationService;
        }

        public Task SendEmailAsync(MailRequest mailRequest)
        {
            var emailAddress = _configurationService.GetEmail();
            var password = _configurationService.GetPassword();
            var host = _configurationService.GetHost();
            var displayName = _configurationService.GetDisplayName();
            var port = _configurationService.GetPort();

            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse(emailAddress));
            emailToSend.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            emailToSend.Subject = mailRequest.Subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = mailRequest.Body
            };

            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect(host, Convert.ToInt32(port), MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate(emailAddress, password);
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);
            }

            return Task.CompletedTask;

        }

        //private SmtpClient CreateSmtpCredentials => new SmtpClient(_mailSettings.Host, _mailSettings.Port)
        //{
        //    EnableSsl = true,
        //    DeliveryMethod = SmtpDeliveryMethod.Network,
        //    Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password)
        //};
    }
}
