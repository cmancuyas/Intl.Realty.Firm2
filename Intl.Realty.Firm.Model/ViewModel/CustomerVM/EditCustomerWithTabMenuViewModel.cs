using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.CustomerVM
{
    public class EditCustomerWithTabMenuViewModel
    {
        public EditCustomerViewModel EditCustomer { get; set; } = new EditCustomerViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
