using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.LeaseCoopVM
{
    public class LeaseCoopListPartialViewModel
    {
        public List<LeaseCoopViewModel> LeaseCoopListViewModel { get; set; } = new List<LeaseCoopViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
