using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.DocumentTypeAssignmentVM
{
    public class CreateDocumentTypeAssignmentWithTabMenuViewModel
    {
        public CreateDocumentTypeAssignmentViewModel CreateDocumentTypeAssignment { get; set; } = new CreateDocumentTypeAssignmentViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
