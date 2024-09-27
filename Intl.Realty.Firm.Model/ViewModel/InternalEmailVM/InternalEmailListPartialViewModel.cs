using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.InternalEmailVM
{
    public class InternalEmailListPartialViewModel
    {
        public List<InternalEmailViewModel> ConsultantViewModels { get; set; } = new List<InternalEmailViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
