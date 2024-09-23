using Intl.Realty.Firm.Model.ViewModel;
using Intl.Realty.Firm.Model.ViewModel.Home;
using Intl.Realty.Firm.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var saleListings = await _unitOfWork.SaleListing.GetAllAsync(x => x.IsDeleted == false);
            var saleCoop = await _unitOfWork.SaleCoop.GetAllAsync(x => x.IsDeleted == false);
            var LeaseListing = await _unitOfWork.LeaseListing.GetAllAsync(x => x.IsDeleted == false);
            var LeaseCoop = await _unitOfWork.LeaseCoop.GetAllAsync(x => x.IsDeleted == false);

            viewModel.SaleListingCount = saleListings.Count();
            viewModel.SaleCoopCount = saleCoop.Count();
            viewModel.LeaseListingCount = LeaseListing.Count();
            viewModel.LeaseCoopCount = LeaseCoop.Count();

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
