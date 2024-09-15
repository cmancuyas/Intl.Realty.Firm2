using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.RoleVM
{
    public class EditRoleWithTabMenuViewModel
    {
        public EditRoleViewModel EditRole { get; set; } = new EditRoleViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
