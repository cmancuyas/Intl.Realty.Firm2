using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.SaleCoopVM
{
    public class CreateSaleCoopWithTabMenuViewModel
    {
        public CreateSaleCoopViewModel CreateSaleCoop { get; set; } = new CreateSaleCoopViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
