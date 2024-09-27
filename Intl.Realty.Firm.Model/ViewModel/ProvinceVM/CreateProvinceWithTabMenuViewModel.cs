using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.ProvinceVM
{
    public class CreateProvinceWithTabMenuViewModel
    {
        public CreateProvinceViewModel CreateProvince { get; set; } = new CreateProvinceViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
