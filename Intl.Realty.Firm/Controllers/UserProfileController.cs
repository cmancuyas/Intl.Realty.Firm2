using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.UserProfileVM;
using Intl.Realty.Firm.Repository.IRepository;
using Intl.Realty.Firm.Utility.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Intl.Realty.Firm.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserProfileController(UserManager<ApplicationUser> userManager,
                                     SignInManager<ApplicationUser> signInManager,
                                     IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Index(Guid id)
        {
            var userModel = await _userManager.FindByIdAsync(id.ToString());

            if (userModel == null)
            {
                return NotFound();
            }

            var viewModel = userModel.ToUserProfileViewModel();

            var department = await _unitOfWork.Department.GetAsync(x => x.Id == userModel.DepartmentId);
            if (department == null)
            {
                viewModel.DepartmentName = "No Department Assigned";
            }
            var employmentStatus = await _unitOfWork.EmploymentStatus.GetAsync(x => x.Id == userModel.EmploymentStatusId);

            viewModel.EmploymentStatusName = employmentStatus.Description;

            var userRoles = await _userManager.GetRolesAsync(userModel);
            var userClaims = await _userManager.GetClaimsAsync(userModel);
            viewModel.Roles = userRoles.ToList();
            viewModel.Claims = userClaims.Select(x => x.Type + " : " + x.Value).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AddPassword()
        {
            var user = await _userManager.GetUserAsync(User);

            var userHasPassword = await _userManager.HasPasswordAsync(user!);

            if (userHasPassword)
            {
                return RedirectToAction("ChangePassword");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPassword(AddPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var result = await _userManager.AddPasswordAsync(user!, viewModel.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                await _signInManager.RefreshSignInAsync(user!);

                return View("AddPasswordConfirmation");
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);

            var userHasPassword = await _userManager.HasPasswordAsync(user!);

            if (!userHasPassword)
            {
                return RedirectToAction("AddPassword");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                var result = await _userManager.ChangePasswordAsync(user, viewModel.CurrentPassword, viewModel.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View();
                }

                await _signInManager.RefreshSignInAsync(user);

                return View("ChangePasswordConfirmation");
            }
            return View(viewModel);
        }
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(viewModel.Email);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, viewModel.Token, viewModel.Password);
                    if (result.Succeeded)
                    {
                        if (await _userManager.IsLockedOutAsync(user))
                        {
                            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        }

                        return View("ResetPasswordConfirmation");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(viewModel);
                }
                return View("ResetPasswordConfirmation");
            }
            return View(viewModel);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }
    }
}
