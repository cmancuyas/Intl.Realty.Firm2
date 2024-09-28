using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.UserProfileVM;
namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class UserProfileMapper
    {
        public static UserProfileViewModel ToUserProfileViewModel(this ApplicationUser model)
        {
            return new UserProfileViewModel
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email!,
                EmailConfirmed = model.EmailConfirmed,
                ProfilePictureId = model.ProfilePictureId,
            };
        }
    }
}
