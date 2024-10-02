using Intl.Realty.Firm.Model.ViewModel.Home;

namespace Intl.Realty.Firm.Service.IService
{
    public interface IAllDealsService
    {
        Task<DashboardViewModel> GetAllDealsForDashboardData(string sortOrder, string searchString, int pageNumber = 1, int pageSize = 10);
    }
}