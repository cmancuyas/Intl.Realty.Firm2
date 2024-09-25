using Intl.Realty.Firm.Model.ViewModel;
using Intl.Realty.Firm.Model.ViewModel.Home;
using Intl.Realty.Firm.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Models;

namespace Intl.Realty.Firm.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger,
                              IUnitOfWork unitOfWork,
                              UserManager<ApplicationUser> userManager
                                )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Dashboard()
        {
            DashboardViewModel viewModel = new DashboardViewModel();
            var saleListingIEnum = await _unitOfWork.SaleListing
                                        .GetAllAsync(
                                            x => x.IsDeleted == false, 
                                            includeProperties: "IRFDeal,TransactionType,DealStatus"
                                            );

            foreach (var item in saleListingIEnum)
            {
                var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(x => x.SaleListingId == item.Id);
                var fileUploadList = fileUploads.ToList();
                foreach(var fileUpload in fileUploads)
                {
                    item.FileUploads!.Add(fileUpload);
                }
            }

            var saleCoopIEnum = await _unitOfWork.SaleCoop.GetAllAsync(x => x.IsDeleted == false, includeProperties: "IRFDeal,TransactionType,DealStatus");
            foreach (var item in saleCoopIEnum)
            {
                var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(x => x.SaleCoopId == item.Id);
                foreach (var fileUpload in fileUploads)
                {
                    item.FileUploads!.Add(fileUpload);
                }
            }

            var leaseListingIEnum = await _unitOfWork.LeaseListing.GetAllAsync(x => x.IsDeleted == false, includeProperties: "IRFDeal,TransactionType,DealStatus");
            foreach (var item in leaseListingIEnum)
            {
                var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(x => x.LeaseListingId == item.Id);
                foreach (var fileUpload in fileUploads)
                {
                    item.FileUploads!.Add(fileUpload);
                }
            }
            var leaseCoopIEnum = await _unitOfWork.LeaseCoop.GetAllAsync(x => x.IsDeleted == false, includeProperties: "IRFDeal,TransactionType,DealStatus");
            foreach (var item in leaseCoopIEnum)
            {
                var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(x => x.LeaseCoopId == item.Id);
                foreach (var fileUpload in fileUploads)
                {
                    item.FileUploads!.Add(fileUpload);
                }
            }

            var user = await _userManager.GetUserAsync(User);

            saleListingIEnum = saleListingIEnum.Where(x => x.CreatedBy == user!.Id);
            saleCoopIEnum = saleCoopIEnum.Where(x => x.CreatedBy == user!.Id);
            leaseListingIEnum = leaseListingIEnum.Where(x => x.CreatedBy == user!.Id);
            leaseCoopIEnum = leaseCoopIEnum.Where(x => x.CreatedBy == user!.Id);

            // SaleListing
            saleListingIEnum = saleListingIEnum.OrderByDescending(x => x.CreatedAt);
            viewModel.SaleListingListAll = saleListingIEnum.ToList();
            foreach (var item in saleListingIEnum)
            {
                var timeNow = DateTime.Now;
                var pastHour = DateTime.Now.AddHours(-1);
                var past5Mins = DateTime.Now.AddMinutes(-5);

                if (past5Mins <= timeNow && item.CreatedAt >= past5Mins)
                {
                    viewModel.SaleListingListPast5Mins!.Add(item);
                }

                else if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    viewModel.SaleListingListPastHour!.Add(item);
                }
                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    viewModel.SaleListingListToday!.Add(item);
                }
                bool isCurrentMonth = DateTime.Now.Month == item.CreatedAt.Month
                                    && DateTime.Now.Year == item.CreatedAt.Year;
                if (isCurrentMonth)
                {
                    viewModel.SaleListingListMonth!.Add(item);
                }
            }

            // SaleCoop
            saleCoopIEnum = saleCoopIEnum.OrderByDescending(x => x.CreatedAt);
            viewModel.SaleCoopListAll = saleCoopIEnum.ToList();

            foreach (var item in saleCoopIEnum)
            {
                var timeNow = DateTime.Now;
                var pastHour = DateTime.Now.AddHours(-1);
                var past5Mins = DateTime.Now.AddMinutes(-5);

                if (past5Mins <= timeNow && item.CreatedAt >= past5Mins)
                {
                    viewModel.SaleCoopListPast5Mins.Add(item);
                }

                else if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    viewModel.SaleCoopListPastHour.Add(item);
                }

                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    viewModel.SaleCoopListToday.Add(item);
                }
                bool isCurrentMonth = DateTime.Now.Month == item.CreatedAt.Month
                                    && DateTime.Now.Year == item.CreatedAt.Year;
                if (isCurrentMonth)
                {
                    viewModel.SaleCoopListMonth.Add(item);
                }
            }

            //Lease Listing
            leaseListingIEnum = leaseListingIEnum.OrderByDescending(x => x.CreatedAt);
            viewModel.LeaseListingListAll = leaseListingIEnum.ToList();

            foreach (var item in leaseListingIEnum)
            {
                var timeNow = DateTime.Now;
                var pastHour = DateTime.Now.AddHours(-1);
                var past5Mins = DateTime.Now.AddMinutes(-5);

                if (past5Mins <= timeNow && item.CreatedAt >= past5Mins)
                {
                    viewModel.LeaseListingListPast5Mins.Add(item);
                }
                else if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    viewModel.LeaseListingListPastHour.Add(item);
                }
                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    viewModel.LeaseListingListToday.Add(item);
                }


                bool isCurrentMonth = DateTime.Now.Month == item.CreatedAt.Month
                                    && DateTime.Now.Year == item.CreatedAt.Year;
                if (isCurrentMonth)
                {
                    viewModel.LeaseListingListMonth.Add(item);
                }
            }

            //Lease Coop
            leaseCoopIEnum = leaseCoopIEnum.OrderByDescending(x => x.CreatedAt);
            viewModel.LeaseCoopListAll = leaseCoopIEnum.ToList();

            foreach (var item in leaseCoopIEnum)
            {
                var timeNow = DateTime.Now;
                var pastHour = DateTime.Now.AddHours(-1);
                var past5Mins = DateTime.Now.AddMinutes(-5);

                if (past5Mins <= timeNow && item.CreatedAt >= past5Mins)
                {
                    viewModel.LeaseCoopListPast5Mins.Add(item);
                }
                else if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    viewModel.LeaseCoopListPastHour.Add(item);
                }
                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    viewModel.LeaseCoopListToday.Add(item);
                }

                bool isCurrentMonth = DateTime.Now.Month == item.CreatedAt.Month
                                    && DateTime.Now.Year == item.CreatedAt.Year;
                if (isCurrentMonth)
                {
                    viewModel.LeaseCoopListMonth.Add(item);
                }
            }

            viewModel.SaleListingCountToday = viewModel.SaleListingListToday!.Count();
            viewModel.SaleCoopCountToday = viewModel.SaleCoopListToday.Count();
            viewModel.LeaseListingCountToday = viewModel.LeaseListingListToday.Count();
            viewModel.LeaseCoopCountToday = viewModel.LeaseCoopListToday.Count();

            return View(viewModel);
        }
        public async Task<IActionResult> AdminDashboard(string searchString, string sortOrder, int pageNumber, string currentFilter)
        {
            DashboardViewModel viewModel = new DashboardViewModel();
            // Get Data
            var saleListingIEnum = await _unitOfWork.SaleListing
                .GetAllAsync(
                    x => x.IsDeleted == false,
                    includeProperties: "IRFDeal,TransactionType,DealStatus"
                    );

            foreach (var item in saleListingIEnum)
            {
                var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(x => x.SaleListingId == item.Id);
                foreach (var fileUpload in fileUploads)
                {
                    item.FileUploads!.Add(fileUpload);
                }
            }

            var saleCoopIEnum = await _unitOfWork.SaleCoop.GetAllAsync(x => x.IsDeleted == false, includeProperties: "IRFDeal,TransactionType,DealStatus");
            foreach (var item in saleCoopIEnum)
            {
                var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(x => x.SaleCoopId == item.Id);
                foreach (var fileUpload in fileUploads)
                {
                    item.FileUploads!.Add(fileUpload);
                }
            }

            var leaseListingIEnum = await _unitOfWork.LeaseListing.GetAllAsync(x => x.IsDeleted == false, includeProperties: "IRFDeal,TransactionType,DealStatus");
            foreach (var item in leaseListingIEnum)
            {
                var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(x => x.LeaseListingId == item.Id);
                foreach (var fileUpload in fileUploads)
                {
                    item.FileUploads!.Add(fileUpload);
                }
            }
            var leaseCoopIEnum = await _unitOfWork.LeaseCoop.GetAllAsync(x => x.IsDeleted == false, includeProperties: "IRFDeal,TransactionType,DealStatus");
            foreach (var item in leaseCoopIEnum)
            {
                var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(x => x.LeaseCoopId == item.Id);
                foreach (var fileUpload in fileUploads)
                {
                    item.FileUploads!.Add(fileUpload);
                }
            }

            // SaleListing
            saleListingIEnum = saleListingIEnum.OrderByDescending(x => x.CreatedAt);
            viewModel.SaleListingListAll = saleListingIEnum.ToList();

            foreach (var item in saleListingIEnum)
            {
                var timeNow = DateTime.Now;
                var pastHour = DateTime.Now.AddHours(-1);
                var past5Mins = DateTime.Now.AddMinutes(-5);

                if (past5Mins <= timeNow && item.CreatedAt >= past5Mins)
                {
                    viewModel.SaleListingListPast5Mins!.Add(item);
                }

                else if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    viewModel.SaleListingListPastHour!.Add(item);
                }
                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    viewModel.SaleListingListToday!.Add(item);
                }
                bool isCurrentMonth = DateTime.Now.Month == item.CreatedAt.Month
                                    && DateTime.Now.Year == item.CreatedAt.Year;
                if (isCurrentMonth)
                {
                    viewModel.SaleListingListMonth!.Add(item);
                }
            }
            // Add Pagination in SaleListingListPerMonth
            if(searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            viewModel.MonthlyCurrentFilter = searchString;
            var saleListingListMonth = viewModel.SaleListingListMonth;

            // SaleCoop
            saleCoopIEnum = saleCoopIEnum.OrderByDescending(x => x.CreatedAt);
            viewModel.SaleCoopListAll = saleCoopIEnum.ToList();

            foreach (var item in saleCoopIEnum)
            {
                var timeNow = DateTime.Now;
                var pastHour = DateTime.Now.AddHours(-1);
                var past5Mins = DateTime.Now.AddMinutes(-5);

                if (past5Mins <= timeNow && item.CreatedAt >= past5Mins)
                {
                    viewModel.SaleCoopListPast5Mins.Add(item);
                }
                else if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    viewModel.SaleCoopListPastHour.Add(item);
                }
                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    viewModel.SaleCoopListToday.Add(item);
                }


                bool isCurrentMonth = DateTime.Now.Month == item.CreatedAt.Month
                                    && DateTime.Now.Year == item.CreatedAt.Year;
                if (isCurrentMonth)
                {
                    viewModel.SaleCoopListMonth.Add(item);
                }
            }

            //Lease Listing
            leaseListingIEnum = leaseListingIEnum.OrderByDescending(x => x.CreatedAt);
            viewModel.LeaseListingListAll = leaseListingIEnum.ToList();

            foreach (var item in leaseListingIEnum)
            {
                var timeNow = DateTime.Now;
                var pastHour = DateTime.Now.AddHours(-1);
                var past5Mins = DateTime.Now.AddMinutes(-5);

                if (past5Mins <= timeNow && item.CreatedAt >= past5Mins)
                {
                    viewModel.LeaseListingListPast5Mins.Add(item);
                }
                else if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    viewModel.LeaseListingListPastHour.Add(item);
                }

                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    viewModel.LeaseListingListToday.Add(item);
                }

                bool isCurrentMonth = DateTime.Now.Month == item.CreatedAt.Month
                                    && DateTime.Now.Year == item.CreatedAt.Year;
                if (isCurrentMonth)
                {
                    viewModel.LeaseListingListMonth.Add(item);
                }
            }

            //Lease Coop
            leaseCoopIEnum = leaseCoopIEnum.OrderByDescending(x => x.CreatedAt);
            viewModel.LeaseCoopListAll = leaseCoopIEnum.ToList();

            foreach (var item in leaseCoopIEnum)
            {
                var timeNow = DateTime.Now;
                var pastHour = DateTime.Now.AddHours(-1);
                var past5Mins = DateTime.Now.AddMinutes(-5);

                if (past5Mins <= timeNow && item.CreatedAt >= past5Mins)
                {
                    viewModel.LeaseCoopListPast5Mins.Add(item);
                }
                else if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    viewModel.LeaseCoopListPastHour.Add(item);
                }
                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    viewModel.LeaseCoopListToday.Add(item);
                }


                bool isCurrentMonth = DateTime.Now.Month == item.CreatedAt.Month
                                    && DateTime.Now.Year == item.CreatedAt.Year;
                if (isCurrentMonth)
                {
                    viewModel.LeaseCoopListMonth.Add(item);
                }
            }

            viewModel.SaleListingCountToday = viewModel.SaleListingListToday.Count();
            viewModel.SaleCoopCountToday = viewModel.SaleCoopListToday.Count();
            viewModel.LeaseListingCountToday = viewModel.LeaseListingListToday.Count();
            viewModel.LeaseCoopCountToday = viewModel.LeaseCoopListToday.Count();


            return View(viewModel);

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
