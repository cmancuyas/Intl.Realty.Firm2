using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.LeaseCoopVM
{
    public class CreateLeaseCoopWithTabMenuViewModel
    {
        public CreateLeaseCoopViewModel CreateLeaseCoopViewModel { get; set; } = new CreateLeaseCoopViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
