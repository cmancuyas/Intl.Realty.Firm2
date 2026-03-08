using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.CustomerVM
{
    public class CustomerListPartialViewModel
    {
        public List<CustomerViewModel> CustomerViewModels { get; set; } = new List<CustomerViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
