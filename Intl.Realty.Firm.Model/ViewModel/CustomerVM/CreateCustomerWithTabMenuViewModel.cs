using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.CustomerVM
{
    public class CreateCustomerWithTabMenuViewModel
    {
        public CreateCustomerViewModel CreateCustomer { get; set; } = new CreateCustomerViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
