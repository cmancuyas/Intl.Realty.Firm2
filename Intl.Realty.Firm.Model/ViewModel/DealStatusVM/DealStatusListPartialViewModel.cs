using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.DealStatusVM
{
    public class DealStatusListPartialViewModel
    {
        public List<DealStatusViewModel> ConsultantViewModels { get; set; } = new List<DealStatusViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
