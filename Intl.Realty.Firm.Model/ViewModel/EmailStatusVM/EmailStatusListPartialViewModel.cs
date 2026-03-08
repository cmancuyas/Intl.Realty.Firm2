using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.EmailStatusVM
{
    public class EmailStatusListPartialViewModel
    {
        public List<EmailStatusViewModel> ConsultantViewModels { get; set; } = new List<EmailStatusViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
