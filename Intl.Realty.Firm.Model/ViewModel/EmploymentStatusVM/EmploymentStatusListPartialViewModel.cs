using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.EmploymentStatusVM
{
    public class EmploymentStatusListPartialViewModel
    {
        public List<EmploymentStatusViewModel> ConsultantViewModels { get; set; } = new List<EmploymentStatusViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
