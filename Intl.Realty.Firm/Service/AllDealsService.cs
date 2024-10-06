using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.Pagination;
using Intl.Realty.Firm.Model.ViewModel.AllDealsVM;
using Intl.Realty.Firm.Model.ViewModel.Home;
using Intl.Realty.Firm.Repository.IRepository;
using Intl.Realty.Firm.Service.IService;
using Intl.Realty.Firm.Utility.Mapper;
using Intl.Realty.Firm.Utility.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;


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

        public async Task<PaginatedList<AllDealViewModel>> GetAllDealsForDashboardData(ApplicationUser user, string sortProperty, SortOrder sortOrder, string searchText = "", int pageIndex = 1, int pageSize = 5, bool getCurrentMonthFlag = true)
        {
            var isAgent = await _userManager.IsInRoleAsync(user!, "Agent");

            IQueryable<SaleListing> saleListingQuery;
            IQueryable<SaleCoop> saleCoopQuery;
            IQueryable<LeaseListing> leaseListingQuery;
            IQueryable<LeaseCoop> leaseCoopQuery;
            if (getCurrentMonthFlag)
            {
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;

                // SaleListing Query
                var saleListingCurrentMonthQuery = LinqExtensions.AndAlso<SaleListing>(x => x.CreatedAt.Month == DateTime.Now.Month, x => x.CreatedAt.Year == DateTime.Now.Year);
                var saleListingCombinedQuery = LinqExtensions.AndAlso<SaleListing>(x => x.IsDeleted == false, saleListingCurrentMonthQuery);
                if (isAgent)
                {
                    saleListingCombinedQuery = LinqExtensions.AndAlso<SaleListing>(x => x.CreatedBy == user.Id, saleListingCurrentMonthQuery);
                }

                saleListingQuery = await _unitOfWork.SaleListing.AsQueryableAsync(saleListingCombinedQuery, includeProperties: "TransactionType,IRFDeal,DealStatus");

                // SaleCoop Query
                var saleCoopCurrentMonthQuery = LinqExtensions.AndAlso<SaleCoop>(x => x.CreatedAt.Month == DateTime.Now.Month, x => x.CreatedAt.Year == DateTime.Now.Year);
                var saleCoopCombinedQuery = LinqExtensions.AndAlso<SaleCoop>(x => x.IsDeleted == false, saleCoopCurrentMonthQuery);
                if (isAgent)
                {
                    saleCoopCombinedQuery = LinqExtensions.AndAlso<SaleCoop>(x => x.CreatedBy == user.Id, saleCoopCurrentMonthQuery);
                }
                
                saleCoopQuery = await _unitOfWork.SaleCoop.AsQueryableAsync(saleCoopCombinedQuery, includeProperties: "TransactionType,IRFDeal,DealStatus");

                // LeaseListing Query
                var leaseListingCurrentMonthQuery = LinqExtensions.AndAlso<LeaseListing>(x => x.CreatedAt.Month == DateTime.Now.Month, x => x.CreatedAt.Year == DateTime.Now.Year);
                var leaseListingCombinedQuery = LinqExtensions.AndAlso<LeaseListing>(x => x.IsDeleted == false, leaseListingCurrentMonthQuery);
                if (isAgent)
                {
                    leaseListingCombinedQuery = LinqExtensions.AndAlso<LeaseListing>(x => x.CreatedBy == user.Id, leaseListingCurrentMonthQuery);
                }
                
                leaseListingQuery = await _unitOfWork.LeaseListing.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");

                // LeaseCoop Query
                var leaseCoopCurrentMonthQuery = LinqExtensions.AndAlso<LeaseCoop>(x => x.CreatedAt.Month == DateTime.Now.Month, x => x.CreatedAt.Year == DateTime.Now.Year);
                var leaseCoopCombinedQuery = LinqExtensions.AndAlso<LeaseCoop>(x => x.IsDeleted == false, leaseCoopCurrentMonthQuery);
                if (isAgent)
                {
                    leaseCoopCombinedQuery = LinqExtensions.AndAlso<LeaseCoop>(x => x.CreatedBy == user.Id, leaseCoopCurrentMonthQuery);
                }

                leaseCoopQuery = await _unitOfWork.LeaseCoop.AsQueryableAsync(leaseCoopCombinedQuery, includeProperties: "TransactionType,IRFDeal,DealStatus");
            }
            else
            {
                // SaleListing Query
                if(isAgent)
                {
                    var saleListingCombinedQuery = LinqExtensions.AndAlso<SaleListing>(x => x.IsDeleted == false, x => x.CreatedBy == user.Id);
                    saleListingQuery = await _unitOfWork.SaleListing.AsQueryableAsync(saleListingCombinedQuery, includeProperties: "TransactionType,IRFDeal,DealStatus");
                }
                else
                {
                    saleListingQuery = await _unitOfWork.SaleListing.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");
                }
                
                //SaleCoop
                if(isAgent)
                {
                    var saleCoopCombinedQuery = LinqExtensions.AndAlso<SaleCoop>(x => x.IsDeleted == false, x => x.CreatedBy == user.Id);
                    saleCoopQuery = await _unitOfWork.SaleCoop.AsQueryableAsync(saleCoopCombinedQuery, includeProperties: "TransactionType,IRFDeal,DealStatus");
                }
                else
                {
                    saleCoopQuery = await _unitOfWork.SaleCoop.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");
                }

                //LeaseListing
                if (isAgent)
                {
                    var leaseListingCombinedQuery = LinqExtensions.AndAlso<LeaseListing>(x => x.IsDeleted == false, x => x.CreatedBy == user.Id);
                    leaseListingQuery = await _unitOfWork.LeaseListing.AsQueryableAsync(leaseListingCombinedQuery, includeProperties: "TransactionType,IRFDeal,DealStatus");
                }
                else
                {
                    leaseListingQuery = await _unitOfWork.LeaseListing.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");
                }
                
                if(isAgent)
                {
                    var leaseCoopCombinedQuery = LinqExtensions.AndAlso<LeaseCoop>(x => x.IsDeleted == false, x => x.CreatedBy == user.Id);
                    leaseCoopQuery = await _unitOfWork.LeaseCoop.AsQueryableAsync(leaseCoopCombinedQuery, includeProperties: "TransactionType,IRFDeal,DealStatus");
                }
                else
                {
                    leaseCoopQuery = await _unitOfWork.LeaseCoop.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");
                }
            }

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


            if (!string.IsNullOrEmpty(searchText))
            {
                allDeals = allDeals.Where(p => p.DealTransactionNumber.ToLower().Contains(searchText.ToLower()) ||
                                               p.TransactionTypeName.ToLower().Contains(searchText.ToLower()) ||
                                               p.PropertyAddress.ToLower().Contains(searchText.ToLower()) ||
                                               p.DealStatusName.ToLower().Contains(searchText.ToLower()) ||
                                               p.CreatedByFullName.ToLower().Contains(searchText.ToLower())
                                               )

                            .ToList();
            }

            allDeals = DoSort(allDeals, sortProperty, sortOrder);

            PaginatedList<AllDealViewModel> allDealsPaginated = new PaginatedList<AllDealViewModel>(allDeals, pageIndex, pageSize);

            return allDealsPaginated;
        }

        private List<AllDealViewModel> DoSort(List<AllDealViewModel> allDeals, string sortProperty, SortOrder sortOrder)
        {
            if (sortProperty == "dealTransactionNumber")
            {
                if (sortOrder == SortOrder.Ascending)
                {
                    allDeals = allDeals.OrderBy(x => x.DealTransactionNumber).ToList();
                }
                else
                {
                    allDeals = allDeals.OrderByDescending(x => x.DealTransactionNumber).ToList();
                }
            }
            else if (sortProperty == "transactionTypeName")
            {
                if (sortOrder == SortOrder.Ascending)
                {
                    allDeals = allDeals.OrderBy(x => x.TransactionTypeName).ToList();
                }
                else
                {
                    allDeals = allDeals.OrderByDescending(x => x.TransactionTypeName).ToList();
                }
            }
            else if (sortProperty == "propertyAddress")
            {
                if (sortOrder == SortOrder.Ascending)
                {
                    allDeals = allDeals.OrderBy(x => x.PropertyAddress).ToList();
                }
                else
                {
                    allDeals = allDeals.OrderByDescending(x => x.PropertyAddress).ToList();
                }
            }
            else if (sortProperty == "dealStatusName")
            {
                if (sortOrder == SortOrder.Ascending)
                {
                    allDeals = allDeals.OrderBy(x => x.DealStatusName).ToList();
                }
                else
                {
                    allDeals = allDeals.OrderByDescending(x => x.DealStatusName).ToList();
                }
            }

            else if (sortProperty == "createdByFullName")
            {
                if (sortOrder == SortOrder.Ascending)
                {
                    allDeals = allDeals.OrderBy(x => x.CreatedByFullName).ToList();
                }
                else
                {
                    allDeals = allDeals.OrderByDescending(x => x.CreatedByFullName).ToList();
                }
            }
            else if (sortProperty == "createdAt")
            {
                if (sortOrder == SortOrder.Ascending)
                {
                    allDeals = allDeals.OrderBy(x => x.CreatedAt).ToList();
                }
                else
                {
                    allDeals = allDeals.OrderByDescending(x => x.CreatedAt).ToList();
                }
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                {
                    allDeals = allDeals.OrderBy(x => x.FinalClosingDate).ToList();
                }
                else
                {
                    allDeals = allDeals.OrderByDescending(x => x.FinalClosingDate).ToList();
                }
            }

            return allDeals;
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
