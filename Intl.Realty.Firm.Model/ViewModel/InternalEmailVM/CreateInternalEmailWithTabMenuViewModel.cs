using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.InternalEmailVM
{
    public class CreateInternalEmailWithTabMenuViewModel
    {
        public CreateInternalEmailViewModel CreateInternalEmail { get; set; } = new CreateInternalEmailViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
