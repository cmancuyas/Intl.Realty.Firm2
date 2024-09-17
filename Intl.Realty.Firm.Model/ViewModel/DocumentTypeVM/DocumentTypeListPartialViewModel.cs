using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.DocumentTypeVM
{
    public class DocumentTypeListPartialViewModel
    {
        public List<DocumentTypeViewModel> ConsultantViewModels { get; set; } = new List<DocumentTypeViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
