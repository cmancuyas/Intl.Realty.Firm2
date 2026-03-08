using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Utility.MenuData
{
    public static class CreateDealTransactionTabMenuData
    {
        public static List<TabMenu> GetCreateDealTransactionTabMenuData()
        {
            List<TabMenu> menus = new List<TabMenu>();
            int count = 0;

            // Start
            TabMenu menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "IRFDetails1",
                Description = "IRF Details 1",
                Tab = "irfDetails1-tab",
                Control = "irfDetails1",
            };

            menus.Add(menu);
            // End

            // Start
            menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "IRFDetails2",
                Description = "IRF Details 2",
                Tab = "irfDetails2-tab",
                Control = "irfDetails2",
            };

            menus.Add(menu);
            // End

            // Start
            menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "FileUpload",
                Description = "File Uploads",
                Tab = "fileUploads-tab",
                Control = "fileUploads",
            };

            menus.Add(menu);
            // End

            // Start
            menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "Notes",
                Description = "Notes",
                Tab = "notes-tab",
                Control = "notes",
            };

            menus.Add(menu);
            // End

            return menus;
        }
    }
}
