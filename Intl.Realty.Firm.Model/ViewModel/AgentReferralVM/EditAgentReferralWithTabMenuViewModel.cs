using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.AgentReferralVM
{
    public class EditAgentReferralWithTabMenuViewModel
    {
        public EditAgentReferralViewModel EditAgentReferral { get; set; } = new EditAgentReferralViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
