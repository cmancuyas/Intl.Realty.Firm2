using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.LeaseListingVM
{
    public class LeaseListingListPartialViewModel
    {
        public List<LeaseListingViewModel> LeaseListingListViewModel { get; set; } = new List<LeaseListingViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
