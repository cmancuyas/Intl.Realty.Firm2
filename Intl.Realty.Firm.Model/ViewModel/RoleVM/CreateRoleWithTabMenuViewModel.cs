using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.RoleVM
{
    public class CreateRoleWithTabMenuViewModel
    {
        public CreateRoleViewModel CreateRole { get; set; } = new CreateRoleViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
