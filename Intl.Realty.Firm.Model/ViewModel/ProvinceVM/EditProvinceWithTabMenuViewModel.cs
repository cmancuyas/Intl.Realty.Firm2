using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.ProvinceVM
{
    public class EditProvinceWithTabMenuViewModel
    {
        public EditProvinceViewModel EditProvince { get; set; } = new EditProvinceViewModel();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
