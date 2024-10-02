using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.Pagination;
using Intl.Realty.Firm.Model.ViewModel.AllDealsVM;
using Intl.Realty.Firm.Model.ViewModel.Home;
using Intl.Realty.Firm.Repository.IRepository;
using Intl.Realty.Firm.Service.IService;
using Intl.Realty.Firm.Utility.Mapper;
using Microsoft.AspNetCore.Identity;


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

        public async Task<DashboardViewModel> GetAllDealsForDashboardData(string sortOrder, string searchString, int pageNumber = 1, int pageSize = 10)
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

                    break;
                case "propertyAddress":
                    allDeals = allDeals.OrderBy(x => x.PropertyAddress);
                    break;
                case "propertyAddress_desc":
                    allDeals = allDeals.OrderByDescending(x => x.PropertyAddress);
                    break;
                case "dealTransactionNumber":
                    allDeals = allDeals.OrderBy(x => x.DealTransactionNumber);
                    break;
                case "dealTransactionNumber_desc":
                    allDeals = allDeals.OrderByDescending(x => x.DealTransactionNumber);
                    break;
                case "transactionType":
                    allDeals = allDeals.OrderBy(x => x.TransactionTypeName);
                    break;
                case "transactionType_desc":
                    allDeals = allDeals.OrderByDescending(x => x.TransactionTypeName);
                    break;
                case "dealStatus":
                    allDeals = allDeals.OrderBy(x => x.DealStatusName);
                    break;
                case "dealStatus_desc":
                    allDeals = allDeals.OrderByDescending(x => x.DealStatusName);
                    break;
                case "createdByFullName":
                    allDeals = allDeals.OrderBy(x => x.CreatedByFullName);
                    break;
                case "createdByFullName_desc":
                    allDeals = allDeals.OrderByDescending(x => x.CreatedByFullName);
                    break;
                case "finalClosingDate":
                    allDeals = allDeals.OrderBy(x => x.FinalClosingDate);
                    break;
                case "finalClosingDate_desc":
                    allDeals = allDeals.OrderByDescending(x => x.FinalClosingDate);
                    break;
                default:
                    allDeals = allDeals.OrderBy(x => x.CreatedAt);
                    break;
            }

            var totalDeals = allDeals.Count();

            var allDealsForDashboard = PaginatedList<AllDealViewModel>.CreateAsync(allDeals.AsQueryable(), pageNumber, pageSize);

            dashboardViewModel.AllDeals = allDealsForDashboard;
            dashboardViewModel.AllDealsTotalDeals = totalDeals;
            dashboardViewModel.SortOrder = sortOrder;
            dashboardViewModel.PageSize = pageSize;

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
