using Intl.Realty.Firm.Helper;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel;
using Intl.Realty.Firm.Model.ViewModel.AccountVM;
using Intl.Realty.Firm.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            //var captchaResult = await _googleCaptchaService.VerifyToken(viewModel.Token);
            //if (!captchaResult)
            //{
            //    return View(viewModel);
            //}

            viewModel.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email not found");
                return View(viewModel);
            }

            // Check if password is valid before confirming email
            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, viewModel.Password);
            if (!user.EmailConfirmed && isPasswordValid)
            {
                ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                return View(viewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(
                                viewModel.Email,
                                viewModel.Password,
                                viewModel.RememberMe,
                                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                // Secure handling of returnUrl
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    _logger.LogWarning("Suspicious return URL detected: {returnUrl}", returnUrl);
                }

                // Retrieve user roles
                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles.Contains("Admin") || userRoles.Contains("SuperAdmin"))
                {
                    return RedirectToAction("Dashboard", "Home");
                }

                // If user has no roles assigned, redirect to Training/Index
                if (!userRoles.Any())
                {
                    return RedirectToAction("Index", "Training");
                }

                // Default redirection for other users
                return RedirectToAction("Dashboard", "Home");

            }

            if (result.IsLockedOut)
            {
                return View("AccountLocked", new { message = "Your account has been locked due to multiple failed login attempts. Please try again later." });
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(viewModel);
        }

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

            // Prevent user enumeration by always returning the same response
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                _logger.LogWarning("Password reset attempted for non-existent or unconfirmed email: {Email}", viewModel.Email);
                return View("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = System.Web.HttpUtility.UrlEncode(token); // Ensure the token is URL-safe

            var passwordResetLink = Url.Action("ResetPassword", "Account",
                new { email = viewModel.Email, token = encodedToken }, Request.Scheme);

            try
            {
                var mailRequest = EmailRequest.CreateResetPasswordEmailRequest(user, passwordResetLink!);
                await _emailService.SendEmailAsync(mailRequest);
                _logger.LogInformation("Password reset email sent to user: {Email}", viewModel.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send password reset email to {Email}", viewModel.Email);
            }

            // Always return ForgotPasswordConfirmation view, regardless of email existence
            return View("ForgotPasswordConfirmation");
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
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            var captchaResult = await _googleCaptchaService.VerifyToken(viewModel.Token);
            if (!captchaResult)
            {
                ModelState.AddModelError("", "Captcha verification failed. Please try again.");
                return View(viewModel);
            }

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

                var applicationUser = new ApplicationUser
                {
                    UserName = viewModel.Email,
                    Email = viewModel.Email,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    DepartmentId = (viewModel.DepartmentId == 0) ? 1 : viewModel.DepartmentId,
                    EmploymentStatusId = 1,
                    ProfilePictureId = viewModel.ProfilePictureId ?? 0,
                    UserAgreement = viewModel.UserAgreement
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
