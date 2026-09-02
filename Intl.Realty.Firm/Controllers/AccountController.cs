using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Helper;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel;
using Intl.Realty.Firm.Model.ViewModel.AccountVM;
using Intl.Realty.Firm.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Intl.Realty.Firm.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly ILogger<AccountController> _logger;
        private readonly IGoogleCaptchaService _googleCaptchaService;
        private readonly IConfigurationService _configurationService;

        public AccountController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                IEmailService emailService,
                                ILogger<AccountController> logger,
                                IGoogleCaptchaService googleCaptchaService,
                                IConfigurationService configurationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _logger = logger;
            _googleCaptchaService = googleCaptchaService;
            _configurationService = configurationService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DetailedInstructions()
        {
            return View();
        }
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string? returnUrl)
        {
            LoginViewModel viewModel = new LoginViewModel()
            {
                ReturnUrl = returnUrl!,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
            };
            return View(viewModel);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel, string? returnUrl, string? token)
        {
            viewModel.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!ModelState.IsValid)
                return View(viewModel);

            if (!await _googleCaptchaService.VerifyToken(token, expectedAction: "login", minScore: 0.5))
            {
                ModelState.AddModelError(string.Empty, "Captcha verification failed. Please try again.");
                return View(viewModel);
            }

            // Load user only for EmailStatusId check + role redirect later
            var user = await _userManager.FindByEmailAsync(viewModel.Email);

            // If user exists, enforce EmailStatus before attempting sign-in
            if (user != null && user.EmailStatusId != 1)
            {
                ModelState.AddModelError(string.Empty, "Your account email status does not permit login. Please contact support.");
                return View(viewModel);
            }

            // Single sign-in call (handles password validation + lockout counters)
            var result = await _signInManager.PasswordSignInAsync(
                viewModel.Email,
                viewModel.Password,
                viewModel.RememberMe,
                lockoutOnFailure: true);

            if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                return View(viewModel);
            }

            if (result.IsLockedOut)
                return RedirectToAction("Lockout");

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View(viewModel);
            }

            // Refresh user reference if it was null before (should not be null after successful sign-in)
            user ??= await _userManager.FindByEmailAsync(viewModel.Email);

            // Secure returnUrl handling
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            _logger.LogWarning("Suspicious return URL detected: {returnUrl}", returnUrl);

            // Redirect users based on role
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Any())
                    return RedirectToAction("Dashboard", "Home");
            }

            return RedirectToAction("Index", "Training");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoginIssueSupport()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitLoginIssue(SupportViewModel model, string? token)
        {
            if (model == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid support request.");
                return View("LoginIssueSupport", new SupportViewModel());
            }

            model.Email = model.Email?.Trim() ?? string.Empty;
            model.Description = model.Description?.Trim() ?? string.Empty;

            _logger.LogInformation("SubmitLoginIssue triggered. Email: {Email}", model.Email);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "SubmitLoginIssue ModelState invalid. Errors: {Errors}",
                    string.Join(" | ",
                        ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                );

                return View("LoginIssueSupport", model);
            }

            // Support users already struggling to log in; use a slightly lower threshold
            // and always require a fresh token from the client.
            if (!await _googleCaptchaService.VerifyToken(token, expectedAction: "login_issue", minScore: 0.3))
            {
                _logger.LogWarning("reCAPTCHA failed for LoginIssueSupport. Email: {Email}", model.Email);
                ModelState.AddModelError(string.Empty, "Captcha verification failed. Please refresh the page and try again.");
                return View("LoginIssueSupport", model);
            }

            try
            {
                var recipients = _configurationService.GetSupportEmail() ?? Array.Empty<string>();
                var recipientsCC = _configurationService.GetSupportEmailCC() ?? Array.Empty<string>();

                if (!recipients.Any())
                {
                    _logger.LogWarning("No recipients configured in GetSupportEmail()");
                    ModelState.AddModelError(string.Empty, "Support email is not configured. Please email support@irfconnect.com directly.");
                    return View("LoginIssueSupport", model);
                }

                var mailRequest = EmailRequest.SendEmailLoginIssue(
                    model,
                    string.Join(',', recipients),
                    string.Join(',', recipientsCC)
                );

                await _emailService.SendEmailAsync(mailRequest);

                _logger.LogInformation("Login issue email sent for {Email}", model.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending login issue email for {Email}", model.Email);
                ModelState.AddModelError(string.Empty,
                    "There was a problem sending your request. Please try again later, or email support@irfconnect.com directly.");
                return View("LoginIssueSupport", model);
            }

            TempData["SuccessMessage"] =
                "Your login issue has been submitted successfully. Our support team will contact you shortly.";

            return RedirectToAction("Login", "Account");
        }


        // ✅ Forgot Password - Request Reset Email
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var email = viewModel.Email?.Trim() ?? string.Empty;
            viewModel.Email = email;

            var user = await _userManager.FindByEmailAsync(email);

            // If user doesn't exist, redirect silently to avoid user enumeration
            if (user == null)
            {
                _logger.LogWarning("Password reset attempted for non-existent email: {Email}", email);
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            // If email is not confirmed, show custom view or error
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                _logger.LogWarning("Password reset attempted for unconfirmed email: {Email}", email);

                return View("EmailNotConfirmed", new ResendConfirmationViewModel
                {
                    Email = email
                });
            }

            // Generate reset token and Base64Url-encode it (Identity UI pattern).
            // Prevents +/= corruption by email clients and avoids double UrlEncode bugs.
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = EncodeTokenForEmail(token);

            var passwordResetLink = Url.Action(
                "ResetPassword",
                "Account",
                new { email, token = encodedToken },
                protocol: GetPublicRequestScheme());

            if (string.IsNullOrWhiteSpace(passwordResetLink))
            {
                _logger.LogError("Failed to generate password reset link for {Email}", email);
                ModelState.AddModelError(string.Empty, "Unable to generate a password reset link. Please try again later.");
                return View(viewModel);
            }

            try
            {
                var mailRequest = EmailRequest.CreateResetPasswordEmailRequest(user, passwordResetLink);
                await _emailService.SendEmailAsync(mailRequest);
                _logger.LogInformation("Password reset email sent to user: {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send password reset email to {Email}", email);
                ModelState.AddModelError(string.Empty,
                    "We could not send the password reset email right now. Please try again in a few minutes, or contact support.");
                return View(viewModel);
            }

            return RedirectToAction("ForgotPasswordConfirmation");
        }


        // ✅ Forgot Password Confirmation Page
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendEmailConfirmation(ResendConfirmationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("EmailNotConfirmed", model);
            }

            model.Email = model.Email?.Trim() ?? string.Empty;

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || await _userManager.IsEmailConfirmedAsync(user))
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = EncodeTokenForEmail(token);

            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = user.Id, token = encodedToken },
                protocol: GetPublicRequestScheme());

            try
            {
                var mailRequest = EmailRequest.CreateEmailConfirmationRequest(user, callbackUrl!);
                await _emailService.SendEmailAsync(mailRequest);
                _logger.LogInformation("Confirmation email resent to: {Email}", user.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to resend confirmation email to {Email}", model.Email);
                ModelState.AddModelError(string.Empty,
                    "We could not send the confirmation email right now. Please try again later.");
                return View("EmailNotConfirmed", model);
            }

            return RedirectToAction("ResendConfirmEmailSent");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResendConfirmEmailSent()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmEmailSent()
        {
            return View();
        }
        // ✅ Reset Password - Show Reset Form
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid password reset request.");
            }

            var model = new ResetPasswordViewModel
            {
                Email = email.Trim(),
                Token = token
            };

            return View(model);
        }

        // ✅ Reset Password - Handle Submission
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Email = model.Email?.Trim() ?? string.Empty;

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid password reset request.";
                return RedirectToAction("ForgotPassword");
            }

            var decodedToken = DecodeTokenFromEmail(model.Token);
            var resetResult = await _userManager.ResetPasswordAsync(user, decodedToken, model.Password);

            // Backward compatibility: older emails used UrlEncode instead of Base64Url.
            if (!resetResult.Succeeded)
            {
                var legacyToken = WebUtility.UrlDecode(model.Token);
                if (!string.Equals(legacyToken, decodedToken, StringComparison.Ordinal))
                {
                    resetResult = await _userManager.ResetPasswordAsync(user, legacyToken, model.Password);
                }
            }

            if (!resetResult.Succeeded)
            {
                _logger.LogWarning("Password reset failed for user: {Email}. Errors: {Errors}",
                    model.Email, string.Join(", ", resetResult.Errors.Select(e => e.Description)));

                foreach (var error in resetResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                ModelState.AddModelError(string.Empty,
                    "If this reset link has expired, request a new one from the Forgot Password page.");
                return View(model);
            }

            // Invalidate any other outstanding reset tokens after success.
            await _userManager.UpdateSecurityStampAsync(user);

            TempData["SuccessMessage"] = "Your password has been reset successfully. Please log in.";
            return RedirectToAction("Login");
        }



        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult UserAgreement()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            var teamLeaders = await _userManager.GetUsersInRoleAsync("TeamLeader");

            var model = new RegisterViewModel
            {
                TeamLeaders = teamLeaders.Select(tl => new SelectListItem
                {
                    Value = tl.Id.ToString(),
                    Text = $"{tl.FirstName} {tl.LastName}"
                }).ToList()
            };

            return View(model);
        }


        //_logger.LogInformation("🔍 Received reCAPTCHA Token: {Token}", viewModel.Token);

        //var captchaResult = await _googleCaptchaService.VerifyToken(viewModel.Token);
        //if (!captchaResult)
        //{
        //    _logger.LogError("❌ reCAPTCHA verification failed for user: {Email}", viewModel.Email);
        //    ModelState.AddModelError("", "Captcha verification failed. Please try again.");
        //    return View(viewModel);
        //}

        //_logger.LogInformation("✅ reCAPTCHA verification successful for user: {Email}", viewModel.Email);

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!viewModel.UserAgreement)
            {
                ModelState.AddModelError(string.Empty, "You must agree to the terms and conditions to register.");
            }

            if (!ModelState.IsValid)
            {
                await PopulateTeamLeadersAsync(viewModel);
                return View(viewModel);
            }

            if (!await _googleCaptchaService.VerifyToken(viewModel.Token, expectedAction: "register", minScore: 0.5))
            {
                _logger.LogWarning("reCAPTCHA failed for register: {Email}", viewModel.Email);
                ModelState.AddModelError(string.Empty, "Captcha verification failed. Please try again.");
                await PopulateTeamLeadersAsync(viewModel);
                return View(viewModel);
            }

            // ---- Normalize & clear stale email from ModelState to avoid “sticky” errors ----
            var trimmedEmail = viewModel.Email?.Trim();
            ModelState.Remove(nameof(viewModel.Email));
            viewModel.Email = trimmedEmail;

            // ---- Early duplicate check (case-insensitive by Identity) ----
            var existingUser = await _userManager.FindByEmailAsync(viewModel.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(viewModel.Email), "This email is already registered.");
                await PopulateTeamLeadersAsync(viewModel);
                return View(viewModel);
            }

            if (viewModel.DepartmentId == 0)
            {
                _logger.LogWarning("Register: Defaulting DepartmentId to 1 for user {Email}", viewModel.Email);
            }

            // Determine TeamLeader
            Guid? teamLeaderId = viewModel.TeamLeaderId;
            if (User.Identity?.IsAuthenticated == true && User.IsInRole("TeamLeader"))
            {
                if (Guid.TryParse(_userManager.GetUserId(User), out var currentTlId))
                {
                    teamLeaderId = currentTlId;
                }
            }

            var applicationUser = new ApplicationUser
            {
                UserName = viewModel.Email,
                Email = viewModel.Email,
                PhoneNumber = viewModel.Phone?.Trim(),
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                DepartmentId = viewModel.DepartmentId == 0 ? 1 : viewModel.DepartmentId,
                EmploymentStatusId = 1,
                EmailStatusId = 1,   // Active
                ProfileImageId = viewModel.ProfileImageId,
                UserAgreement = viewModel.UserAgreement,
                RegistrationDate = DateTime.UtcNow,
                TeamLeaderId = teamLeaderId,
                EmailConfirmed = true // keep your current behavior
            };

            var createResult = await _userManager.CreateAsync(applicationUser, viewModel.Password);
            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    // Route email-related errors to the Email field for better UX
                    if ((error.Code?.IndexOf("Duplicate", StringComparison.OrdinalIgnoreCase) ?? -1) >= 0 ||
                        (error.Description?.IndexOf("email", StringComparison.OrdinalIgnoreCase) ?? -1) >= 0)
                    {
                        ModelState.AddModelError(nameof(viewModel.Email), error.Description);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                await PopulateTeamLeadersAsync(viewModel);
                return View(viewModel);
            }

            // -------- Send confirmation email (retry handled inside EmailService) --------
            var confirmationEmailSent = false;
            try
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                var encodedToken = EncodeTokenForEmail(token);
                var confirmationLink = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = applicationUser.Id, token = encodedToken },
                    protocol: GetPublicRequestScheme());

                if (string.IsNullOrWhiteSpace(confirmationLink))
                {
                    throw new InvalidOperationException("Unable to generate email confirmation link.");
                }

                var mailRequest = EmailRequest.CreateRegistrationVerification(viewModel, confirmationLink);
                await _emailService.SendEmailAsync(mailRequest);
                confirmationEmailSent = true;

                _logger.LogInformation("Registration: confirmation email sent to {Email}.", applicationUser.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration: failed to send confirmation email to {Email}.", applicationUser.Email);
            }

            // -------- Always attempt to add Agent role (safety) --------
            try
            {
                var addRoleResult = await _userManager.AddToRoleAsync(applicationUser, "Agent");
                if (!addRoleResult.Succeeded)
                {
                    _logger.LogError("Registration: failed to add Agent role to {Email}. Errors: {Errors}",
                        applicationUser.Email,
                        string.Join("; ", addRoleResult.Errors.Select(e => $"{e.Code}:{e.Description}")));
                }
                else
                {
                    _logger.LogInformation("Registration: Agent role added to {Email}.", applicationUser.Email);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration: exception while adding Agent role to {Email}.", applicationUser.Email);
                // Proceed regardless
            }

            // If an admin is creating users from an existing session
            if (_signInManager.IsSignedIn(User) && (User.IsInRole("Admin") || User.IsInRole("SuperAdmin")))
            {
                return RedirectToAction("AdminDashboard", "Home");
            }

            if (viewModel.AutoSignInAfterRegister)
            {
                await _signInManager.SignInAsync(applicationUser, isPersistent: false);
            }

            TempData["RegisteredEmail"] = applicationUser.Email;
            TempData["ConfirmationEmailSent"] = confirmationEmailSent;
            if (!confirmationEmailSent)
            {
                TempData["EmailWarning"] =
                    "Your account was created, but we could not send the confirmation email. Please use Resend below, or contact support.";
            }

            return RedirectToAction(nameof(RegistrationSuccessful));
        }

        // Helper to repopulate team leaders for the dropdown
        private async Task PopulateTeamLeadersAsync(RegisterViewModel vm)
        {
            vm.TeamLeaders = (await _userManager.GetUsersInRoleAsync("TeamLeader"))
                .Select(tl => new SelectListItem
                {
                    Value = tl.Id.ToString(),
                    Text = $"{tl.FirstName} {tl.LastName}"
                })
                .ToList();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegistrationSuccessful()
        {
            return View();
        }

        /// <summary>
        /// Resends the registration confirmation email. Works even when EmailConfirmed is already true,
        /// so users who never received the original message can still get a usable link.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendRegistrationEmail(string email)
        {
            email = email?.Trim() ?? string.Empty;
            TempData["RegisteredEmail"] = email;

            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["ConfirmationEmailSent"] = false;
                TempData["EmailWarning"] = "Email address is required to resend the confirmation message.";
                return RedirectToAction(nameof(RegistrationSuccessful));
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Avoid account enumeration: show a generic success-style message.
                TempData["ConfirmationEmailSent"] = true;
                TempData["EmailWarning"] = null;
                TempData["SuccessMessage"] = "If an account exists for that email, a confirmation message has been sent.";
                return RedirectToAction(nameof(RegistrationSuccessful));
            }

            try
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = EncodeTokenForEmail(token);
                var confirmationLink = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = user.Id, token = encodedToken },
                    protocol: GetPublicRequestScheme());

                if (string.IsNullOrWhiteSpace(confirmationLink))
                    throw new InvalidOperationException("Unable to generate email confirmation link.");

                var registerVm = new RegisterViewModel
                {
                    Email = user.Email ?? email,
                    FirstName = user.FirstName ?? string.Empty,
                    LastName = user.LastName ?? string.Empty
                };

                var mailRequest = EmailRequest.CreateRegistrationVerification(registerVm, confirmationLink);
                await _emailService.SendEmailAsync(mailRequest);

                TempData["ConfirmationEmailSent"] = true;
                TempData["EmailWarning"] = null;
                TempData["SuccessMessage"] = "Confirmation email sent. Please check your inbox and spam folder.";
                _logger.LogInformation("ResendRegistrationEmail: confirmation email sent to {Email}.", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ResendRegistrationEmail: failed for {Email}.", email);
                TempData["ConfirmationEmailSent"] = false;
                TempData["EmailWarning"] =
                    "We could not send the confirmation email right now. Please try again in a few minutes, or contact support.";
            }

            return RedirectToAction(nameof(RegistrationSuccessful));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("ConfirmEmail: Missing userId or token.");
                return View("Error");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("ConfirmEmail: User not found. ID: {UserId}", userId);
                return View("Error");
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                _logger.LogInformation("ConfirmEmail: Email already confirmed for user {Email}.", user.Email);
                return View("ConfirmEmailAlreadyConfirmed");
            }

            // Decode Base64Url token (new emails) with UrlDecode fallback for older links.
            token = DecodeTokenFromEmail(token);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                var legacyToken = WebUtility.UrlDecode(token);
                if (!string.Equals(legacyToken, token, StringComparison.Ordinal))
                {
                    result = await _userManager.ConfirmEmailAsync(user, legacyToken);
                }
            }

            if (result.Succeeded)
            {
                _logger.LogInformation("ConfirmEmail: Email successfully confirmed for user {Email}.", user.Email);
                return View("ConfirmEmailSuccess");
            }

            // Log errors if any
            _logger.LogWarning("ConfirmEmail: Failed for user {Email}. Errors: {Errors}",
                user.Email, string.Join(", ", result.Errors.Select(e => e.Description)));

            return View("ConfirmEmailFailed");
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
                                    new { ReturnUrl = returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null!, string remoteError = null!)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View("Login", loginViewModel);
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Error loading external login information.");

                return View("Login", loginViewModel);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            ApplicationUser user = new ApplicationUser();
            if (email != null)
            {
                user = await _userManager.FindByEmailAsync(email);

                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View("Login", loginViewModel);
                }
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {

                if (email != null)
                {
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await _userManager.CreateAsync(user);
                    }

                    await _userManager.AddLoginAsync(user!, info);
                    await _signInManager.SignInAsync(user!, isPersistent: false);

                    return LocalRedirect(returnUrl ?? string.Empty);
                }

                ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please contact support on admin@intlrealty.com";

                return View("Error");
            }
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> AdminResetPassword(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return Json(new { success = false, error = "Invalid request." });

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Json(new { success = false, error = "User not found." });

            string newPassword = GenerateRandomPassword(12);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                return Json(new { success = true, generatedPassword = newPassword });
            }
            else
            {
                string error = string.Join("; ", result.Errors.Select(e => e.Description));
                return Json(new { success = false, error });
            }
        }

        public static string GenerateRandomPassword(int length = 12)
        {
            const string valid = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789!@$%";
            var res = new char[length];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                var bytes = new byte[length];
                rng.GetBytes(bytes);
                for (int i = 0; i < length; i++)
                {
                    res[i] = valid[bytes[i] % valid.Length];
                }
            }
            return new string(res);
        }

        /// <summary>
        /// Prefer https for emailed links on shared hosting where SSL terminates before the app.
        /// </summary>
        private string GetPublicRequestScheme()
        {
            var forwardedProto = Request.Headers["X-Forwarded-Proto"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(forwardedProto))
                return forwardedProto.Split(',')[0].Trim();

            var host = Request.Host.Host;
            if (string.Equals(host, "localhost", StringComparison.OrdinalIgnoreCase) ||
                host.StartsWith("127.0.0.1", StringComparison.OrdinalIgnoreCase))
            {
                return Request.Scheme;
            }

            return "https";
        }

        private static string EncodeTokenForEmail(string token)
        {
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        }

        private static string DecodeTokenFromEmail(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return token;

            try
            {
                return Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            }
            catch (Exception)
            {
                // Older emails may still use UrlEncode / raw Identity tokens.
                return WebUtility.UrlDecode(token);
            }
        }
    }
}
