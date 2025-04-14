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

        public AccountController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                IEmailService emailService,
                                ILogger<AccountController> logger,
                                IGoogleCaptchaService googleCaptchaService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _logger = logger;
            _googleCaptchaService = googleCaptchaService;
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
        public async Task<IActionResult> Login(LoginViewModel viewModel, string? returnUrl)
        {
            viewModel.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await _userManager.FindByEmailAsync(viewModel.Email);

            // ✅ Prevent user enumeration: Always return the same error
            if (user == null || !await _userManager.CheckPasswordAsync(user, viewModel.Password))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View(viewModel); // ✅ Must return the view — not redirect
            }

            // ✅ Use SignInManager to check if email is confirmed
            var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.RememberMe, lockoutOnFailure: true);

            if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                return View(viewModel);
            }

            if (result.IsLockedOut)
            {
                return RedirectToAction("Lockout");
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
                return View(viewModel);
            }

            // ✅ Secure returnUrl handling
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            _logger.LogWarning("Suspicious return URL detected: {returnUrl}", returnUrl);

            // ✅ Redirect users based on role
            var userRoles = await _userManager.GetRolesAsync(user);

            var hasRole = userRoles.Any(); // ✅ Check if the user has any role

            if (hasRole)
            {
                return RedirectToAction("Dashboard", "Home");
            }

            return RedirectToAction("Index", "Training"); // ✅ Default fallback if no roles assigned

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

            return RedirectToAction("ConfirmationEmailSent");
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




        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            //_logger.LogInformation("🔍 Received reCAPTCHA Token: {Token}", viewModel.Token);

            //var captchaResult = await _googleCaptchaService.VerifyToken(viewModel.Token);
            //if (!captchaResult)
            //{
            //    _logger.LogError("❌ reCAPTCHA verification failed for user: {Email}", viewModel.Email);
            //    ModelState.AddModelError("", "Captcha verification failed. Please try again.");
            //    return View(viewModel);
            //}

            //_logger.LogInformation("✅ reCAPTCHA verification successful for user: {Email}", viewModel.Email);

            if (!viewModel.UserAgreement)
            {
                ModelState.AddModelError("", "You must agree to the terms and conditions to register.");
                return View(viewModel);
            }

            if (ModelState.IsValid)
            {
                if (viewModel.DepartmentId == 0)
                {
                    _logger.LogWarning("Register: Defaulting DepartmentId to 1 for user {Email}", viewModel.Email);
                }

                // 🔹 Capture TeamLeader ID from the form dropdown
                Guid? teamLeaderId = viewModel.TeamLeaderId;

                // 🔹 If the current user is a TeamLeader, override the dropdown selection
                if (User.Identity.IsAuthenticated && User.IsInRole("TeamLeader"))
                {
                    teamLeaderId = Guid.Parse(_userManager.GetUserId(User));
                }

                var applicationUser = new ApplicationUser
                {
                    UserName = viewModel.Email,
                    Email = viewModel.Email,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    DepartmentId = (viewModel.DepartmentId == 0) ? 1 : viewModel.DepartmentId,
                    EmploymentStatusId = 1,
                    ProfilePictureId = viewModel.ProfilePictureId ?? 0,
                    UserAgreement = viewModel.UserAgreement,
                    RegistrationDate = DateTime.UtcNow,
                    TeamLeaderId = teamLeaderId // ✅ Assign the selected TeamLeader
                };

                var result = await _userManager.CreateAsync(applicationUser, viewModel.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                                                                new { userId = applicationUser.Id, token = token }, Request.Scheme);

                    var mailRequest = EmailRequest.CreateRegistrationVerification(viewModel, confirmationLink!);
                    await _emailService.SendEmailAsync(mailRequest);

                    _logger.Log(LogLevel.Warning, confirmationLink);

                    if (_signInManager.IsSignedIn(User) && (User.IsInRole("Admin") || User.IsInRole("SuperAdmin")))
                    {
                        return RedirectToAction("AdminDashboard", "Home");
                    }

                    await _userManager.AddToRoleAsync(applicationUser, "Agent");

                    if (viewModel.AutoSignInAfterRegister)
                    {
                        await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                    }

                    return View("ConfirmEmailSent");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // 🔹 Reload TeamLeaders for the dropdown before returning the view
            viewModel.TeamLeaders = (await _userManager.GetUsersInRoleAsync("TeamLeader"))
                .Select(tl => new SelectListItem
                {
                    Value = tl.Id.ToString(),
                    Text = $"{tl.FirstName} {tl.LastName}"
                }).ToList();

            return View(viewModel);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = ($"The USER ID {userId} is invalid");
                return View("NotFound");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return View();
            }

            ErrorViewModel errorViewModel = new ErrorViewModel();
            errorViewModel.ErrorTitle = "Email cannot be confirmed";
            return View("Error", errorViewModel);
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
    }
}
