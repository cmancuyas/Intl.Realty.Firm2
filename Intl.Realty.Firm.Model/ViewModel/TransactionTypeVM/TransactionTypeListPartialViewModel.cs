using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeVM
{
    public class TransactionTypeListPartialViewModel
    {
        public List<TransactionTypeViewModel> ConsultantViewModels { get; set; } = new List<TransactionTypeViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
