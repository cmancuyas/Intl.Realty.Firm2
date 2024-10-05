using Intl.Realty.Firm.Model.Pagination;
using Intl.Realty.Firm.Model.ViewModel.AllDealsVM;

namespace Intl.Realty.Firm.Service.IService
{
    public interface IAllDealsService
    {
        Task<PaginatedList<AllDealViewModel>> GetAllDealsForDashboardData(string sortProperty, SortOrder sortOrder = SortOrder.Descending, string searchText = "", int pageIndex = 1, int pageSize = 5, bool getCurrentMonthFlag = true);
    }
}