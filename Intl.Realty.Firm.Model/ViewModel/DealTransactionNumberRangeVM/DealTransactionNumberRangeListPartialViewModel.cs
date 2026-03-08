using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.DealTransactionNumberRangeVM
{
    public class DealTransactionNumberRangeListPartialViewModel
    {
        public List<DealTransactionNumberRangeViewModel> ConsultantViewModels { get; set; } = new List<DealTransactionNumberRangeViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
