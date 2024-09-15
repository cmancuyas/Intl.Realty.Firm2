using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.DepartmentVM
{
    public class CreateDepartmentWithTabMenuViewModel
    {
        public CreateDepartmentViewModel CreateDepartment { get; set; } = new CreateDepartmentViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
