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
using System.Net;
using System.Security.Claims;

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
            _logger.LogInformation("📨 SubmitLoginIssue triggered. Email: {Email}", model.Email);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "❌ ModelState invalid. Errors: {Errors}",
                    string.Join(" | ",
                        ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                );

                return View("LoginIssueSupport", model);
            }

            // ✅ reCAPTCHA v3 validation
            if (!await _googleCaptchaService.VerifyToken(token, expectedAction: "login_issue", minScore: 0.5))
            {
                _logger.LogWarning("❌ reCAPTCHA failed for LoginIssueSupport. Email: {Email}", model.Email);
                ModelState.AddModelError(string.Empty, "Captcha verification failed. Please try again.");
                return View("LoginIssueSupport", model);
            }

            try
            {
                var recipients = _configurationService.GetSupportEmail() ?? Array.Empty<string>();
                var recipientsCC = _configurationService.GetSupportEmailCC() ?? Array.Empty<string>();

                if (!recipients.Any())
                {
                    _logger.LogWarning("❗ No recipients configured in GetSupportEmail()");
                    ModelState.AddModelError(string.Empty, "Support email is not configured.");
                    return View("LoginIssueSupport", model);
                }

                var mailRequest = EmailRequest.SendEmailLoginIssue(
                    model,
                    string.Join(',', recipients),
                    string.Join(',', recipientsCC)
                );

                await _emailService.SendEmailAsync(mailRequest);

                _logger.LogInformation("✅ Login issue email sent for {Email}", model.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error sending login issue email for {Email}", model.Email);
                ModelState.AddModelError(string.Empty, "There was a problem sending your request. Please try again later.");
                return View("LoginIssueSupport", model);
            }

            TempData["SuccessMessage"] = "Your login issue has been submitted successfully.";
            return RedirectToAction("LoginIssueConfirmation");
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
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await _userManager.FindByEmailAsync(viewModel.Email);

            // If user doesn't exist, redirect silently to avoid user enumeration
            if (user == null)
            {
                _logger.LogWarning("Password reset attempted for non-existent email: {Email}", viewModel.Email);
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            // If email is not confirmed, show custom view or error
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                _logger.LogWarning("Password reset attempted for unconfirmed email: {Email}", viewModel.Email);

                // You could return a view with the option to resend confirmation
                return View("EmailNotConfirmed", new ResendConfirmationViewModel
                {
                    Email = viewModel.Email
                });
            }

            // Generate reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);

            var passwordResetLink = Url.Action("ResetPassword", "Account",
                new { email = viewModel.Email, token = encodedToken }, Request.Scheme);

            try
            {
                var mailRequest = EmailRequest.CreateResetPasswordEmailRequest(user, passwordResetLink);
                await _emailService.SendEmailAsync(mailRequest);
                _logger.LogInformation("Password reset email sent to user: {Email}", viewModel.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send password reset email to {Email}", viewModel.Email);
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
        public async Task<IActionResult> ResendEmailConfirmation(ResendConfirmationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("EmailNotConfirmed", model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || await _userManager.IsEmailConfirmedAsync(user))
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);

            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = encodedToken }, protocol: Request.Scheme);
            var mailRequest = EmailRequest.CreateEmailConfirmationRequest(user, callbackUrl);
            await _emailService.SendEmailAsync(mailRequest);

            _logger.LogInformation("Confirmation email resent to: {Email}", user.Email);

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
                Email = email,
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
                return View(model); // ✅ Ensure validation errors are returned
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid password reset request.";
                return RedirectToAction("ForgotPassword");
            }

            // ✅ Decode the token safely
            var decodedToken = WebUtility.UrlDecode(model.Token);
            _logger.LogInformation("Decoded Token for {Email}: {Token}", model.Email, decodedToken); // ✅ Debugging

            var resetResult = await _userManager.ResetPasswordAsync(user, decodedToken, model.Password);

            if (!resetResult.Succeeded)
            {
                _logger.LogError("Password reset failed for user: {Email}. Errors: {Errors}",
                    model.Email, string.Join(", ", resetResult.Errors.Select(e => e.Description)));

                foreach (var error in resetResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

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

            // -------- Try to send email, but do not block role assignment on failure --------
            try
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                var confirmationLink = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = applicationUser.Id, token },
                    protocol: Request.Scheme);

                var mailRequest = EmailRequest.CreateRegistrationVerification(viewModel, confirmationLink!);
                await _emailService.SendEmailAsync(mailRequest);

                _logger.LogInformation("Registration: confirmation email queued for {Email}.", applicationUser.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration: failed to send confirmation email to {Email}. Continuing.", applicationUser.Email);
                // Intentionally swallow exception — we still proceed to add role and succeed the flow
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

            return View("RegistrationSuccessful");
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

            // Optional: decode the token (ASP.NET Core usually handles this automatically)
            token = WebUtility.UrlDecode(token);

            var result = await _userManager.ConfirmEmailAsync(user, token);
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
    }
}
