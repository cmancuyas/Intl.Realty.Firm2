using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.LeaseCoopVM
{
    public class EditLeaseCoopWithTabMenuViewModel
    {
        public EditLeaseCoopViewModel EditLeaseCoopViewModel { get; set; } = new EditLeaseCoopViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
