using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.ContactEmailVM
{
    public class EditContactEmailWithTabMenuViewModel
    {
        public EditContactEmailViewModel EditContactEmail { get; set; } = new EditContactEmailViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
