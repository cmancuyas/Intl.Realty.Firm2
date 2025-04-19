using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.AgentReferralVM
{
    public class CreateAgentReferralWithTabMenuViewModel
    {
        public CreateAgentReferralViewModel CreateAgentReferral { get; set; } = new CreateAgentReferralViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
