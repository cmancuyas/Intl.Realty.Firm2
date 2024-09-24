using Intl.Realty.Firm.Model.ViewModel;
using Intl.Realty.Firm.Model.ViewModel.Home;
using Intl.Realty.Firm.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Intl.Realty.Firm.Model.Model;

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
            var saleListingIEnum = await _unitOfWork.SaleListing.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "IRFDeal,TransactionType,DealStatus");
            var saleCoopIEnum = await _unitOfWork.SaleCoop.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "IRFDeal,TransactionType,DealStatus");
            var leaseListingIEnum = await _unitOfWork.LeaseListing.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "IRFDeal,TransactionType,DealStatus");
            var leaseCoopIEnum = await _unitOfWork.LeaseCoop.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "IRFDeal,TransactionType,DealStatus");

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
                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    viewModel.SaleListingListToday.Add(item);
                }
                if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    viewModel.SaleListingListPastHour.Add(item);
                }

                bool isCurrentMonth = DateTime.Now.Month == item.CreatedAt.Month
                                    && DateTime.Now.Year == item.CreatedAt.Year;
                if (isCurrentMonth)
                {
                    viewModel.SaleListingListMonth.Add(item);
                }
            }



            // SaleCoop
            saleCoopIEnum = saleCoopIEnum.OrderByDescending(x => x.CreatedAt);
            viewModel.SaleCoopListAll = saleCoopIEnum.ToList();

            foreach (var item in saleCoopIEnum)
            {
                var timeNow = DateTime.Now;
                var pastHour = DateTime.Now.AddHours(-1);
                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    viewModel.SaleCoopListToday.Add(item);
                }
                if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    viewModel.SaleCoopListPastHour.Add(item);
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
                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    viewModel.LeaseListingListToday.Add(item);
                }
                if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    viewModel.LeaseListingListPastHour.Add(item);
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
                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    viewModel.LeaseCoopListToday.Add(item);
                }
                if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    viewModel.LeaseCoopListPastHour.Add(item);
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
        public async Task<IActionResult> AdminDashboard()
        {
            DashboardViewModel viewModel = new DashboardViewModel();
            var saleListingIEnum = await _unitOfWork.SaleListing.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "IRFDeal,TransactionType,DealStatus");
            var saleCoopIEnum = await _unitOfWork.SaleCoop.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "IRFDeal,TransactionType,DealStatus");
            var leaseListingIEnum = await _unitOfWork.LeaseListing.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "IRFDeal,TransactionType,DealStatus");
            var leaseCoopIEnum = await _unitOfWork.LeaseCoop.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "IRFDeal,TransactionType,DealStatus");

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
                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    viewModel.SaleListingListToday.Add(item);
                }
                if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    viewModel.SaleListingListPastHour.Add(item);
                }

                bool isCurrentMonth = DateTime.Now.Month == item.CreatedAt.Month
                                    && DateTime.Now.Year == item.CreatedAt.Year;
                if (isCurrentMonth)
                {
                    viewModel.SaleListingListMonth.Add(item);
                }
            }



            // SaleCoop
            saleCoopIEnum = saleCoopIEnum.OrderByDescending(x => x.CreatedAt);
            viewModel.SaleCoopListAll = saleCoopIEnum.ToList();

            foreach (var item in saleCoopIEnum)
            {
                var timeNow = DateTime.Now;
                var pastHour = DateTime.Now.AddHours(-1);
                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    viewModel.SaleCoopListToday.Add(item);
                }
                if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    viewModel.SaleCoopListPastHour.Add(item);
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
                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    viewModel.LeaseListingListToday.Add(item);
                }
                if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    viewModel.LeaseListingListPastHour.Add(item);
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
                if (item.CreatedAt > DateTime.Now.AddHours(-24) && item.CreatedAt <= DateTime.Now)
                {
                    viewModel.LeaseCoopListToday.Add(item);
                }
                if (pastHour <= timeNow && item.CreatedAt >= pastHour)
                {
                    viewModel.LeaseCoopListPastHour.Add(item);
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
