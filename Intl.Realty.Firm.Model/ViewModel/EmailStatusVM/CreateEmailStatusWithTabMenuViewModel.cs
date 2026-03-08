using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.EmailStatusVM
{
    public class CreateEmailStatusWithTabMenuViewModel
    {
        public CreateEmailStatusViewModel CreateEmailStatus { get; set; } = new CreateEmailStatusViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
