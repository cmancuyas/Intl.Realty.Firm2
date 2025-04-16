using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.AccountVM;
using Intl.Realty.Firm.Model.ViewModel.ReferAnAgentVM;
using Intl.Realty.Firm.Models.Auxiliary;

namespace Intl.Realty.Firm.Helper
{
    public class EmailRequest
    {
        public static MailRequest CreateEmailConfirmationRequest(ApplicationUser user, string confirmationLink)
        {
            var template = EmailTemplate.UserEmailConfirmationTemplate(user, confirmationLink);

            var mailRequest = new MailRequest
            {
                ToEmail = user.Email!,
                Subject = "do not reply - Confirm Your Email",
                Body = template
            };

            return mailRequest;
        }

        public static MailRequest CreateRegistrationVerification(RegisterViewModel registerViewModel, string confirmationLink)
        {
            var template = EmailTemplate.UserRegistrationTemplate(registerViewModel, confirmationLink);
            var mailRequest = new MailRequest
            {
                ToEmail = registerViewModel.Email,
                Subject = "do not reply - User Registration",
                Body = template
            };

            return mailRequest;
        }

        public static MailRequest CreateResetPasswordEmailRequest(ApplicationUser user, string resetLink)
        {
            var template = EmailTemplate.UserResetPasswordTemplate(user, resetLink);
            var mailRequest = new MailRequest
            {
                ToEmail = user.Email!,
                Subject = "do not reply - Reset Password",
                Body = template
            };
            return mailRequest;
        }

        public static MailRequest SendNotificationWhenDealIsSubmitted(ApplicationUser user, 
                                                                     IList<string> userRoles, string transactionTypeName, 
                                                                     List<InternalEmail> emails, string dealId, string dealLink)
        {
            var emailString = string.Empty;
            foreach (var email in emails)
            {
                emailString += email.Email + ",";
            }

            var userRole = userRoles.FirstOrDefault();

            emailString = emailString.Remove(emailString.Length - 1,1);

            var template = EmailTemplate.SendNotificationWhenDealIsSubmitted(user, userRole!, transactionTypeName, emails, dealId, dealLink);

            var mailRequest = new MailRequest
            {
                ToEmail = emailString,
                Subject = "do not reply " + dealId,
                Body = template
            };
            return mailRequest;
        }

        public static MailRequest SendRequestToEditTheDeal(ApplicationUser user, 
                                                           IList<string> userRoles, 
                                                           string transactionTypeName,
                                                           List<InternalEmail> emails, 
                                                           string dealId, 
                                                           string dealLink)
        {
            var emailString = string.Empty;
            foreach (var email in emails)
            {
                emailString += email.Email + ",";
            }

            emailString = emailString.Remove(emailString.Length - 1, 1);

            var userRole = userRoles.FirstOrDefault();    

            var template = EmailTemplate.SendRequestToEditTheDeal(user, userRole!, transactionTypeName, emails, dealId, dealLink);

            var mailRequest = new MailRequest
            {
                ToEmail = emailString,
                Subject = "do not reply " + dealId,
                Body = template
            };
            return mailRequest;
        }
        public static MailRequest SendRemailToReferAnAgent(ContactEmail contactEmail,
                                                   ReferAnAgentViewModel referAnAgentViewModel,
                                                   string agentLink)
        {


            var template = EmailTemplate.SendRemailToReferAnAgent(contactEmail, referAnAgentViewModel, agentLink);

            var mailRequest = new MailRequest
            {
                ToEmail = contactEmail.Email,
                Subject = "do not reply - Refer an agent" ,
                Body = template
            };
            return mailRequest;
        }
    }
}
