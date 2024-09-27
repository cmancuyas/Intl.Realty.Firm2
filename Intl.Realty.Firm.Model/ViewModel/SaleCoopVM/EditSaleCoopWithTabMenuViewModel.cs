using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.SaleCoopVM
{
    public class EditSaleCoopWithTabMenuViewModel
    {
        public EditSaleCoopViewModel EditSaleCoop { get; set; } = new EditSaleCoopViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
