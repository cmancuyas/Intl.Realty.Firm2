using Intl.Realty.Firm.Model.ViewModel.Home;

namespace Intl.Realty.Firm.Service.IService
{
    public interface IAllDealsService
    {
        Task<DashboardViewModel> GetAllDealsForDashboardData(int? pageIndex, int? pageSize, string searchString, string sortOrder);
    }
}