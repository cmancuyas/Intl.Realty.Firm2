using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.AccountVM;
using Intl.Realty.Firm.Model.ViewModel.ReferAnAgentVM;

namespace Intl.Realty.Firm.Helper
{
    public class EmailTemplate
    {
        public static string UserRegistrationTemplate(RegisterViewModel registerViewModel, string confirmationLink)
        {
            string template =
                    @$"<html>
                     <meta charset=""utf-8"">
                    <body>
                    <p>Good day <strong>{registerViewModel.FirstName},</strong></p>
                    <p><span style=""background-color:transparent;color:#000000;font-family:Arial,sans-serif;font-size:11pt;""><span style=""font-style:normal;font-variant:normal;font-weight:400;text-decoration:none;vertical-align:baseline;white-space:pre-wrap;"">We have received your user <strong>registration request</strong> for the INTL Realty Firm.&nbsp;</span></span></p>
                    <p><span style=""background-color:transparent;color:#000000;font-family:Arial,sans-serif;font-size:11pt;""><span style=""font-style:normal;font-variant:normal;font-weight:400;text-decoration:none;vertical-align:baseline;white-space:pre-wrap;"">Confirm your email by clicking on the link below.&nbsp;</span></span></p>
                    <p><strong>{confirmationLink}</strong>
                    <p>&nbsp;</p>
                    <p>
                        <meta charset=""utf-8""><span style=""background-color:transparent;color:#000000;font-family:Arial,sans-serif;font-size:11pt;""><span style=""font-style:normal;font-variant:normal;font-weight:400;text-decoration:none;vertical-align:baseline;white-space:pre-wrap;"">Thank you.&nbsp;</span></span>
                    </p>
                    </body>
                    </html>";
            return template;
        }

        public static string UserResetPasswordTemplate(ApplicationUser user, string resetPasswordLink)
        {
            var encodedResetLink = System.Web.HttpUtility.UrlEncode(resetPasswordLink);

            return $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1'>
        <title>Reset Your Password</title>
        <style>
            body, table, td {{
                font-family: Arial, sans-serif !important;
            }}
            .container {{
                max-width: 600px;
                margin: 0 auto;
                padding: 20px;
                background-color: #ffffff;
                border-radius: 8px;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                text-align: center;
            }}
            .header {{
                font-size: 24px;
                font-weight: bold;
                color: #333;
                padding-bottom: 20px;
            }}
            .text {{
                font-size: 16px;
                color: #555;
                margin-bottom: 20px;
            }}
            .button-container {{
                text-align: center;
                margin-top: 20px;
            }}
            .button {{
                background-color: #0d6efd;
                color: #ffffff !important;
                padding: 12px 24px;
                font-size: 16px;
                font-weight: bold;
                text-decoration: none;
                display: inline-block;
                border-radius: 5px;
            }}
            .button-table {{
                border-collapse: separate;
                border-spacing: 0;
                border-radius: 5px;
                background-color: #0d6efd;
                text-align: center;
            }}
            @media screen and (max-width: 600px) {{
                .container {{
                    width: 90%;
                    padding: 15px;
                }}
                .button {{
                    padding: 10px 20px;
                    font-size: 14px;
                }}
            }}
        </style>
    </head>
    <body style='background-color: #f7fafc; padding: 20px;'>
        <div class='container'>
            <img src='https://d15k2d11r6t6rl.cloudfront.net/pub/r388/l239mmxz/pwn/3tk/6kt/315.png' width='150' alt='Reset Password' />
            <div class='header'>Reset Your Password</div>
            <p class='text'>Dear {user.FirstName},</p>
            <p class='text'>
                We received a request to reset your password. Click the button below to reset it.
                If you did not request this, you can safely ignore this email.
            </p>

            <!-- Ensuring button is clickable -->
            <div class='button-container'>
                <table role='presentation' class='button-table' width='100%' cellspacing='0' cellpadding='0'>
                    <tr>
                        <td align='center'>
                            <a href='{resetPasswordLink}' class='button' target='_blank'>Reset Password</a>
                        </td>
                    </tr>
                </table>
            </div>

            <p class='text'>If the button does not work, copy and paste the following link into your browser:</p>
            <p class='text' style='word-wrap: break-word;'><a href='{resetPasswordLink}' target='_blank'>{resetPasswordLink}</a></p>
            
            <hr>
            <p class='text'>If you have any questions, contact support at support@internationalrealtyfirm.com</p>
        </div>
    </body>
    </html>";
        }


