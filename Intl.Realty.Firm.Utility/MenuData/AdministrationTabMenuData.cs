using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Utility.MenuData
{
    public static class AdministrationTabMenuData
    {
        private static TabMenu CreateTabMenu(int id, string menu, string description, string tab, string control)
        {
            return new TabMenu
            {
                Id = id,
                Menu = menu,
                Description = description,
                Tab = tab,
                Control = control
            };
        }

        public static IReadOnlyList<TabMenu> GetAdministrationTabMenuData()
        {
            int count = 0;
            var menus = new List<TabMenu>
            {
                CreateTabMenu(++count, "User", "Users", "users-tab", "users"),
                CreateTabMenu(++count, "Role", "Roles", "roles-tab", "roles"),
                CreateTabMenu(++count, "EmploymentStatus", "Employment Status", "employmentStatuses-tab", "employmentStatuses"),
                CreateTabMenu(++count, "Department", "Departments", "departments-tab", "departments"),
                CreateTabMenu(++count, "Province", "Provinces", "provinces-tab", "provinces"),
                CreateTabMenu(++count, "InternalEmail", "Internal Emails", "internalEmails-tab", "internalEmails"),
                CreateTabMenu(++count, "ContactEmail", "Contact Emails", "contactEmails-tab", "contactEmails"),
                CreateTabMenu(++count, "EmailCategory", "Email Categories", "emailCategories-tab", "emailCategories"),
                CreateTabMenu(++count, "AgentReferral", "Agent Referrals", "agentReferrals-tab", "agentReferrals"),
                CreateTabMenu(++count, "EmailStatus", "Email Statuses", "emailStatuses-tab", "emailStatuses")
            };

            return menus.AsReadOnly();
        }

        public static IReadOnlyList<TabMenu> GetEditUserTabMenuData()
        {
            int count = 0;
            var menus = new List<TabMenu>
            {
                CreateTabMenu(++count, "EditUser", "Edit User", "editUsers-tab", "editUsers"),
                CreateTabMenu(++count, "EditRole", "Edit Role", "editRoles-tab", "editRoles")
            };

            return menus.AsReadOnly();
        }
    }
}
