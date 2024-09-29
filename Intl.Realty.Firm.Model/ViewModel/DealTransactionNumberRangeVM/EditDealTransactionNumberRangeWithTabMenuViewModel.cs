using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.DealTransactionNumberRangeVM
{
    public class EditDealTransactionNumberRangeWithTabMenuViewModel
    {
        public EditDealTransactionNumberRangeViewModel EditDealTransactionNumberRange { get; set; } = new EditDealTransactionNumberRangeViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
