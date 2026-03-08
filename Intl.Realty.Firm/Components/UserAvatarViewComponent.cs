using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.UserAvatarVM;
using Intl.Realty.Firm.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Intl.Realty.Firm.Components
{
    public class UserAvatarViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserAvatarViewComponent(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null) return View(new UserAvatarViewModel());

            var profileImageUrl = "/files/img/avatar-blank.png";

            if (user.ProfileImageId != null)
            {
                var image = await _unitOfWork.ProfileImage.GetAsync(p => p.Id == user.ProfileImageId);
                if (image != null)
                {
                    profileImageUrl = image.FullPath;
                }
            }

            var roles = await _userManager.GetRolesAsync(user);

            var model = new UserAvatarViewModel
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email ?? "",
                ProfileImageUrl = profileImageUrl,
                Roles = roles.ToList()
            };

            return View("Default", model);
        }
    }
}
