using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.DealStatusVM
{
    public class CreateDealStatusWithTabMenuViewModel
    {
        public CreateDealStatusViewModel CreateDealStatus { get; set; } = new CreateDealStatusViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
