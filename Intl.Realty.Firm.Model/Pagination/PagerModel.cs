using Microsoft.AspNetCore.Mvc.Rendering;
namespace Intl.Realty.Firm.Model.Pagination
{
    public class PagerModel
    {
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public int StartRecord { get; private set; }
        public int EndRecord { get; private set; }

        public string Action { get; set; } = "Dashboard";
        public string? SearchText { get; set; }
        public string? SortExpression { get; set; }

        public PagerModel()
        {
            
        }
        public PagerModel(int totalItems, int currentPage, int pageSize = 5, string action = "Dashboard")
        {
            Action = action;
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;

            int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);

            TotalPages = totalPages;

            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            if (endPage > TotalPages)
            {
                endPage = TotalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            StartRecord = (CurrentPage - 1) * PageSize + 1;

            EndRecord = StartRecord - 1 + PageSize;
            if (EndRecord > TotalItems)
            {
                EndRecord = TotalItems;
            }

            if (TotalItems == 0)
            {
                StartPage = 0;
                StartRecord = 0;
                CurrentPage = 0;
                EndRecord = 0;
            }
            else
            {
                StartPage = startPage;
                EndPage = endPage;
            }
        }

        public List<SelectListItem> GetPageSizes()
        {
            var pageSizes = new List<SelectListItem>();

            for (int lastPage = 5; lastPage <= 50; lastPage += 5)
            {
                if (lastPage == this.PageSize)
                {
                    pageSizes.Add(new SelectListItem(lastPage.ToString(), lastPage.ToString(), true));
                }
                else
                {
                    pageSizes.Add(new SelectListItem(lastPage.ToString(), lastPage.ToString()));
                }
            }
            return pageSizes;
        }
    }
}
