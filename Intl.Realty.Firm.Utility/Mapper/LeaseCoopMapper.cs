using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.AllDealsVM;
using Intl.Realty.Firm.Model.ViewModel.LeaseCoopVM;

namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class LeaseCoopMapper
    {
        public static IEnumerable<DeleteLeaseCoopViewModel> ToDeleteLeaseCoopIEnumViewModel(this IEnumerable<LeaseCoop> modelIEnum)
        {
            IEnumerable<DeleteLeaseCoopViewModel> viewModelIEnum = new List<DeleteLeaseCoopViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DeleteLeaseCoopViewModel()
                {
                    Id = x.Id,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy,
                    DeletedByFullName = x.DeletedByFullName,
                });
            }
            return viewModelIEnum;
        }
        public static EditLeaseCoopViewModel ToEditLeaseCoopViewModel(this LeaseCoop model)
        {
            return new EditLeaseCoopViewModel
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
                EditIRFDealLeaseCoopViewModel = model.IRFDeal!.ToEditIRFDealLeaseCoopViewModel(),
                DealStatus = model.DealStatus,  

                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                UpdatedBy = model.UpdatedBy,
                UpdatedAt = model.UpdatedAt
            };
        }
        public static IEnumerable<LeaseCoopViewModel> ToLeaseCoopIEnumViewModel(this IEnumerable<LeaseCoop> modelIEnum)
        {
            IEnumerable<LeaseCoopViewModel> viewModelIEnum = new List<LeaseCoopViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new LeaseCoopViewModel()
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
        public static LeaseCoop ToLeaseCoopModel(this CreateLeaseCoopViewModel viewModel)
        {
            return new LeaseCoop
            {
                DealTransactionNumber = viewModel.DealTransactionNumber,
                TransactionTypeId = viewModel.TransactionTypeId,
                IRFDealId = viewModel.IRFDealId,
                DealStatusId = viewModel.DealStatusId,
                DateSubmitted = viewModel.DateSubmitted,
                RowVersion = viewModel.RowVersion,
                CreatedByFullName = viewModel.CreatedByFullName,
                Notes = viewModel.Notes,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static LeaseCoop ToLeaseCoopModel(this EditLeaseCoopViewModel viewModel)
        {
            return new LeaseCoop
            {
                Id = viewModel.Id,
                DealTransactionNumber = viewModel.DealTransactionNumber,
                TransactionTypeId = viewModel.TransactionTypeId,
                IRFDealId = viewModel.IRFDealId,
                DealStatusId = viewModel.DealStatusId,
                DateSubmitted = viewModel.DateSubmitted,
                RowVersion = viewModel.RowVersion,
                UpdatedByFullName = viewModel.UpdatedByFullName,
                Notes = viewModel.Notes,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedAt = viewModel.UpdatedAt,
            };
        }
        public static List<LeaseCoopExport> ToLeaseCoopListExport(this List<LeaseCoopViewModel> modelList)
        {
            List<LeaseCoopExport> exportList = new List<LeaseCoopExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new LeaseCoopExport
                {
                    DealTransactionNumber = x.DealTransactionNumber,
                    TransactionType = x.TransactionType!.Description,
                    DateCreated = x.CreatedAt,
                    DateSubmitted = x.DateSubmitted??DateTime.MinValue,
                    PropertyAddress = x.IRFDeal.PropertyAddress??"",
                    FinalSalePrice = x.IRFDeal.FinalLeasePrice??0,
                    FinalClosingDate = x.IRFDeal.FinalClosingDate??DateTime.UtcNow,
                    DepositAmount = x.IRFDeal.DepositAmount??0,
                    DepositDate = x.IRFDeal.DepositDate ?? DateTime.UtcNow,
                    BuyerName = x.IRFDeal.BuyerName??"",
                    LandLordName = x.IRFDeal.LandLordName ??"",
                    ListingCommissionSign = x.IRFDeal.ListingCommissionSign??1,
                    ListingCommission = x.IRFDeal.ListingCommission??0,
                    BuyingCommissionSign = x.IRFDeal.BuyingCommissionSign ?? 1,
                    BuyingCommission = x.IRFDeal.BuyingCommission ?? 0,
                    ListingAgentName = x.IRFDeal.ListingAgentName ?? "",
                    ListingBrokerage = x.IRFDeal.ListingBrokerage ?? "",
                    ListingBrokerageFax = x.IRFDeal.ListingBrokerageFax ?? "",
                    BuyerAgentName = x.IRFDeal.BuyerAgentName ?? "",
                    BuyerBrokerage = x.IRFDeal.BuyerBrokerage ?? "",
                    BuyerBrokerageFax = x.IRFDeal.BuyerBrokerageFax ?? "",
                    SellerLawyer = x.IRFDeal.SellerLawyer ?? "",
                    SellerLawyerAddress = x.IRFDeal.SellerLawyerAddress ?? "",
                    SellerPhoneNumber = x.IRFDeal.SellerEmail ?? "",
                    BuyerLawyer = x.IRFDeal.BuyerLawyer ?? "",
                    BuyerLawyerAddress = x.IRFDeal.BuyerLawyerAddress ?? "",
                    BuyerEmailPhone = x.IRFDeal.BuyerEmail ?? "",
                    DealStatus = x.DealStatus!.Description
                }).ToList();
            }
            return exportList;
        }

        public static PreviewLeaseCoopViewModel ToPreviewLeaseCoopViewModel(this LeaseCoop model)
        {
            return new PreviewLeaseCoopViewModel
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
                FileUploads = model.FileUploads!,

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

        public static List<LeaseCoopViewModel> ToLeaseCoopListViewModel(this IEnumerable<LeaseCoop> modelList)
        {
            List<LeaseCoopViewModel> listViewModel = new List<LeaseCoopViewModel>();

            if (modelList != null)
            {
                listViewModel = modelList.Select(x => new LeaseCoopViewModel
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
        public static AllDealViewModel ToAllDealsViewModel(this LeaseCoop model,
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
            viewModel.PropertyAddress = model.IRFDeal!.PropertyAddress??"";
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