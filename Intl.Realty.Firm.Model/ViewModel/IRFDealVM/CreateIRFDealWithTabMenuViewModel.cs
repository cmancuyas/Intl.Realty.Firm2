using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.IRFDealVM
{
    public class CreateIRFDealWithTabMenuViewModel
    {
        public CreateIRFDealViewModel CreateIRFDeal { get; set; } = new CreateIRFDealViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
