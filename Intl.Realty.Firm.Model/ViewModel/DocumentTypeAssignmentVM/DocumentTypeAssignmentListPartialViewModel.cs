using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.DocumentTypeAssignmentVM
{
    public class DocumentTypeAssignmentListPartialViewModel
    {
        public List<DocumentTypeAssignmentViewModel> DocumentTypeAssignmentViewModels { get; set; } = new List<DocumentTypeAssignmentViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
