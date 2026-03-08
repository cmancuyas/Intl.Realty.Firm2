using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.SaleListingVM
{
    public class CreateSaleListingWithTabMenuViewModel
    {
        public CreateSaleListingViewModel CreateSaleListingViewModel { get; set; } = new CreateSaleListingViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
