using Intl.Realty.Firm.Model.ViewModel.AccountVM;

namespace Intl.Realty.Firm.Model.ViewModel.UserVM
{
    public class ManageUserRolesViewModel
    {
        public Guid UserId { get; set; }
        public List<UserRolesViewModel> Roles { get; set; } = new List<UserRolesViewModel>();
    }
}
