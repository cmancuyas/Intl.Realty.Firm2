using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.EmploymentStatusVM
{
    public class CreateEmploymentStatusWithTabMenuViewModel
    {
        public CreateEmploymentStatusViewModel CreateEmploymentStatus { get; set; } = new CreateEmploymentStatusViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
