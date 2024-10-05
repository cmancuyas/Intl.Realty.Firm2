using Intl.Realty.Firm.Model.ViewModel;
using Intl.Realty.Firm.Model.ViewModel.Home;
using Intl.Realty.Firm.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.AllDealsVM;
using Intl.Realty.Firm.Utility.Mapper;
using Intl.Realty.Firm.Service.IService;
using Intl.Realty.Firm.Utilities;

namespace Intl.Realty.Firm.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAllDealsService _allDealsService;

        public HomeController(ILogger<HomeController> logger,
                              IUnitOfWork unitOfWork,
                              UserManager<ApplicationUser> userManager,
                              IAllDealsService allDealsService
                                )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _allDealsService = allDealsService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Dashboard(int pageIndex = 1, int pageSize = 10, string sortColumn = "FinalClosingDate", string sortOrder = "desc", string searchQuery = "")
        {
            DashboardViewModel viewModel = await GetDashboardData();

            // Get the dashboard data using the unified DashboardModel
            var dashboardViewModel = await _allDealsService.GetAllDealsForDashboardData(
                pageIndex, pageSize, sortColumn, sortOrder, searchQuery
            );

            viewModel.AllDealsTotalCount = dashboardViewModel.AllDealsTotalCount;
            viewModel.AllDeals = dashboardViewModel.AllDeals;
            viewModel.SortOrder = sortOrder;
            viewModel.SearchQuery = searchQuery;
            viewModel.PageSize = pageSize;
            viewModel.CurrentPage = pageIndex;
            viewModel.SortColumn = sortColumn;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_DashboardPartial", viewModel);
            }

            return View(viewModel);
        }
        public async Task<IActionResult> AdminDashboard()
        {
            DashboardViewModel viewModel = await GetDashboardData();

            return View(viewModel);
        }

        private async Task<DashboardViewModel> GetDashboardData()
        {
            var user = await _userManager.GetUserAsync(User);

            var isAgent = await _userManager.IsInRoleAsync(user!, "Agent");
            var isDealAdmin = await _userManager.IsInRoleAsync(user!, "DealAdmin");
            var isAdmin = await _userManager.IsInRoleAsync(user!, "Admin");
            var isSuperAdmin = await _userManager.IsInRoleAsync(user!, "SuperAdmin");

            DashboardViewModel viewModel = new DashboardViewModel();

            var saleListingIEnum = await GetSaleListingData();
            var saleCoopIEnum = await GetSaleCoopData();
            var leaseListingIEnum = await GetLeaseListingData();
            var leaseCoopIEnum = await GetLeaseCoopData();

            if (isAgent)
            {
                saleListingIEnum = saleListingIEnum.Where(x => x.CreatedBy == user!.Id);
                saleCoopIEnum = saleCoopIEnum.Where(x => x.CreatedBy == user!.Id);
                leaseListingIEnum = leaseListingIEnum.Where(x => x.CreatedBy == user!.Id);
                leaseCoopIEnum = leaseCoopIEnum.Where(x => x.CreatedBy == user!.Id);
            }

            saleListingIEnum = saleListingIEnum.OrderByDescending(x => x.CreatedAt);
            saleCoopIEnum = saleCoopIEnum.OrderByDescending(x => x.CreatedAt);
            leaseListingIEnum = leaseListingIEnum.OrderByDescending(x => x.CreatedAt);
            leaseCoopIEnum = leaseCoopIEnum.OrderByDescending(x => x.CreatedAt);

            viewModel = GetAllDealsPerSaleListing(saleListingIEnum, viewModel);
            viewModel = GetAllDealsPerSaleCoop(saleCoopIEnum, viewModel);
            viewModel = GetAllDealsPerLeaseListing(leaseListingIEnum, viewModel);
            viewModel = GetAllDealsPerLeaseCoop(leaseCoopIEnum, viewModel);

            viewModel.SaleListingCountToday = viewModel.SaleListingListToday!.Count();
            viewModel.SaleCoopCountToday = viewModel.SaleCoopListToday.Count();
            viewModel.LeaseListingCountToday = viewModel.LeaseListingListToday.Count();
            viewModel.LeaseCoopCountToday = viewModel.LeaseCoopListToday.Count();

            var allDealsPast1Min = await GetAllDealsFor(viewModel.SaleListingListPast1Min,
                                                 viewModel.SaleCoopListPast1Min,
                                                 viewModel.LeaseCoopListPast1Min,
                                                 viewModel.LeaseListingListPast1Min);

            var allDealsPast5Mins = await GetAllDealsFor(viewModel.SaleListingListPast5Mins,
                                                 viewModel.SaleCoopListPast5Mins,
                                                 viewModel.LeaseCoopListPast5Mins,
                                                 viewModel.LeaseListingListPast5Mins);

            var allDealsPastHour = await GetAllDealsFor(viewModel.SaleListingListPastHour,
                                                 viewModel.SaleCoopListPastHour,
                                                 viewModel.LeaseCoopListPastHour,
                                                 viewModel.LeaseListingListPastHour);

            var allDealsToday = await GetAllDealsFor(viewModel.SaleListingListToday,
                                                 viewModel.SaleCoopListToday,
                                                 viewModel.LeaseCoopListToday,
                                                 viewModel.LeaseListingListToday);

            allDealsPast1Min = allDealsPast1Min.OrderByDescending(x => x.CreatedAt).ToList();
            allDealsPast5Mins = allDealsPast5Mins.OrderByDescending(x => x.CreatedAt).ToList();
            allDealsPastHour = allDealsPastHour.OrderByDescending(x => x.CreatedAt).ToList();
            allDealsToday = allDealsToday.OrderByDescending(x => x.CreatedAt).ToList();
            allDealsPastHour = allDealsPastHour.OrderByDescending(x => x.CreatedAt).ToList();

            viewModel.AllDealsPast1Min = allDealsPast1Min;
            viewModel.AllDealsPast5Mins = allDealsPast5Mins;
            viewModel.AllDealsPastHour = allDealsPastHour;
            viewModel.AllDealsToday = allDealsToday;


            if (allDealsToday.Count > 10)
            {
                allDealsToday.RemoveRange(10, allDealsToday.Count - 10);
            }

            viewModel.AllDealsToday = allDealsToday;

            return viewModel;
        }
        private async Task<IEnumerable<SaleListing>> GetSaleListingData()
        {
            var modelList = await _unitOfWork.SaleListing
                            .GetAllAsync(
                                x => x.IsDeleted == false,
                                includeProperties: "IRFDeal,TransactionType,DealStatus",
                                orderBy: x=>x.OrderByDescending(x => x.CreatedAt)
                                );
            foreach (var item in modelList)
            {
                var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(x => x.SaleListingId == item.Id);
                var fileUploadList = fileUploads.ToList();
                foreach (var fileUpload in fileUploads)
                {
                    item.FileUploads!.Add(fileUpload);
                }
            }

            return modelList;
        }
        private async Task<IEnumerable<SaleCoop>> GetSaleCoopData()
        {
            var modelList = await _unitOfWork.SaleCoop
                            .GetAllAsync(
                                x => x.IsDeleted == false,
                                includeProperties: "IRFDeal,TransactionType,DealStatus"
                                );

            foreach (var item in modelList)
            {
                var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(x => x.SaleCoopId == item.Id);
                var fileUploadList = fileUploads.ToList();
                foreach (var fileUpload in fileUploads)
                {
                    item.FileUploads!.Add(fileUpload);
                }
            }

            return modelList;
        }
        private async Task<IEnumerable<LeaseListing>> GetLeaseListingData()
        {
            var modelList = await _unitOfWork.LeaseListing
                            .GetAllAsync(
                                x => x.IsDeleted == false,
                                includeProperties: "IRFDeal,TransactionType,DealStatus"
                                );

            foreach (var item in modelList)
            {
                var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(x => x.LeaseListingId == item.Id);
                var fileUploadList = fileUploads.ToList();
                foreach (var fileUpload in fileUploads)
                {
                    item.FileUploads!.Add(fileUpload);
                }
            }

            return modelList;
        }
        private async Task<IEnumerable<LeaseCoop>> GetLeaseCoopData()
        {
            var modelList = await _unitOfWork.LeaseCoop
                            .GetAllAsync(
                                x => x.IsDeleted == false,
                                includeProperties: "IRFDeal,TransactionType,DealStatus"
                                );

            foreach (var item in modelList)
            {
                var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(x => x.LeaseCoopId == item.Id);
                var fileUploadList = fileUploads.ToList();
                foreach (var fileUpload in fileUploads)
                {
                    item.FileUploads!.Add(fileUpload);
                }
            }

            return modelList;
        }
        private DashboardViewModel GetAllDealsPerSaleListing(IEnumerable<SaleListing> saleListingIEnum, DashboardViewModel dashboardViewModel)
        {
            List<SaleListing> saleListingListPast1Min = new List<SaleListing>();
            List<SaleListing> saleListingListPast5Mins = new List<SaleListing>();
            List<SaleListing> saleListingListPastHour = new List<SaleListing>();
            List<SaleListing> saleListingListToday = new List<SaleListing>();
            List<SaleListing> saleListingListMonth = new List<SaleListing>();

            var saleListingListAll = saleListingIEnum.ToList();
            foreach (var item in saleListingIEnum)
            {
                var timeNow = DateTime.Now;
                var pastHour = DateTime.Now.AddHours(-1);
                var past5Mins = DateTime.Now.AddMinutes(-5);
                var past1Min = DateTime.Now.AddMinutes(-1);

                if (past1Min <= timeNow && item.CreatedAt >= past1Min)
                {
                    saleListingListPast1Min.Add(item);
                }
                else if (past5Mins <= timeNow && item.CreatedAt >= past5Mins)
                {
                    saleListingListPast5Mins.Add(item);
                }

                else if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    saleListingListPastHour.Add(item);
                }
                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    saleListingListToday!.Add(item);
                }
                bool isCurrentMonth = DateTime.Now.Month == item.CreatedAt.Month
                                    && DateTime.Now.Year == item.CreatedAt.Year;
                if (isCurrentMonth)
                {
                    saleListingListMonth!.Add(item);
                }
            }
            dashboardViewModel.SaleListingListPast1Min = saleListingListPast1Min;
            dashboardViewModel.SaleListingListPast5Mins = saleListingListPast5Mins;
            dashboardViewModel.SaleListingListPastHour = saleListingListPastHour;
            dashboardViewModel.SaleListingListToday = saleListingListToday;

            return (dashboardViewModel);
        }
        private DashboardViewModel GetAllDealsPerSaleCoop(IEnumerable<SaleCoop> saleCoopIEnum, DashboardViewModel dashboardViewModel)
        {
            List<SaleCoop> saleCoopListPast1Min = new List<SaleCoop>();
            List<SaleCoop> saleCoopListPast5Mins = new List<SaleCoop>();
            List<SaleCoop> saleCoopListPastHour = new List<SaleCoop>();
            List<SaleCoop> saleCoopListToday = new List<SaleCoop>();
            List<SaleCoop> saleCoopListMonth = new List<SaleCoop>();

            var saleCoopListAll = saleCoopIEnum.ToList();
            foreach (var item in saleCoopIEnum)
            {
                var timeNow = DateTime.Now;
                var pastHour = DateTime.Now.AddHours(-1);
                var past5Mins = DateTime.Now.AddMinutes(-5);
                var past1Min = DateTime.Now.AddMinutes(-1);

                if (past1Min <= timeNow && item.CreatedAt >= past1Min)
                {
                    saleCoopListPast1Min.Add(item);
                }
                else if (past5Mins <= timeNow && item.CreatedAt >= past5Mins)
                {
                    saleCoopListPast5Mins.Add(item);
                }

                else if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    saleCoopListPastHour.Add(item);
                }
                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    saleCoopListToday!.Add(item);
                }
                bool isCurrentMonth = DateTime.Now.Month == item.CreatedAt.Month
                                    && DateTime.Now.Year == item.CreatedAt.Year;
                if (isCurrentMonth)
                {
                    saleCoopListMonth!.Add(item);
                }
            }
            dashboardViewModel.SaleCoopListPast1Min = saleCoopListPast1Min;
            dashboardViewModel.SaleCoopListPast5Mins = saleCoopListPast5Mins;
            dashboardViewModel.SaleCoopListPastHour = saleCoopListPastHour;
            dashboardViewModel.SaleCoopListToday = saleCoopListToday;

            return (dashboardViewModel);
        }
        private DashboardViewModel GetAllDealsPerLeaseListing(IEnumerable<LeaseListing> leaseListingIEnum, DashboardViewModel dashboardViewModel)
        {
            List<LeaseListing> leaseListingListPast1Min = new List<LeaseListing>();
            List<LeaseListing> leaseListingListPast5Mins = new List<LeaseListing>();
            List<LeaseListing> leaseListingListPastHour = new List<LeaseListing>();
            List<LeaseListing> leaseListingListToday = new List<LeaseListing>();
            List<LeaseListing> leaseListingListMonth = new List<LeaseListing>();

            var leaseListingListAll = leaseListingIEnum.ToList();
            foreach (var item in leaseListingIEnum)
            {
                var timeNow = DateTime.Now;
                var pastHour = DateTime.Now.AddHours(-1);
                var past5Mins = DateTime.Now.AddMinutes(-5);
                var past1Min = DateTime.Now.AddMinutes(-1);

                if (past1Min <= timeNow && item.CreatedAt >= past1Min)
                {
                    leaseListingListPast1Min.Add(item);
                }
                else if (past5Mins <= timeNow && item.CreatedAt >= past5Mins)
                {
                    leaseListingListPast5Mins.Add(item);
                }

                else if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    leaseListingListPastHour.Add(item);
                }
                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    leaseListingListToday!.Add(item);
                }
                bool isCurrentMonth = DateTime.Now.Month == item.CreatedAt.Month
                                    && DateTime.Now.Year == item.CreatedAt.Year;
                if (isCurrentMonth)
                {
                    leaseListingListMonth!.Add(item);
                }
            }
            dashboardViewModel.LeaseListingListPast1Min = leaseListingListPast1Min;
            dashboardViewModel.LeaseListingListPast5Mins = leaseListingListPast5Mins;
            dashboardViewModel.LeaseListingListPastHour = leaseListingListPastHour;
            dashboardViewModel.LeaseListingListToday = leaseListingListToday;

            return (dashboardViewModel);
        }
        private DashboardViewModel GetAllDealsPerLeaseCoop(IEnumerable<LeaseCoop> leaseCoopIEnum, DashboardViewModel dashboardViewModel)
        {
            List<LeaseCoop> leaseCoopListPast1Min = new List<LeaseCoop>();
            List<LeaseCoop> leaseCoopListPast5Mins = new List<LeaseCoop>();
            List<LeaseCoop> leaseCoopListPastHour = new List<LeaseCoop>();
            List<LeaseCoop> leaseCoopListToday = new List<LeaseCoop>();
            List<LeaseCoop> leaseCoopListMonth = new List<LeaseCoop>();

            var leaseCoopListAll = leaseCoopIEnum.ToList();
            foreach (var item in leaseCoopIEnum)
            {
                var timeNow = DateTime.Now;
                var pastHour = DateTime.Now.AddHours(-1);
                var past5Mins = DateTime.Now.AddMinutes(-5);
                var past1Min = DateTime.Now.AddMinutes(-1);

                if (past1Min <= timeNow && item.CreatedAt >= past1Min)
                {
                    leaseCoopListPast1Min.Add(item);
                }
                else if (past5Mins <= timeNow && item.CreatedAt >= past5Mins)
                {
                    leaseCoopListPast5Mins.Add(item);
                }

                else if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    leaseCoopListPastHour.Add(item);
                }
                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    leaseCoopListToday!.Add(item);
                }
                bool isCurrentMonth = DateTime.Now.Month == item.CreatedAt.Month
                                    && DateTime.Now.Year == item.CreatedAt.Year;
                if (isCurrentMonth)
                {
                    leaseCoopListMonth!.Add(item);
                }
            }
            dashboardViewModel.LeaseCoopListPast1Min = leaseCoopListPast1Min;
            dashboardViewModel.LeaseCoopListPast5Mins = leaseCoopListPast5Mins;
            dashboardViewModel.LeaseCoopListPastHour = leaseCoopListPastHour;
            dashboardViewModel.LeaseCoopListToday = leaseCoopListToday;

            return (dashboardViewModel);
        }


        private async Task<List<AllDealViewModel>> GetAllDealsFor(
                                                    List<SaleListing> saleListingList,
                                                    List<SaleCoop> saleCoopList,
                                                    List<LeaseCoop> leaseCoopList,
                                                    List<LeaseListing> leaseListingList)
        {
            List<AllDealViewModel> allDealsToday = new List<AllDealViewModel>();

            foreach (var item in saleListingList)
            {
                var createdByUser = await _userManager.FindByIdAsync(item.CreatedBy.ToString()!);
                ApplicationUser updatedByUser = new ApplicationUser();
                ApplicationUser deletedByUser = new ApplicationUser();

                if (item.UpdatedBy != null)
                {
                    updatedByUser = await _userManager.FindByIdAsync(item.UpdatedBy.ToString());
                }
                if (item.DeletedBy != null)
                {
                    deletedByUser = await _userManager.FindByIdAsync(item.DeletedBy.ToString());
                }

                var allDealToday = item.ToAllDealsViewModel(createdByUser!, updatedByUser, deletedByUser);
                allDealsToday.Add(allDealToday);
            }

            foreach (var item in saleCoopList)
            {
                var createdByUser = await _userManager.FindByIdAsync(item.CreatedBy.ToString()!);
                ApplicationUser updatedByUser = new ApplicationUser();
                ApplicationUser deletedByUser = new ApplicationUser();

                if (item.UpdatedBy != null)
                {
                    updatedByUser = await _userManager.FindByIdAsync(item.UpdatedBy.ToString());
                }
                if (item.DeletedBy != null)
                {
                    deletedByUser = await _userManager.FindByIdAsync(item.DeletedBy.ToString());
                }


                var allDealToday = item.ToAllDealsViewModel(createdByUser!, updatedByUser!, deletedByUser!);
                allDealsToday.Add(allDealToday);
            }

            foreach (var item in leaseListingList)
            {
                var createdByUser = await _userManager.FindByIdAsync(item.CreatedBy.ToString()!);
                ApplicationUser updatedByUser = new ApplicationUser();
                ApplicationUser deletedByUser = new ApplicationUser();

                if (item.UpdatedBy != null)
                {
                    updatedByUser = await _userManager.FindByIdAsync(item.UpdatedBy.ToString());
                }
                if (item.DeletedBy != null)
                {
                    deletedByUser = await _userManager.FindByIdAsync(item.DeletedBy.ToString());
                }


                var allDealToday = item.ToAllDealsViewModel(createdByUser!, updatedByUser!, deletedByUser!);
                allDealsToday.Add(allDealToday);
            }

            foreach (var item in leaseCoopList)
            {
                var createdByUser = await _userManager.FindByIdAsync(item.CreatedBy.ToString()!);
                ApplicationUser updatedByUser = new ApplicationUser();
                ApplicationUser deletedByUser = new ApplicationUser();

                if (item.UpdatedBy != null)
                {
                    updatedByUser = await _userManager.FindByIdAsync(item.UpdatedBy.ToString());
                }
                if (item.DeletedBy != null)
                {
                    deletedByUser = await _userManager.FindByIdAsync(item.DeletedBy.ToString());
                }

                var allDealToday = item.ToAllDealsViewModel(createdByUser!, updatedByUser!, deletedByUser!);
                allDealsToday.Add(allDealToday);
            }


            return allDealsToday;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
