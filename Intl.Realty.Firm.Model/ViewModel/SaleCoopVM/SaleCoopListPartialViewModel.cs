using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.SaleCoopVM
{
    public class SaleCoopListPartialViewModel
    {
        public List<SaleCoopViewModel> SaleCoopListViewModel { get; set; } = new List<SaleCoopViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
