using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.Pagination;
using Intl.Realty.Firm.Model.ViewModel.AllDealsVM;
using Intl.Realty.Firm.Models.Pagination;

namespace Intl.Realty.Firm.Service.IService
{
    public interface IAllDealsService
    {
        Task<PaginatedDealsViewModel> GetAllDealsForDashboardData(ApplicationUser user, string sortProperty, SortOrder sortOrder = SortOrder.Descending, string searchText = "", int pageIndex = 1, int pageSize = 5, bool getCurrentMonthFlag = true);
    }
}