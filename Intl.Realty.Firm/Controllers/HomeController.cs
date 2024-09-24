using Intl.Realty.Firm.Model.ViewModel;
using Intl.Realty.Firm.Model.ViewModel.Home;
using Intl.Realty.Firm.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;
using System.Diagnostics;

namespace Intl.Realty.Firm.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,
                              IUnitOfWork unitOfWork
                                )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
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

            viewModel.SaleListingCountToday = saleListingIEnum.Count();
            viewModel.SaleCoopCountToday = saleCoopIEnum.Count();
            viewModel.LeaseListingCountToday = leaseListingIEnum.Count();
            viewModel.LeaseCoopCountToday = leaseCoopIEnum.Count();

            // SaleListing
            saleListingIEnum = saleListingIEnum.OrderByDescending(x => x.CreatedAt);
            viewModel.SaleListingListAll = saleListingIEnum.ToList();
            viewModel.SaleListingListPastHour = saleListingIEnum.Where(x=>x.CreatedAt <= DateTime.Now.AddHours(-1)).ToList();


            viewModel.SaleListingListToday = saleListingIEnum.Where(x=>x.CreatedAt == DateTime.Today).ToList();
            
            foreach (var item in saleListingIEnum)
            {
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
            viewModel.SaleCoopListPastHour = saleCoopIEnum.Where(x => x.CreatedAt <= DateTime.Now.AddHours(-1)).ToList();


            viewModel.SaleCoopListToday = saleCoopIEnum.Where(x => x.CreatedAt == DateTime.Today).ToList();

            foreach (var item in saleCoopIEnum)
            {
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
            viewModel.LeaseListingListPastHour = leaseListingIEnum.Where(x => x.CreatedAt <= DateTime.Now.AddHours(-1)).ToList();


            viewModel.LeaseListingListToday = leaseListingIEnum.Where(x => x.CreatedAt == DateTime.Today).ToList();

            foreach (var item in leaseListingIEnum)
            {
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
            viewModel.LeaseCoopListPastHour = leaseCoopIEnum.Where(x => x.CreatedAt <= DateTime.Now.AddHours(-1)).ToList();


            viewModel.LeaseCoopListToday = leaseCoopIEnum.Where(x => x.CreatedAt == DateTime.Today).ToList();

            foreach (var item in leaseCoopIEnum)
            {
                bool isCurrentMonth = DateTime.Now.Month == item.CreatedAt.Month
                                    && DateTime.Now.Year == item.CreatedAt.Year;
                if (isCurrentMonth)
                {
                    viewModel.LeaseCoopListMonth.Add(item);
                }
            }

            return View(viewModel);
        }
        public async Task<IActionResult> AdminDashboard()
        {
            DashboardViewModel viewModel = new DashboardViewModel();
            var saleListingIEnum = await _unitOfWork.SaleListing.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "IRFDeal,TransactionType,DealStatus");
            var saleCoopIEnum = await _unitOfWork.SaleCoop.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "IRFDeal,TransactionType,DealStatus");
            var leaseListingIEnum = await _unitOfWork.LeaseListing.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "IRFDeal,TransactionType,DealStatus");
            var leaseCoopIEnum = await _unitOfWork.LeaseCoop.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "IRFDeal,TransactionType,DealStatus");

            viewModel.SaleListingCountToday = saleListingIEnum.Count();
            viewModel.SaleCoopCountToday = saleCoopIEnum.Count();
            viewModel.LeaseListingCountToday = leaseListingIEnum.Count();
            viewModel.LeaseCoopCountToday = leaseCoopIEnum.Count();

            // SaleListing
            saleListingIEnum = saleListingIEnum.OrderByDescending(x => x.CreatedAt);
            viewModel.SaleListingListAll = saleListingIEnum.ToList();
            viewModel.SaleListingListPastHour = saleListingIEnum.Where(x => x.CreatedAt <= DateTime.Now.AddHours(-1)).ToList();


            viewModel.SaleListingListToday = saleListingIEnum.Where(x => x.CreatedAt == DateTime.Today).ToList();

            foreach (var item in saleListingIEnum)
            {
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
            viewModel.SaleCoopListPastHour = saleCoopIEnum.Where(x => x.CreatedAt <= DateTime.Now.AddHours(-1)).ToList();


            viewModel.SaleCoopListToday = saleCoopIEnum.Where(x => x.CreatedAt == DateTime.Today).ToList();

            foreach (var item in saleCoopIEnum)
            {
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
            viewModel.LeaseListingListPastHour = leaseListingIEnum.Where(x => x.CreatedAt <= DateTime.Now.AddHours(-1)).ToList();


            viewModel.LeaseListingListToday = leaseListingIEnum.Where(x => x.CreatedAt == DateTime.Today).ToList();

            foreach (var item in leaseListingIEnum)
            {
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
            viewModel.LeaseCoopListPastHour = leaseCoopIEnum.Where(x => x.CreatedAt <= DateTime.Now.AddHours(-1)).ToList();


            viewModel.LeaseCoopListToday = leaseCoopIEnum.Where(x => x.CreatedAt == DateTime.Today).ToList();

            foreach (var item in leaseCoopIEnum)
            {
                bool isCurrentMonth = DateTime.Now.Month == item.CreatedAt.Month
                                    && DateTime.Now.Year == item.CreatedAt.Year;
                if (isCurrentMonth)
                {
                    viewModel.LeaseCoopListMonth.Add(item);
                }
            }


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
