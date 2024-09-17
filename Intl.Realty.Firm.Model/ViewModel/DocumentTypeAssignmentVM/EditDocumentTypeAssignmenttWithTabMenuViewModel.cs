using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.DocumentTypeAssignmentVM
{
    public class EditDocumentTypeAssignmentWithTabMenuViewModel
    {
        public EditDocumentTypeAssignmentViewModel EditDocumentTypeAssignment { get; set; } = new EditDocumentTypeAssignmentViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
