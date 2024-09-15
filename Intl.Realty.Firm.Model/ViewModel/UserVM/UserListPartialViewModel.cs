using Intl.Realty.Firm.Model.Menu;
using Intl.Realty.Firm.Model.Model;

namespace Intl.Realty.Firm.Model.ViewModel.UserVM
{
    public class UserListPartialViewModel
    {
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
