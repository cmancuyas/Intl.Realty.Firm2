using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeVM
{
    public class EditTransactionTypeWithTabMenuViewModel
    {
        public EditTransactionTypeViewModel EditTransactionType { get; set; } = new EditTransactionTypeViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
