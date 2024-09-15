using Intl.Realty.Firm.Model.Model.Identity;

namespace Intl.Realty.Firm.Model.ViewModel.AccountVM
{
    public class UserClaimsViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public List<UserClaim> Claims { get; set; } = new List<UserClaim>();
    }
}
