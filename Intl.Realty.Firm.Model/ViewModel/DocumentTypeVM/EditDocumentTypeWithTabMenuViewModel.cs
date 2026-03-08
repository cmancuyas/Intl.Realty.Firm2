using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.DocumentTypeVM
{
    public class EditDocumentTypeWithTabMenuViewModel
    {
        public EditDocumentTypeViewModel EditDocumentType { get; set; } = new EditDocumentTypeViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
