using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.UserProfileVM;
using Intl.Realty.Firm.Repository.IRepository;
using Intl.Realty.Firm.Service.IService;
using Intl.Realty.Firm.Utility.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Intl.Realty.Firm.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHandlerService _fileHandlerService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserProfileController(UserManager<ApplicationUser> userManager,
                                     SignInManager<ApplicationUser> signInManager,
                                     IUnitOfWork unitOfWork,
                                     IFileHandlerService fileHandlerService,
                                     IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _fileHandlerService = fileHandlerService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> Index(Guid id)
        {
            var userModel = await _userManager.Users
                .Include(u => u.ProfileImage)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (userModel == null)
                return NotFound();

            var viewModel = userModel.ToUserProfileViewModel();

            // Add this line if your ViewModel has ProfileImageUrl
            viewModel.ProfileImageUrl = userModel.ProfileImage?.FullPath ?? "/files/img/avatar-blank.png";

            // 🔹 Fetch Department Info
            var department = await _unitOfWork.Department.GetAsync(x => x.Id == userModel.DepartmentId);
            viewModel.DepartmentName = department?.Description ?? "No Department Assigned";

            // 🔹 Fetch Employment Status
            var employmentStatus = await _unitOfWork.EmploymentStatus.GetAsync(x => x.Id == userModel.EmploymentStatusId);
            viewModel.EmploymentStatusName = employmentStatus?.Description ?? "Not Specified";

            // 🔹 Fetch User Roles & Claims
            var userRoles = await _userManager.GetRolesAsync(userModel);
            var userClaims = await _userManager.GetClaimsAsync(userModel);
            viewModel.Roles = userRoles.ToList();
            viewModel.Claims = userClaims.Select(x => $"{x.Type} : {x.Value}").ToList();

            // 🔹 Fetch all users with the "TeamLeader" role for the dropdown
            var teamLeaders = await _userManager.GetUsersInRoleAsync("TeamLeader");
            viewModel.TeamLeaders = teamLeaders.Select(tl => new SelectListItem
            {
                Value = tl.Id.ToString(),
                Text = $"{tl.FirstName} {tl.LastName} ({tl.Email})"
            }).ToList();

            // 🔹 Assign the current TeamLeader (if any)
            viewModel.TeamLeaderId = userModel.TeamLeaderId;

            // 🔹 Fetch Team Members if the user is a TeamLeader
            if (userRoles.Contains("TeamLeader"))
            {
                var teamMembers = await _userManager.Users
                    .Where(u => u.TeamLeaderId == userModel.Id)
                    .Select(u => new TeamMemberViewModel
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email
                    }).ToListAsync();

                viewModel.TeamMembers = teamMembers;
            }
            else
            {
                viewModel.TeamMembers = new List<TeamMemberViewModel>(); // Ensure it's not null
            }

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateTeamLeader(Guid userId, Guid? teamLeaderId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return NotFound();
            }

            user.TeamLeaderId = teamLeaderId; // Update the TeamLeader ID

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Team Leader updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update Team Leader.";
            }

            return RedirectToAction("Index", new { id = userId }); // Redirect back to the profile page
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
                    return View(viewModel);
                }

                await _signInManager.RefreshSignInAsync(user);

                // Redirect to ChangePasswordConfirmation and pass the user ID
                return RedirectToAction("ChangePasswordConfirmation", new { userId = user.Id });
            }
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult ChangePasswordConfirmation(Guid userId)
        {
            ViewBag.UserId = userId; // Pass the user ID to the view
            return View();
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
        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(Guid userId, IFormFile profileImage)
        {
            if (profileImage == null || profileImage.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select a valid image.";
                return RedirectToAction("Index", new { id = userId });
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Index", new { id = userId });
            }

            // Delete old profile image if exists
            if (user.ProfileImageId.HasValue)
            {
                var oldImage = await _unitOfWork.ProfileImage.GetAsync(i => i.Id == user.ProfileImageId.Value);
                if (oldImage != null)
                {
                    var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, oldImage.FullPath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                    await _fileHandlerService.DeleteFile(oldPath);
                    await _unitOfWork.ProfileImage.RemoveAsync(oldImage);
                }
            }

            // Save new image using file handler
            var directory = Path.Combine("uploads", "profile-images", userId.ToString());
            var (fileName, extension, originalFileName) = await _fileHandlerService.UploadFile(profileImage, directory);

            var fullPath = Path.Combine("/", directory, fileName + extension).Replace("\\", "/");

            var profileImageEntity = new ProfileImage
            {
                FileName = fileName,
                FileExtension = extension,
                OriginalFileName = originalFileName,
                FullPath = fullPath,
                FileSize = (profileImage.Length / 1024f).ToString("F2") + " KB",
                UploadedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Id
            };

            await _unitOfWork.ProfileImage.AddAsync(profileImageEntity);
            user.ProfileImageId = profileImageEntity.Id;
            await _userManager.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            TempData["SuccessMessage"] = "Profile picture updated successfully.";
            return RedirectToAction("Index", new { id = userId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProfilePicture(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Index", new { id = userId });
            }

            if (!user.ProfileImageId.HasValue)
            {
                TempData["ErrorMessage"] = "No profile picture to delete.";
                return RedirectToAction("Index", new { id = userId });
            }

            var profileImage = await _unitOfWork.ProfileImage.GetAsync(i => i.Id == user.ProfileImageId.Value);
            if (profileImage != null)
            {
                var physicalPath = Path.Combine(_webHostEnvironment.WebRootPath, profileImage.FullPath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

                var success = await _fileHandlerService.DeleteFile(physicalPath);
                if (success)
                {
                    await _unitOfWork.ProfileImage.RemoveAsync(profileImage);
                    user.ProfileImageId = null;
                    await _userManager.UpdateAsync(user);
                    await _unitOfWork.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Profile picture removed successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to remove profile picture file.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Profile image record not found.";
            }

            return RedirectToAction("Index", new { id = userId });
        }


    }
}
