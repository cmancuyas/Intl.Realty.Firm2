using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.AccountVM;
using Intl.Realty.Firm.Model.ViewModel.ReferAnAgentVM;
using System.Net;

namespace Intl.Realty.Firm.Helper
{
    public class EmailTemplate
    {
        public static string UserEmailConfirmationTemplate(ApplicationUser user, string confirmationLink)
        {
            return BuildConfirmationEmailHtml(
                user.FirstName ?? "User",
                confirmationLink,
                "Confirm Your Email",
                "Thank you for registering with International Realty Firm. Please confirm your email by clicking the button below.");
        }

        public static string UserRegistrationTemplate(RegisterViewModel registerViewModel, string confirmationLink)
        {
            return BuildConfirmationEmailHtml(
                registerViewModel.FirstName ?? "User",
                confirmationLink,
                "Confirm Your Registration",
                "We have received your registration request for International Realty Firm. Please confirm your email by clicking the button below.");
        }

        private static string BuildConfirmationEmailHtml(
            string firstName,
            string confirmationLink,
            string heading,
            string introText)
        {
            var safeName = WebUtility.HtmlEncode(firstName);
            var safeLink = WebUtility.HtmlEncode(confirmationLink);

            return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1'>
    <title>{WebUtility.HtmlEncode(heading)}</title>
</head>
<body style='background-color:#f7fafc;padding:20px;font-family:Arial,sans-serif;'>
    <div style='max-width:600px;margin:0 auto;padding:24px;background:#ffffff;border-radius:8px;'>
        <h2 style='color:#333;margin-top:0;'>{WebUtility.HtmlEncode(heading)}</h2>
        <p style='color:#555;font-size:16px;'>Dear {safeName},</p>
        <p style='color:#555;font-size:16px;'>{WebUtility.HtmlEncode(introText)}</p>
        <p style='text-align:center;margin:28px 0;'>
            <a href='{confirmationLink}'
               style='background-color:#0d6efd;color:#ffffff;padding:12px 24px;font-size:16px;font-weight:bold;text-decoration:none;border-radius:5px;display:inline-block;'
               target='_blank' rel='noopener'>
                Confirm Email
            </a>
        </p>
        <p style='color:#555;font-size:14px;'>If the button does not work, copy and paste this link into your browser:</p>
        <p style='color:#555;font-size:14px;word-break:break-all;'><a href='{confirmationLink}' target='_blank' rel='noopener'>{safeLink}</a></p>
        <hr style='border:none;border-top:1px solid #eee;margin:24px 0;' />
        <p style='color:#777;font-size:13px;'>If you did not sign up, you can safely ignore this email.</p>
        <p style='color:#777;font-size:13px;'>Questions? Contact support@internationalrealtyfirm.com</p>
    </div>
</body>
</html>";
        }

        public static string UserResetPasswordTemplate(ApplicationUser user, string resetPasswordLink)
        {
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
            <p class='text'>Dear {WebUtility.HtmlEncode(user.FirstName)},</p>
            <p class='text'>
                We received a request to reset your password. Click the button below to reset it.
                If you did not request this, you can safely ignore this email.
            </p>
            <p class='text'>This link expires in 24 hours.</p>

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


        public static string SendNotificationWhenDealIsSubmitted(
            ApplicationUser user,
            string recipientName,      // The name or role for greeting
            string transactionTypeName,
            string dealId,
            string dealLink,
            bool isCreator = false     // <--- NEW
        )
        {
            string greeting;
            string body;

            if (isCreator)
            {
                greeting = $"Good day <strong>{user.FirstName} {user.LastName},</strong>";
                body = $@"You have created Deal ID: <strong>{transactionTypeName} - {dealId}</strong>.<br/>
                  Click this link to login to view.<br/>
                  <strong>{dealLink}</strong>";
            }
            else
            {
                greeting = $"Good day <strong>{recipientName},</strong>";
                body = $@"{user.FirstName} {user.LastName} created Deal ID: <strong>{transactionTypeName} - {dealId}</strong>.<br/>
                  Click this link to login and review.<br/>
                  <strong>{dealLink}</strong>";
            }

            string template = $@"<html>
                                <meta charset=""utf-8"">
                                <body>
                                    <p>{greeting}</p>
                                    <p>{body}</p>
                                    <p>Thank you.</p>
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
            @$"
                <html>
                <head>
                    <meta charset=""utf-8"">
                </head>
                <body style=""font-family: Arial, sans-serif; font-size: 11pt; color: #000000;"">
                    <p>Good day <strong>{contactEmail.FirstName}</strong>,</p>

                    <p>The following agent has been referred:</p>
                    <ul>
                        <li><strong>Referrer Name:</strong> {referAnAgentViewModel.Name}</li>
                        <li><strong>Referrer Phone:</strong> {referAnAgentViewModel.TelephoneNumber}</li>
                        <li><strong>Referred Agent Name:</strong> {referAnAgentViewModel.ReferredAgentName ?? "<em>Not provided</em>"}</li>
                        <li><strong>Notes:</strong> {(!string.IsNullOrWhiteSpace(referAnAgentViewModel.Notes) ? referAnAgentViewModel.Notes : "<em>None</em>")}</li>
                    </ul>

                    <p>Click the link below to log in and review:</p>
                    <p>
                        <a href=""{agentLink}"" style=""color: #1a73e8;"">{agentLink}</a>
                    </p>

                    <p>Thank you.</p>
                </body>
                </html>";

            return template;
        }

        public static string SendEmailLoginIssue(SupportViewModel supportViewModel)
        {
            string template = $@"
        <html>
        <head>
            <meta charset=""utf-8"">
        </head>
        <body style=""font-family: Arial, sans-serif; font-size: 11pt; color: #000000;"">
            <p>Hello Support Team,</p>

            <p>A new login issue has been submitted via the support form. The details are as follows:</p>

            <ul>
                <li><strong>Email Address:</strong> {WebUtility.HtmlEncode(supportViewModel.Email)}</li>
                <li><strong>Description:</strong> {WebUtility.HtmlEncode(supportViewModel.Description).Replace("\n", "<br />")}</li>
            </ul>

            <p>Please review and follow up with the user as appropriate.</p>

            <p>Thank you,<br />IRF Support Portal</p>
        </body>
        </html>";

            return template;
        }

    }
}
