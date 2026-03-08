using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.InternalEmailVM
{
    public class EditInternalEmailWithTabMenuViewModel
    {
        public EditInternalEmailViewModel EditInternalEmail { get; set; } = new EditInternalEmailViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
