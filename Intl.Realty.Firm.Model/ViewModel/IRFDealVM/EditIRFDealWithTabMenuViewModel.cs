using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.IRFDealVM
{
    public class EditIRFDealWithTabMenuViewModel
    {
        public EditIRFDealViewModel EditIRFDeal { get; set; } = new EditIRFDealViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
