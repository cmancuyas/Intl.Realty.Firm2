using Intl.Realty.Firm.Model.Model;

namespace Intl.Realty.Firm.Model.ViewModel.Account
{
    public class UserClaimsViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public List<UserClaim> Claims { get; set; } = new List<UserClaim>();
    }
}
