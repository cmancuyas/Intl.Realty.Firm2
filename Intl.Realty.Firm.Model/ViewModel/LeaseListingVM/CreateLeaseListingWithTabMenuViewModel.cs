using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.LeaseListingVM
{
    public class CreateLeaseListingWithTabMenuViewModel
    {
        public CreateLeaseListingViewModel CreateLeaseListing { get; set; } = new CreateLeaseListingViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
