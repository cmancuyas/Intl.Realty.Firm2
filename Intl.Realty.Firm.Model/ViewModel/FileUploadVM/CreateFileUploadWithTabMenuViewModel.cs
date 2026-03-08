using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.FileUploadVM
{
    public class CreateFileUploadWithTabMenuViewModel
    {
        public CreateFileUploadViewModel CreateFileUpload { get; set; } = new CreateFileUploadViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
