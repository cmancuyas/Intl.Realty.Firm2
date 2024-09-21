using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.FileUploadVM
{
    public class EditFileUploadWithTabMenuViewModel
    {
        public EditFileUploadViewModel EditFileUpload { get; set; } = new EditFileUploadViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
