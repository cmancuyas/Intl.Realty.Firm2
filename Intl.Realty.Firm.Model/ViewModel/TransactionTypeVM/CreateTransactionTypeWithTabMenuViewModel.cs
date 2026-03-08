using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeVM
{
    public class CreateTransactionTypeWithTabMenuViewModel
    {
        public CreateTransactionTypeViewModel CreateTransactionType { get; set; } = new CreateTransactionTypeViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
