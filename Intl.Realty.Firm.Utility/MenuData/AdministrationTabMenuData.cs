using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Utility.MenuData
{
    public static class AdministrationTabMenuData
    {
        public static List<TabMenu> GetAdministrationTabMenuData()
        {
            List<TabMenu> menus = new List<TabMenu>();
            int count = 0;

            // 1st TabMenu
            TabMenu menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "ManageUser",
                Description = "Manage Users",
                Tab = "manageUsers-tab",
                Control = "manageUsers",
            };

            menus.Add(menu);

            // Second TabMenu
            menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "ManageRole",
                Description = "Manage Roles",
                Tab = "manageRoles-tab",
                Control = "manageRoles",
            };

            menus.Add(menu);

            // Third TabMenu
            menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "EmploymentStatus",
                Description = "Employment Status",
                Tab = "employmentStatus-tab",
                Control = "employmentStatus",
            };

            menus.Add(menu);

            // Fourth TabMenu
            menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "Department",
                Description = "Departments",
                Tab = "departments-tab",
                Control = "departments",
            };

            menus.Add(menu);

            return menus;
        }
    }
}
