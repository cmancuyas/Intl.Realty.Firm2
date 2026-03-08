using Intl.Realty.Firm.Model.Menu;
using Intl.Realty.Firm.Model.Model;
using Microsoft.AspNetCore.Identity;

namespace Intl.Realty.Firm.Model.ViewModel.RoleVM
{
    public class RoleListPartialViewModel
    {
        public List<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();
        public List<TabMenu> Menus { get; set; } = new List<TabMenu>();
    }
}
