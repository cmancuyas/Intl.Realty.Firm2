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

        public async Task<DashboardViewModel> GetAllDealsForDashboardData(int pageIndex, int pageSize, string search, string sortOrder)
        {
            var saleListingQuery = await _unitOfWork.SaleListing.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");
            var saleCoopQuery = await _unitOfWork.SaleCoop.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");
            var leaseListingQuery = await _unitOfWork.LeaseListing.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");
            var leaseCoopQuery = await _unitOfWork.LeaseCoop.AsQueryableAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");



            var allDeals = saleListingQuery
                .Select(p => new AllDealViewModel
                {
                    Id = p.Id,
                    BridgeId = p.Id,
                    DealTransactionNumber = p.DealTransactionNumber,
                    TransactionTypeId = p.TransactionTypeId,
                    TransactionTypeName = p.TransactionType!.Description,

                    PropertyAddress = p.IRFDeal!.PropertyAddress,
                    DealStatusName = p.DealStatus!.Description,
                    FinalClosingDate = p.IRFDeal.FinalClosingDate,
                    FileExtensions = p.FileUploads!.Select(x => x.FileExtension).ToList(),
                    CreatedAt = p.CreatedAt,
                    CreatedByFullName = GetUserByUserId(p.CreatedBy),
                    UpdatedByFullName = GetUserByUserId(p.UpdatedBy),
                    DeletedByFullName = GetUserByUserId(p.DeletedBy)

                })
                .Concat(saleCoopQuery.Select(p => new AllDealViewModel
                {
                    Id = p.Id,
                    BridgeId = p.Id,
                    DealTransactionNumber = p.DealTransactionNumber,
                    TransactionTypeId = p.TransactionTypeId,
                    TransactionTypeName = p.TransactionType!.Description,

                    PropertyAddress = p.IRFDeal!.PropertyAddress,
                    DealStatusName = p.DealStatus!.Description,
                    FinalClosingDate = p.IRFDeal.FinalClosingDate,
                    FileExtensions = p.FileUploads!.Select(x => x.FileExtension).ToList(),
                    CreatedAt = p.CreatedAt,
                    CreatedByFullName = GetUserByUserId(p.CreatedBy),
                    UpdatedByFullName = GetUserByUserId(p.UpdatedBy),
                    DeletedByFullName = GetUserByUserId(p.DeletedBy)
                }))
                .Concat(leaseListingQuery.Select(p => new AllDealViewModel
                 {
                     Id = p.Id,
                     BridgeId = p.Id,
                     DealTransactionNumber = p.DealTransactionNumber,
                     TransactionTypeId = p.TransactionTypeId,
                     TransactionTypeName = p.TransactionType!.Description,

                     PropertyAddress = p.IRFDeal!.PropertyAddress,
                     DealStatusName = p.DealStatus!.Description,
                     FinalClosingDate = p.IRFDeal.FinalClosingDate,
                     FileExtensions = p.FileUploads!.Select(x => x.FileExtension).ToList(),
                     CreatedAt = p.CreatedAt,
                     CreatedByFullName = GetUserByUserId(p.CreatedBy),
                     UpdatedByFullName = GetUserByUserId(p.UpdatedBy),
                     DeletedByFullName = GetUserByUserId(p.DeletedBy)
                 }))
                .Concat(leaseCoopQuery.Select(p => new AllDealViewModel
                {
                    Id = p.Id,
                    BridgeId = p.Id,
                    DealTransactionNumber = p.DealTransactionNumber,
                    TransactionTypeId = p.TransactionTypeId,
                    TransactionTypeName = p.TransactionType!.Description,

                    PropertyAddress = p.IRFDeal!.PropertyAddress,
                    DealStatusName = p.DealStatus!.Description,
                    FinalClosingDate = p.IRFDeal.FinalClosingDate,
                    FileExtensions = p.FileUploads!.Select(x => x.FileExtension).ToList(),
                    CreatedAt = p.CreatedAt,
                    CreatedByFullName = GetUserByUserId(p.CreatedBy),
                    UpdatedByFullName = GetUserByUserId(p.UpdatedBy),
                    DeletedByFullName = GetUserByUserId(p.DeletedBy)
                }));

            if (!string.IsNullOrEmpty(search))
            {
                allDeals = allDeals.Where(p => p.TransactionTypeName.ToLower().Contains(search));
                allDeals = allDeals.Where(p => p.PropertyAddress.ToLower().Contains(search));
                allDeals = allDeals.Where(p => p.DealStatusName.ToLower().Contains(search));
                allDeals = allDeals.Where(p => p.CreatedByFullName.ToLower().Contains(search));
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

            var allDealsForDashboard = await PaginatedList<AllDealViewModel>.CreateAsync(allDeals.AsQueryable(), pageIndex, pageSize);

            return new DashboardViewModel
            {
                AllDeals = allDealsForDashboard,
                AllDealsTotalDeals = totalDeals
            };

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
