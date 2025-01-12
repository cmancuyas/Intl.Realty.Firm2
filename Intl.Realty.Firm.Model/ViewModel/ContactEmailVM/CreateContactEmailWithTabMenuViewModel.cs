using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.ContactEmailVM
{
    public class CreateContactEmailWithTabMenuViewModel
    {
        public CreateContactEmailViewModel CreateContactEmail { get; set; } = new CreateContactEmailViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
