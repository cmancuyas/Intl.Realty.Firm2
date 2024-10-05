using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.Pagination;
using Intl.Realty.Firm.Model.ViewModel.AllDealsVM;
using Intl.Realty.Firm.Model.ViewModel.Home;
using Intl.Realty.Firm.Repository.IRepository;
using Intl.Realty.Firm.Service.IService;
using Intl.Realty.Firm.Utility.Mapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


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

        public async Task<DashboardViewModel> GetAllDealsForDashboardData(int pageIndex = 1, int pageSize = 10, string sortColumn = "FinalClosingDate", string sortOrder = "desc", string searchQuery = "")
        {
            var saleListingQuery = await _unitOfWork.SaleListing.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");
            var saleCoopQuery = await _unitOfWork.SaleCoop.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");
            var leaseListingQuery = await _unitOfWork.LeaseListing.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");
            var leaseCoopQuery = await _unitOfWork.LeaseCoop.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");

            List<AllDealViewModel> allDeals = new List<AllDealViewModel>();

            foreach (var item in saleListingQuery.ToList())
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

                allDeals.Add(allDeal);
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

                allDeals.Add(allDeal);
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

                allDeals.Add(allDeal);
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

                allDeals.Add(allDeal);
            }


            if (!string.IsNullOrEmpty(searchQuery))
            {
                allDeals = allDeals.Where(p => p.DealTransactionNumber.ToLower().Contains(searchQuery.ToLower()) ||
                                               p.TransactionTypeName.ToLower().Contains(searchQuery.ToLower()) ||
                                               p.PropertyAddress.ToLower().Contains(searchQuery.ToLower())  ||
                                               p.DealStatusName.ToLower().Contains(searchQuery.ToLower()) ||
                                               p.CreatedByFullName.ToLower().Contains(searchQuery.ToLower())
                                               )
    
                            .ToList();
            }

            // Sorting logic based on the sort order
            switch (sortOrder)
            {
                case "createdAt_asc":
                    allDeals = allDeals.OrderBy(x => x.CreatedAt).ToList();
                    break;
                case "createdAt_desc":
                    allDeals = allDeals.OrderByDescending(x => x.CreatedAt).ToList();
                    break;
                case "propertyAddress_asc":
                    allDeals = allDeals.OrderBy(x => x.PropertyAddress).ToList();
                    break;
                case "propertyAddress_desc":
                    allDeals = allDeals.OrderByDescending(x => x.PropertyAddress).ToList();
                    break;
                case "dealTransactionNumber_asc":
                    allDeals = allDeals.OrderBy(x => x.DealTransactionNumber).ToList();
                    break;
                case "dealTransactionNumber_desc":
                    allDeals = allDeals.OrderByDescending(x => x.DealTransactionNumber).ToList();
                    break;
                case "transactionTypeName_asc":
                    allDeals = allDeals.OrderBy(x => x.TransactionTypeName).ToList();
                    break;
                case "transactionTypeName_desc":
                    allDeals = allDeals.OrderByDescending(x => x.TransactionTypeName).ToList();
                    break;
                case "dealStatusName_asc":
                    allDeals = allDeals.OrderBy(x => x.DealStatusName).ToList();
                    break;
                case "dealStatusName_desc":
                    allDeals = allDeals.OrderByDescending(x => x.DealStatusName).ToList();
                    break;
                case "createdByFullName_asc":
                    allDeals = allDeals.OrderBy(x => x.CreatedByFullName).ToList();
                    break;
                case "createdByFullName_desc":
                    allDeals = allDeals.OrderByDescending(x => x.CreatedByFullName).ToList();
                    break;
                case "finalClosingDate_asc":
                    allDeals = allDeals.OrderBy(x => x.FinalClosingDate).ToList();
                    break;
                case "finalClosingDate_desc":
                    allDeals = allDeals.OrderByDescending(x => x.FinalClosingDate).ToList();
                    break;
                default:
                    allDeals = allDeals.OrderBy(x => x.CreatedAt).ToList();
                    break;
            }

            var totalCount = allDeals.Count();

            int currentPage = pageIndex;
            int currentPageSize = pageSize;
            var paginatedList = await PaginatedList<AllDealViewModel>.CreateAsync(allDeals, currentPage, currentPageSize);

            DashboardViewModel dashboardViewModel = new DashboardViewModel();
            dashboardViewModel.AllDeals = paginatedList;
            dashboardViewModel.AllDealsTotalCount = totalCount;
            dashboardViewModel.SortOrder = sortOrder;
            dashboardViewModel.PageSize = pageSize;
            dashboardViewModel.CurrentPage = currentPage;
            dashboardViewModel.SortColumn = sortColumn; 


            return dashboardViewModel;
        }

        public async Task<string> GetUserByUserId(Guid? id)
        {
            string fullName = "";
            if (id == null)
            {
                var user = await _userManager.FindByIdAsync(id.ToString()!);

                fullName = user!.FirstName + " " + user.LastName;

            }
            return fullName;
        }
    }
}
