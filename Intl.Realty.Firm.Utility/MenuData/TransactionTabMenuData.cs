using Intl.Realty.Firm.Model.Menu;

namespace Intl.Realty.Firm.Utility.MenuData
{
    public static class TransactionTabMenuData
    {
        public static List<TabMenu> GetTransactionTabMenuData()
        {
            List<TabMenu> menus = new List<TabMenu>();
            int count = 0;

            // Start
            TabMenu menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "TransactionType",
                Description = "Transaction Type",
                Tab = "transactionTypes-tab",
                Control = "transactionTypes",
            };

            menus.Add(menu);
            // End

            // Start
            menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "DocumentType",
                Description = "Document Type",
                Tab = "documentTypes-tab",
                Control = "documentTypes",
            };

            menus.Add(menu);
            // End

            // Start
            menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "DealStatus",
                Description = "Deal Status",
                Tab = "dealStatuses-tab",
                Control = "dealStatuses",
            };

            menus.Add(menu);
            // End

            // Start
            menu = new TabMenu()
            {
                Id = count + 1,
                Menu = "DocumentTypeAssignment",
                Description = "Document Type Assignment",
                Tab = "documentTypeAssignments-tab",
                Control = "documentTypeAssignments",
            };

            menus.Add(menu);
            // End

            return menus;
        }
    }
}
