using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.IRFDealVM
{
    public class IRFDealListPartialViewModel
    {
        public List<IRFDealViewModel> IRFDealViewModels { get; set; } = new List<IRFDealViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
