using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Utility.MenuData
{
    public static class EditSaleListingTabMenuData
    {
        public static List<TabMenu> GetEditSaleListingTabMenuData()
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
                Menu = "ViewFileUpload",
                Description = "View File Uploads",
                Tab = "viewFileUploads-tab",
                Control = "viewFileUploads",
            };

            menus.Add(menu);
            // End

            // Start
            menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "CreateFileUpload",
                Description = "Create File Uploads",
                Tab = "createFileUploads-tab",
                Control = "createFileUploads",
            };

            menus.Add(menu);
            // End

            return menus;
        }
    }
}
