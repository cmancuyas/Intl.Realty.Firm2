using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.ContactEmailVM
{
    public class ContactEmailListPartialViewModel
    {
        public List<ContactEmailViewModel> ConsultantViewModels { get; set; } = new List<ContactEmailViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
