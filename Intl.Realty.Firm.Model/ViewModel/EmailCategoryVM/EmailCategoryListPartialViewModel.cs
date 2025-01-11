using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.EmailCategoryVM
{
    public class EmailCategoryListPartialViewModel
    {
        public List<EmailCategoryViewModel> EmailCategoryViewListModel { get; set; } = new List<EmailCategoryViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
