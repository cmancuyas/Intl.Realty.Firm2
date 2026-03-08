using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.EmailCategoryVM
{
    public class EditEmailCategoryWithTabMenuViewModel
    {
        public EditEmailCategoryViewModel EditEmailCategory { get; set; } = new EditEmailCategoryViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
