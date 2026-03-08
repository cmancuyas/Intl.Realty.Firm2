using Intl.Realty.Firm.Models.Auxiliary;

namespace Intl.Realty.Firm.Service.IService
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
