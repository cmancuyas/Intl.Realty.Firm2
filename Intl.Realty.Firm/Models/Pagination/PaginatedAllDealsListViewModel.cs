using Intl.Realty.Firm.Model.Pagination;
using Intl.Realty.Firm.Model.ViewModel.AllDealsVM;

namespace Intl.Realty.Firm.Models.Pagination
{
    public class PaginatedAllDealsListViewModel<T> where T : class
    {
        public PagerModel Pager { get; set; } = new PagerModel();// For Current Month
        public PaginatedList<AllDealViewModel> PaginatedList { get; set; }  // For Current Month 
        public SortModel SortModel { get; set; } // For Current Month
        public string SearchText { get; set; } // For Current Month
    }
}
