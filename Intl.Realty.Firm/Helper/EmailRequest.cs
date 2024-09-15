using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.AccountVM;
using Intl.Realty.Firm.Models.Auxiliary;

namespace Intl.Realty.Firm.Helper
{
    public class EmailRequest
    {
        public static MailRequest CreateRegistrationVerification(RegisterViewModel registerViewModel, string confirmationLink)
        {
            var template = EmailTemplate.UserRegistrationTemplate(registerViewModel, confirmationLink);
            var mailRequest = new MailRequest
            {
                ToEmail = registerViewModel.Email,
                Subject = "do not reply",
                Body = template
            };

            return mailRequest;
        }

        public static MailRequest CreateResetPasswordEmailRequest(ApplicationUser user, string resetLink)
        {
            var template = EmailTemplate.UserResetPasswordTemplate(user, resetLink);
            var mailRequest = new MailRequest
            {
                ToEmail = user.Email,
                Subject = "do not reply",
                Body = template
            };
            return mailRequest;
        }
    }
}
