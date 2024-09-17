using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.UserVM;
namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class UserMapper
    {
        public static List<UserExport> ToUserListExport(this List<ApplicationUser> modelList)
        {
            List<UserExport> exportList = new List<UserExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new UserExport
                {
                    FirstName = x.FirstName!,
                    LastName = x.LastName!,
                    Email = x.Email!
                }).ToList();
            }
            return exportList;
        }
        public static PreviewUserViewModel ToPreviewUserViewModel(this ApplicationUser model)
        {
            return new PreviewUserViewModel
            {
                Id = model.Id,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Suffix = model.Suffix,
                Email = model.Email!,
                ProfilePictureId = model.ProfilePictureId,
            };
        }
    }
}
