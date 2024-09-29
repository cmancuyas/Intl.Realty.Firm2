using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.DealTransactionNumberRangeVM
{
    public class CreateDealTransactionNumberRangeWithTabMenuViewModel
    {
        public CreateDealTransactionNumberRangeViewModel CreateDealTransactionNumberRange { get; set; } = new CreateDealTransactionNumberRangeViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
