using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.FileUploadVM
{
    public class FileUploadListPartialViewModel
    {
        public List<FileUploadViewModel> ConsultantViewModels { get; set; } = new List<FileUploadViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
