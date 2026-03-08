using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.DepartmentVM
{
    public class EditDepartmentWithTabMenuViewModel
    {
        public EditDepartmentViewModel EditDepartment { get; set; } = new EditDepartmentViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
