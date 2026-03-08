using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Model.ViewModel.ProvinceVM
{
    public class ProvinceListPartialViewModel
    {
        public List<ProvinceViewModel> ConsultantViewModels { get; set; } = new List<ProvinceViewModel>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
