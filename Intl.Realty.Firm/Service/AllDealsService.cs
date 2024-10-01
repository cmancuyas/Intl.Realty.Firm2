using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.Pagination;
using Intl.Realty.Firm.Model.ViewModel.AllDealsVM;
using Intl.Realty.Firm.Model.ViewModel.Home;
using Intl.Realty.Firm.Repository.IRepository;
using Intl.Realty.Firm.Service.IService;
using Intl.Realty.Firm.Utility.Mapper;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace Intl.Realty.Firm.Service
{
    public class AllDealsService : IAllDealsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public AllDealsService(IUnitOfWork unitOfWork,
                               UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<DashboardViewModel> GetAllDealsForDashboardData(int? pageIndex, int? pageSize, string searchString, string sortOrder)
        {
            var saleListingQuery = await _unitOfWork.SaleListing.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");
            var saleCoopQuery = await _unitOfWork.SaleCoop.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");
            var leaseListingQuery = await _unitOfWork.LeaseListing.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");
            var leaseCoopQuery = await _unitOfWork.LeaseCoop.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");

            List<AllDealViewModel> result = new List<AllDealViewModel>();

            foreach(var item in saleListingQuery.ToList())
            {
                var createdBy = await _userManager.FindByIdAsync(item.CreatedBy.ToString());
                ApplicationUser updatedBy;
                ApplicationUser deletedBy;
                if (item.UpdatedBy != null)
                {
                    updatedBy = await _userManager.FindByIdAsync(item.UpdatedBy.ToString());
                }
                if (item.DeletedBy != null)
                {
                    deletedBy = await _userManager.FindByIdAsync(item.DeletedBy.ToString());
                }
                
                var allDeal = item.ToAllDealsViewModel(createdBy, updatedBy = new(), deletedBy = new());

                result.Add(allDeal);
            }
            foreach (var item in saleCoopQuery.ToList())
            {
                var createdBy = await _userManager.FindByIdAsync(item.CreatedBy.ToString());
                ApplicationUser updatedBy;
                ApplicationUser deletedBy;
                if (item.UpdatedBy != null)
                {
                    updatedBy = await _userManager.FindByIdAsync(item.UpdatedBy.ToString());
                }
                if (item.DeletedBy != null)
                {
                    deletedBy = await _userManager.FindByIdAsync(item.DeletedBy.ToString());
                }

                var allDeal = item.ToAllDealsViewModel(createdBy, updatedBy = new(), deletedBy = new());

                result.Add(allDeal);
            }
            foreach (var item in leaseListingQuery.ToList())
            {
                var createdBy = await _userManager.FindByIdAsync(item.CreatedBy.ToString());
                ApplicationUser updatedBy;
                ApplicationUser deletedBy;
                if (item.UpdatedBy != null)
                {
                    updatedBy = await _userManager.FindByIdAsync(item.UpdatedBy.ToString());
                }
                if (item.DeletedBy != null)
                {
                    deletedBy = await _userManager.FindByIdAsync(item.DeletedBy.ToString());
                }

                var allDeal = item.ToAllDealsViewModel(createdBy, updatedBy = new(), deletedBy = new());

                result.Add(allDeal);
            }
            foreach (var item in leaseCoopQuery.ToList())
            {
                var createdBy = await _userManager.FindByIdAsync(item.CreatedBy.ToString());
                ApplicationUser updatedBy;
                ApplicationUser deletedBy;
                if (item.UpdatedBy != null)
                {
                    updatedBy = await _userManager.FindByIdAsync(item.UpdatedBy.ToString());
                }
                if (item.DeletedBy != null)
                {
                    deletedBy = await _userManager.FindByIdAsync(item.DeletedBy.ToString());
                }

                var allDeal = item.ToAllDealsViewModel(createdBy, updatedBy = new(), deletedBy = new());

                result.Add(allDeal);
            }

            IQueryable<AllDealViewModel> allDeals = result.AsQueryable();

            DashboardViewModel dashboardViewModel = new DashboardViewModel();

            if (!string.IsNullOrEmpty(searchString))
            {
                allDeals = allDeals.Where(p => p.TransactionTypeName.ToLower().Contains(searchString));
                allDeals = allDeals.Where(p => p.PropertyAddress.ToLower().Contains(searchString));
                allDeals = allDeals.Where(p => p.DealStatusName.ToLower().Contains(searchString));
                allDeals = allDeals.Where(p => p.CreatedByFullName.ToLower().Contains(searchString));
            }
            // Sorting logic based on the sort order
            switch (sortOrder)
            {
                case "createdAt_desc":
                    allDeals = allDeals.OrderByDescending(x => x.CreatedAt);
                    dashboardViewModel.CreatedAtSort = sortOrder;
                    break;
                case "propertyAddress":
                    allDeals = allDeals.OrderBy(x => x.PropertyAddress);
                    dashboardViewModel.PropertyAddressSort = sortOrder;
                    break;
                case "propertyAddress_desc":
                    allDeals = allDeals.OrderByDescending(x => x.PropertyAddress);
                    dashboardViewModel.PropertyAddressSort = sortOrder;
                    break;
                case "dealTransactionNumber":
                    allDeals = allDeals.OrderBy(x => x.DealTransactionNumber);
                    dashboardViewModel.DealTransactionNumberSort = sortOrder;
                    break;
                case "dealTransactionNumber_desc":
                    allDeals = allDeals.OrderByDescending(x => x.DealTransactionNumber);
                    dashboardViewModel.DealTransactionNumberSort = sortOrder;
                    break;
                case "transactionType":
                    allDeals = allDeals.OrderBy(x => x.TransactionTypeName);
                    dashboardViewModel.TransactionTypeNameSort = sortOrder;
                    break;
                case "transactionType_desc":
                    allDeals = allDeals.OrderByDescending(x => x.TransactionTypeName);
                    dashboardViewModel.TransactionTypeNameSort = sortOrder;
                    break;
                case "dealStatus":
                    allDeals = allDeals.OrderBy(x => x.DealStatusName);
                    dashboardViewModel.DealStatusNameSort = sortOrder;
                    break;
                case "dealStatus_desc":
                    allDeals = allDeals.OrderByDescending(x => x.DealStatusName);
                    dashboardViewModel.DealStatusNameSort = sortOrder;
                    break;
                case "createdByFullName":
                    allDeals = allDeals.OrderBy(x => x.CreatedByFullName);
                    dashboardViewModel.CreatedAtSort = sortOrder;
                    break;
                case "createdByFullName_desc":
                    allDeals = allDeals.OrderByDescending(x => x.CreatedByFullName);
                    dashboardViewModel.CreatedByFullNameSort = sortOrder;
                    break;
                case "finalClosingDate":
                    allDeals = allDeals.OrderBy(x => x.FinalClosingDate);
                    dashboardViewModel.FinalClosingDateSort = sortOrder;
                    break;
                case "finalClosingDate_desc":
                    allDeals = allDeals.OrderByDescending(x => x.FinalClosingDate);
                    dashboardViewModel.FinalClosingDateSort = sortOrder;
                    break;
                default:
                    allDeals = allDeals.OrderBy(x => x.CreatedAt);
                    dashboardViewModel.CreatedAtSort = sortOrder;
                    break;
            }

            var totalDeals = allDeals.Count();

            var allDealsForDashboard = PaginatedList<AllDealViewModel>.CreateAsync(allDeals.AsQueryable(), pageIndex ?? 1, pageSize ?? 10);

            dashboardViewModel.AllDeals = allDealsForDashboard;
            dashboardViewModel.AllDealsTotalDeals = totalDeals;
            dashboardViewModel.CurrentSort = sortOrder;
            dashboardViewModel.DealTransactionNumberSort = sortOrder;

            return dashboardViewModel;
        }

        public string GetUserByUserId(Guid? id)
        {
            string fullName = "";
            if (id == null)
            {
                var user = _userManager.FindByIdAsync(id.ToString()!).GetAwaiter().GetResult();

                fullName = user!.FirstName + " " + user.LastName;    

            }
            return fullName;
        }
    }
}
