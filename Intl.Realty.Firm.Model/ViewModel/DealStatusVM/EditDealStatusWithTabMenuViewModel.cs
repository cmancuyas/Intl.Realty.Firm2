using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.DealStatusVM
{
    public class EditDealStatusWithTabMenuViewModel
    {
        public EditDealStatusViewModel EditDealStatus { get; set; } = new EditDealStatusViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
