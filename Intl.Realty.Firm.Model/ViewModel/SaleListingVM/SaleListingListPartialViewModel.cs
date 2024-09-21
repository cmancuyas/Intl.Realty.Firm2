using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.SaleListingVM
{
    public class SaleListingListPartialViewModel
    {
        public List<SaleListingViewModel> SaleListingListViewModel { get; set; } = new List<SaleListingViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
