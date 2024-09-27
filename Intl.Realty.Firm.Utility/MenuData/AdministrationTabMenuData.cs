using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Utility.MenuData
{
    public static class AdministrationTabMenuData
    {
        public static List<TabMenu> GetAdministrationTabMenuData()
        {
            List<TabMenu> menus = new List<TabMenu>();
            int count = 0;

            // Start
            TabMenu menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "User",
                Description = "Users",
                Tab = "users-tab",
                Control = "users",
            };

            menus.Add(menu);
            // End

            // Start
            menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "Role",
                Description = "Roles",
                Tab = "roles-tab",
                Control = "roles",
            };

            menus.Add(menu);
            // End

            // Start
            menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "EmploymentStatus",
                Description = "Employment Status",
                Tab = "employmentStatuses-tab",
                Control = "employmentStatuses",
            };

            menus.Add(menu);
            // End

            // Start
            menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "Department",
                Description = "Departments",
                Tab = "departments-tab",
                Control = "departments",
            };

            menus.Add(menu);
            // End
            // Start
            menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "InternalEmail",
                Description = "Internal Emails",
                Tab = "internalEmails-tab",
                Control = "internalEmails",
            };

            menus.Add(menu);
            // End
            return menus;
        }

        public static List<TabMenu> GetEditUserTabMenuData()
        {
            List<TabMenu> menus = new List<TabMenu>();
            int count = 0;
            // Start
            TabMenu menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "EditUser",
                Description = "Edit User",
                Tab = "editUsers-tab",
                Control = "editUsers",
            };

            menus.Add(menu);
            // End

            // Start
            menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "EditRole",
                Description = "EditRole",
                Tab = "editRoles-tab",
                Control = "editRoles",
            };

            menus.Add(menu);
            // End

            return menus;
        }
    }
}
