using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.AgentReferralVM
{
    public class AgentReferralListPartialViewModel
    {
        public List<AgentReferralViewModel> AgentReferralViewModels { get; set; } = new List<AgentReferralViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
