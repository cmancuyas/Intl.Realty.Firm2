using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.EmailStatusVM
{
    public class EditEmailStatusWithTabMenuViewModel
    {
        public EditEmailStatusViewModel EditEmailStatus { get; set; } = new EditEmailStatusViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
