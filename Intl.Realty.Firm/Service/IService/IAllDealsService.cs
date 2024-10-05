using Intl.Realty.Firm.Model.ViewModel.Home;

namespace Intl.Realty.Firm.Service.IService
{
    public interface IAllDealsService
    {
        Task<DashboardViewModel> GetAllDealsForDashboardData(int pageIndex = 1, int pageSize = 10, string sortColumn = "FinalClosingDate", string sortOrder = "desc", string searchQuery = "");
    }
}