        public static string SendNotificationWhenDealIsSubmitted(ApplicationUser user, string userRole, string transactionTypeName, List<InternalEmail> emails, string dealId, string dealLink)
        {
            var roleString = string.Empty;
            foreach(InternalEmail email in emails)
            {
                roleString += email.RoleName + ",";
            }
            roleString = roleString.Remove(roleString.Length - 1, 1);

            string template =
                    @$"<html>
                     <meta charset=""utf-8"">
                    <body>
                    <p>Good day <strong>{roleString},</strong></p>
                    <p><span style=""background-color:transparent;color:#000000;font-family:Arial,sans-serif;font-size:11pt;""><span style=""font-style:normal;font-variant:normal;font-weight:400;text-decoration:none;vertical-align:baseline;white-space:pre-wrap;"">{userRole} {user.FirstName} {user.LastName} created Deal ID: <strong>{transactionTypeName} - {dealId}</strong>.&nbsp;</span></span></p>
                    <p><span style=""background-color:transparent;color:#000000;font-family:Arial,sans-serif;font-size:11pt;""><span style=""font-style:normal;font-variant:normal;font-weight:400;text-decoration:none;vertical-align:baseline;white-space:pre-wrap;"">Click this link to login and review.&nbsp;</span></span></p>
                    <p><strong>{dealLink}</strong>
                    <p>&nbsp;</p>
                    <p>
                        <meta charset=""utf-8""><span style=""background-color:transparent;color:#000000;font-family:Arial,sans-serif;font-size:11pt;""><span style=""font-style:normal;font-variant:normal;font-weight:400;text-decoration:none;vertical-align:baseline;white-space:pre-wrap;"">Thank you.&nbsp;</span></span>
                    </p>
                    </body>
                    </html>";
            return template;
        }

        public static string SendRequestToEditTheDeal(ApplicationUser user, 
                                                     string userRole,
                                                     string transactionTypeName,
                                                     List<InternalEmail> emails, 
                                                     string dealId, 
                                                     string dealLink)
        {
            var roleString = string.Empty;
            foreach (InternalEmail email in emails)
            {
                roleString += email.RoleName + ",";
            }
            roleString = roleString.Remove(roleString.Length - 1, 1);

            string template =
                    @$"<html>
                     <meta charset=""utf-8"">
                    <body>
                    <p>Good day <strong>{roleString},</strong></p>
                    <p><span style=""background-color:transparent;color:#000000;font-family:Arial,sans-serif;font-size:11pt;""><span style=""font-style:normal;font-variant:normal;font-weight:400;text-decoration:none;vertical-align:baseline;white-space:pre-wrap;"">{userRole} {user.FirstName} {user.LastName} is requesting to edit Deal ID: <strong>{transactionTypeName} - {dealId}</strong>.&nbsp;</span></span></p>
                    <p><span style=""background-color:transparent;color:#000000;font-family:Arial,sans-serif;font-size:11pt;""><span style=""font-style:normal;font-variant:normal;font-weight:400;text-decoration:none;vertical-align:baseline;white-space:pre-wrap;"">Click this link to login and review.&nbsp;</span></span></p>
                    <p><strong>{dealLink}</strong>
                    <p>&nbsp;</p>
                    <p>
                        <meta charset=""utf-8""><span style=""background-color:transparent;color:#000000;font-family:Arial,sans-serif;font-size:11pt;""><span style=""font-style:normal;font-variant:normal;font-weight:400;text-decoration:none;vertical-align:baseline;white-space:pre-wrap;"">Thank you.&nbsp;</span></span>
                    </p>
                    </body>
                    </html>";
            return template;
        }
        public static string SendRemailToReferAnAgent(ContactEmail contactEmail,
                                                     ReferAnAgentViewModel referAnAgentViewModel,
                                                     string agentLink)
        {
            string template = 
            @$"<html>
                     <meta charset=""utf-8"">
                    <body>
                    <p>Good day <strong>{contactEmail.FirstName},</strong></p>
                    <p><span style=""background-color:transparent;color:#000000;font-family:Arial,sans-serif;font-size:11pt;""><span style=""font-style:normal;font-variant:normal;font-weight:400;text-decoration:none;vertical-align:baseline;white-space:pre-wrap;"">This agent being referred:  <br> Name: {referAnAgentViewModel.Name} <br> Phone Number: {referAnAgentViewModel.TelephoneNumber} .&nbsp;</span></span></p>
                    <p><span style=""background-color:transparent;color:#000000;font-family:Arial,sans-serif;font-size:11pt;""><span style=""font-style:normal;font-variant:normal;font-weight:400;text-decoration:none;vertical-align:baseline;white-space:pre-wrap;"">Click this link to login and review.&nbsp;</span></span></p>
                    <p><strong>{agentLink}</strong>
                    <p>&nbsp;</p>
                    <p>
                        <meta charset=""utf-8""><span style=""background-color:transparent;color:#000000;font-family:Arial,sans-serif;font-size:11pt;""><span style=""font-style:normal;font-variant:normal;font-weight:400;text-decoration:none;vertical-align:baseline;white-space:pre-wrap;"">Thank you.&nbsp;</span></span>
                    </p>
                    </body>
                    </html>";
            return template;
        }
    }
}
