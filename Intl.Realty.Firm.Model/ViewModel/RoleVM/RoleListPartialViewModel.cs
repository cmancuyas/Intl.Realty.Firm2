using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.RoleVM
{
    public class RoleListPartialViewModel
    {
        public List<RoleViewModel> Roles { get; set; } = new List<RoleViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
