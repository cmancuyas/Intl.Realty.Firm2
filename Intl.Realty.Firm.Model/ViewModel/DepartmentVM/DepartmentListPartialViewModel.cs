using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.DepartmentVM
{
    public class DepartmentListPartialViewModel
    {
        public List<DepartmentViewModel> ConsultantViewModels { get; set; } = new List<DepartmentViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
