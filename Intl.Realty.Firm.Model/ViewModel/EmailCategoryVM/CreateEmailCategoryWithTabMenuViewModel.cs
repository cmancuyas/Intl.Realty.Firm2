using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.EmailCategoryVM
{
    public class CreateEmailCategoryWithTabMenuViewModel
    {
        public CreateEmailCategoryViewModel CreateEmailCategory { get; set; } = new CreateEmailCategoryViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
