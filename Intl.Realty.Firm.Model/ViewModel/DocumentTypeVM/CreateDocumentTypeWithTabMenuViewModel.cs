using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.DocumentTypeVM
{
    public class CreateDocumentTypeWithTabMenuViewModel
    {
        public CreateDocumentTypeViewModel CreateDocumentType { get; set; } = new CreateDocumentTypeViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
