using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.SaleListingVM
{
    public class EditSaleListingWithTabMenuViewModel
    {
        public EditSaleListingViewModel EditSaleListingViewModel { get; set; } = new EditSaleListingViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
