using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.AllDealsVM;
using Intl.Realty.Firm.Model.ViewModel.LeaseListingVM;

namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class LeaseListingMapper
    {
        public static IEnumerable<DeleteLeaseListingViewModel> ToDeleteLeaseListingIEnumViewModel(this IEnumerable<LeaseListing> modelIEnum)
        {
            IEnumerable<DeleteLeaseListingViewModel> viewModelIEnum = new List<DeleteLeaseListingViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DeleteLeaseListingViewModel()
                {
                    Id = x.Id,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy,
                    DeletedByFullName = x.DeletedByFullName,
                });
            }
            return viewModelIEnum;
        }
        public static EditLeaseListingViewModel ToEditLeaseListingViewModel(this LeaseListing model)
        {
            return new EditLeaseListingViewModel
            {
                Id = model.Id,
                DealTransactionNumber = model.DealTransactionNumber,
                TransactionTypeId = model.TransactionTypeId,
                IRFDealId = model.IRFDealId,
                DealStatusId = model.DealStatusId,
                DateSubmitted = model.DateSubmitted,
                RowVersion = model.RowVersion,
                Notes = model.Notes,

                TransactionType = model.TransactionType,
                EditIRFDealLeaseListingViewModel = model.IRFDeal!.ToEditIRFDealLeaseListingViewModel(),
                DealStatus = model.DealStatus,  

                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                UpdatedBy = model.UpdatedBy,
                UpdatedAt = model.UpdatedAt
            };
        }
        public static IEnumerable<LeaseListingViewModel> ToLeaseListingIEnumViewModel(this IEnumerable<LeaseListing> modelIEnum)
        {
            IEnumerable<LeaseListingViewModel> viewModelIEnum = new List<LeaseListingViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new LeaseListingViewModel()
                {
                    Id = x.Id,
                    DealTransactionNumber = x.DealTransactionNumber,
                    TransactionTypeId = x.TransactionTypeId,
                    IRFDealId = x.IRFDealId,
                    DealStatusId = x.DealStatusId,
                    DateSubmitted = x.DateSubmitted,
                    RowVersion = x.RowVersion,  
                    CreatedByFullName = x.CreatedByFullName,
                    UpdatedByFullName = x.UpdatedByFullName,
                    DeletedByFullName = x.DeletedByFullName,

                    Notes = x.Notes,

                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted,
                    CreatedBy = x.CreatedBy,
                    CreatedAt = x.CreatedAt,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy,
                });
            }
            return viewModelIEnum!;
        }
        public static LeaseListing ToLeaseListingModel(this CreateLeaseListingViewModel viewModel)
        {
            return new LeaseListing
            {
                TransactionTypeId = viewModel.TransactionTypeId,
                IRFDealId = viewModel.IRFDealId,
                DealStatusId = viewModel.DealStatusId,
                DateSubmitted = viewModel.DateSubmitted,
                DealTransactionNumber = viewModel.DealTransactionNumber,
                RowVersion = viewModel.RowVersion,
                CreatedByFullName = viewModel.CreatedByFullName,
                Notes = viewModel.Notes,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static LeaseListing ToLeaseListingModel(this EditLeaseListingViewModel viewModel)
        {
            return new LeaseListing
            {
                Id = viewModel.Id,

                TransactionTypeId = viewModel.TransactionTypeId,
                IRFDealId = viewModel.IRFDealId,
                DealStatusId = viewModel.DealStatusId,
                DateSubmitted = viewModel.DateSubmitted,
                DealTransactionNumber = viewModel.DealTransactionNumber,
                TransactionType = viewModel.TransactionType,
                UpdatedByFullName = viewModel.UpdatedByFullName,
                Notes = viewModel.Notes,
                RowVersion = viewModel.RowVersion,
                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedAt = viewModel.UpdatedAt,
            };
        }
        public static List<LeaseListingExport> ToLeaseListingListExport(this List<LeaseListingViewModel> modelList)
        {
            List<LeaseListingExport> exportList = new List<LeaseListingExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new LeaseListingExport
                {
                    DealTransactionNumber = x.DealTransactionNumber,
                    TransactionType = x.TransactionType!.Description,
                    DateCreated = x.CreatedAt,
                    DateSubmitted = x.DateSubmitted??DateTime.MinValue,
                    PropertyAddress = x.IRFDeal!.PropertyAddress ?? "",
                    FinalLeasePrice = x.IRFDeal.FinalLeasePrice ?? 0,
                    FinalClosingDate = x.IRFDeal.FinalClosingDate??DateTime.UtcNow,
                    DepositAmount = x.IRFDeal.DepositAmount ?? 0,
                    DepositDate = x.IRFDeal.DepositDate??DateTime.UtcNow,
                    BuyerName = x.IRFDeal.BuyerName??"",
                    LandLordName = x.IRFDeal.LandLordName??"",
                    ListingCommissionSign = x.IRFDeal.ListingCommissionSign??1,
                    ListingCommission = x.IRFDeal.ListingCommission??0,
                    BuyingCommissionSign = x.IRFDeal.BuyingCommissionSign??1,
                    BuyingCommission= x.IRFDeal.BuyingCommission ?? 0,
                    ListingAgentName = x.IRFDeal.ListingAgentName??"",
                    ListingBrokerage = x.IRFDeal.ListingBrokerage ?? "",
                    ListingBrokerageFax = x.IRFDeal.ListingBrokerageFax ?? "",
                    BuyerAgentName = x.IRFDeal.BuyerAgentName ?? "",
                    BuyerBrokerage = x.IRFDeal.BuyerBrokerage ?? "",
                    BuyerBrokerageFax = x.IRFDeal.BuyerBrokerageFax ?? "",
                    SellerLawyer = x.IRFDeal.SellerLawyer ?? "",
                    SellerLawyerAddress = x.IRFDeal.SellerLawyerAddress ?? "",
                    SellerEmailPhone = x.IRFDeal.SellerEmail ?? "",
                    BuyerLawyer = x.IRFDeal.BuyerLawyer ?? "",
                    BuyerLawyerAddress = x.IRFDeal.BuyerLawyerAddress ?? "",
                    BuyerEmailPhone = x.IRFDeal.BuyerEmail ?? "",
                    DealStatus = x.DealStatus!.Description
                }).ToList();
            }
            return exportList;
        }

        public static PreviewLeaseListingViewModel ToPreviewLeaseListingViewModel(this LeaseListing model)
        {
            return new PreviewLeaseListingViewModel
            {
                Id = model.Id,
                DealTransactionNumber = model.DealTransactionNumber,
                TransactionTypeId = model.TransactionTypeId,
                IRFDealId = model.IRFDealId,
                DealStatusId = model.DealStatusId,
                DateSubmitted = model.DateSubmitted,
                TransactionType = model.TransactionType!,
                IRFDeal = model.IRFDeal!,
                DealStatus = model.DealStatus!,
                FileUploads = model.FileUploads?
                .Where(x => x.IsActive && !x.IsDeleted)
                .ToList(),

                SavedAdditionalDocuments = model.AdditionalDocuments?
                .Where(x => x.IsActive && !x.IsDeleted)
                .ToList(),

                CreatedByFullName = model.CreatedByFullName,
                UpdatedByFullName = model.UpdatedByFullName,
                DeletedByFullName = model.DeletedByFullName,

                Notes = model.Notes!,

                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                CreatedAt = model.CreatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.UpdatedBy,
                UpdatedAt = model.UpdatedAt,
                DeletedBy = model.DeletedBy,
                DeletedAt = model.DeletedAt,
            };

        }

        public static List<LeaseListingViewModel> ToLeaseListingListViewModel(this IEnumerable<LeaseListing> modelList)
        {
            List<LeaseListingViewModel> listViewModel = new List<LeaseListingViewModel>();

            if (modelList != null)
            {
                listViewModel = modelList.Select(x => new LeaseListingViewModel
                {
                    Id = x.Id,
                    DealTransactionNumber = x.DealTransactionNumber,
                    TransactionTypeId = x.TransactionTypeId,
                    IRFDealId = x.IRFDealId,
                    DealStatusId = x.DealStatusId,
                    RowVersion = x.RowVersion,
                    TransactionType = x.TransactionType,
                    IRFDeal = x.IRFDeal,
                    DealStatus = x.DealStatus,
                    FileUploads = x.FileUploads,
                    
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedAt = x.UpdatedAt,
                    DeletedBy = x.DeletedBy,
                    DeletedAt = x.DeletedAt,
                }).ToList();
            }
            return listViewModel; ;
        }

        public static AllDealViewModel ToAllDealsViewModel(this LeaseListing model,
                                            ApplicationUser createdByUser,
                                            ApplicationUser updatedByUser,
                                            ApplicationUser deletedByUser
                                            )
        {
            AllDealViewModel viewModel = new AllDealViewModel();

            viewModel.Id = model.Id;
            viewModel.BridgeId = model.Id;
            viewModel.DealTransactionNumber = model.DealTransactionNumber;
            viewModel.TransactionTypeId = model.TransactionTypeId;
            viewModel.TransactionTypeName = model.TransactionType!.Description;
            viewModel.BridgeId = model.Id;
            viewModel.PropertyAddress = model.IRFDeal!.PropertyAddress ?? "";
            viewModel.DealStatusName = model.DealStatus!.Description;
            viewModel.FinalClosingDate = model.IRFDeal.FinalClosingDate??DateTime.UtcNow;
            viewModel.FileExtensions = model.FileUploads!.Select(x => x.FileExtension).ToList();
            viewModel.CreatedAt = model.CreatedAt;
            viewModel.CreatedByFullName = createdByUser.FirstName + " " + createdByUser.LastName;
            viewModel.UpdatedByFullName = updatedByUser.FirstName + " " + updatedByUser.LastName;
            viewModel.DeletedByFullName = deletedByUser.FirstName + " " + deletedByUser.LastName;
            viewModel.RowVersion = model.RowVersion;
            return viewModel;
        }
    }
}