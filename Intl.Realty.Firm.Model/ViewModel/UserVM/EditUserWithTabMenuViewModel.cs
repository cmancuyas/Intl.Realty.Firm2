using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.UserVM
{
    public class EditUserWithTabMenuViewModel
    {
        public EditUserViewModel EditUser { get; set; } = new EditUserViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
