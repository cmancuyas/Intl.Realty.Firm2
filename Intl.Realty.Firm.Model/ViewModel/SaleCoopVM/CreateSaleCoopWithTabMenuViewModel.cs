using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.SaleCoopVM
{
    public class CreateSaleCoopWithTabMenuViewModel
    {
        public CreateSaleCoopViewModel CreateSaleCoopViewModel { get; set; } = new CreateSaleCoopViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
