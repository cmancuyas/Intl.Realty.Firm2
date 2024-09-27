using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.LeaseListingVM
{
    public class EditLeaseListingWithTabMenuViewModel
    {
        public EditLeaseListingViewModel EditLeaseListing { get; set; } = new EditLeaseListingViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
