using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.UserVM
{
    public class CreateUserWithTabMenuViewModel
    {
        public CreateUserViewModel CreateUser { get; set; } = new CreateUserViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
