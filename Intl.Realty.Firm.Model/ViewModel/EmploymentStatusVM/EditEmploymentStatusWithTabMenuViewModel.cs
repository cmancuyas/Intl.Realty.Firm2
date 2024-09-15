using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.EmploymentStatusVM
{
    public class EditEmploymentStatusWithTabMenuViewModel
    {
        public EditEmploymentStatusViewModel EditEmploymentStatus { get; set; } = new EditEmploymentStatusViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
