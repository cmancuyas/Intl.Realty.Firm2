using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.AllDealsVM;
using Intl.Realty.Firm.Model.ViewModel.Home;
using Intl.Realty.Firm.Service;
using Microsoft.AspNetCore.Mvc;

namespace Intl.Realty.Firm.Models
{
    public class DashboardModel
    {
        private readonly AllDealsService _allDealsService;

        [BindProperty]
        public DashboardViewModel DashboardViewModel { get; set; }

        public DashboardModel(AllDealsService allDealsService)
        {
            _allDealsService = allDealsService;
        }

        public async Task OnGetAsync(int? pageIndex)
        {
            if (DashboardViewModel == null)
            {
                DashboardViewModel = new DashboardViewModel();
            }

            int pageNumber = pageIndex ?? 1;

            DashboardViewModel = await _allDealsService.GetAllDealsForDashboardData(
                pageNumber,
                DashboardViewModel.AllDealsPageSize,
                DashboardViewModel.AllDealsSearchString,
                DashboardViewModel.AllDealsCurrentSort
            );
        }

    }
}
