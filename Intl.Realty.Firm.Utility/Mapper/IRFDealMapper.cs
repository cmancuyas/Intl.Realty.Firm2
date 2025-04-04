using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.IRFDealVM;

namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class IRFDealMapper
    {
        public static IEnumerable<DeleteIRFDealViewModel> ToDeleteIRFDealIEnumViewModel(this IEnumerable<IRFDeal> modelIEnum)
        {
            IEnumerable<DeleteIRFDealViewModel> viewModelIEnum = new List<DeleteIRFDealViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DeleteIRFDealViewModel()
                {
                    Id = x.Id,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy,
                });
            }
            return viewModelIEnum;
        }
        public static EditIRFDealSaleListingViewModel ToEditIRFDealSaleListingViewModel(this IRFDeal model)
        {
            return new EditIRFDealSaleListingViewModel
            {
                Id = model.Id,
                //SaleListingId = model.SaleListingId,

                PropertyAddress = model.PropertyAddress,
                FinalSalePrice = model.FinalSalePrice ?? 0,
                FinalClosingDate = model.FinalClosingDate,

                DepositAmount = model.DepositAmount,
                DepositDate = model.DepositDate,
                BuyerName = model.BuyerName,
                LandLordName = model.LandLordName,
                ListingCommissionPercentage = model.ListingCommissionPercentage,
                BuyingCommissionPercentage = model.BuyingCommissionPercentage,
                ListingAgentName = model.ListingAgentName ?? "",
                ListingBrokerage = model.ListingBrokerage ?? "",
                ListingBrokerageFax = model.ListingBrokerageFax ?? "",
                BuyerAgentName = model.BuyerAgentName ?? "",
                BuyerBrokerage = model.BuyerBrokerage ?? "",
                BuyerBrokerageFax = model.BuyerBrokerageFax ?? "",
                SellersLawyer = model.SellersLawyer ?? "",
                SellersLawyerAddress = model.SellersLawyerAddress ?? "",
                SellersPhoneNumber = model.SellersPhoneNumber ?? "",
                BuyersLawyer = model.BuyersLawyer ?? "",
                BuyersLawyerAddress = model.BuyersLawyerAddress ?? "",
                BuyersPhoneNumber = model.BuyersPhoneNumber ?? "",

                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                UpdatedBy = model.UpdatedBy ?? Guid.Empty,
                UpdatedAt = model.UpdatedAt ?? DateTime.Now
            };
        }
        public static EditIRFDealSaleCoopViewModel ToEditIRFDealSaleCoopViewModel(this IRFDeal model)
        {
            return new EditIRFDealSaleCoopViewModel
            {
                Id = model.Id,
                //SaleListingId = model.SaleListingId,

                PropertyAddress = model.PropertyAddress,
                FinalSalePrice = model.FinalSalePrice ?? 0,
                FinalClosingDate = model.FinalClosingDate,

                DepositAmount = model.DepositAmount,
                DepositDate = model.DepositDate,
                BuyerName = model.BuyerName,
                LandLordName = model.LandLordName,
                //ListingCommissionPercentage = model.ListingCommissionPercentage,
                BuyingCommissionPercentage = model.BuyingCommissionPercentage,
                ListingAgentName = model.ListingAgentName ?? "",
                ListingBrokerage = model.ListingBrokerage ?? "",
                ListingBrokerageFax = model.ListingBrokerageFax ?? "",
                BuyerAgentName = model.BuyerAgentName ?? "",
                BuyerBrokerage = model.BuyerBrokerage ?? "",
                BuyerBrokerageFax = model.BuyerBrokerageFax ?? "",
                SellersLawyer = model.SellersLawyer ?? "",
                SellersLawyerAddress = model.SellersLawyerAddress ?? "",
                SellersPhoneNumber = model.SellersPhoneNumber ?? "",
                BuyersLawyer = model.BuyersLawyer ?? "",
                BuyersLawyerAddress = model.BuyersLawyerAddress ?? "",
                BuyersPhoneNumber = model.BuyersPhoneNumber ?? "",

                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                UpdatedBy = model.UpdatedBy ?? Guid.Empty,
                UpdatedAt = model.UpdatedAt ?? DateTime.Now
            };
        }
        public static EditIRFDealLeaseListingViewModel ToEditIRFDealLeaseListingViewModel(this IRFDeal model)
        {
            return new EditIRFDealLeaseListingViewModel
            {
                Id = model.Id,
                //SaleListingId = model.SaleListingId,

                PropertyAddress = model.PropertyAddress,
                FinalLeasePrice = model.FinalLeasePrice ?? 0,
                FinalClosingDate = model.FinalClosingDate,

                DepositAmount = model.DepositAmount,
                DepositDate = model.DepositDate,
                TenantName = model.TenantName ?? "",
                LandLordName = model.LandLordName,
                ListingCommissionPercentage = model.ListingCommissionPercentage,
                BuyingCommissionPercentage = model.BuyingCommissionPercentage,
                ListingAgentName = model.ListingAgentName ?? "",
                ListingBrokerage = model.ListingBrokerage ?? "",
                ListingBrokerageFax = model.ListingBrokerageFax ?? "",
                TenantAgentName = model.TenantAgentName ?? "",
                TenantBrokerage = model.TenantBrokerage ?? "",
                TenantBrokerageFax = model.TenantBrokerageFax ?? "",
                TenantEmailAddress = model.TenantEmailAddress ?? "",

                TenantPhoneNumber = model.TenantPhoneNumber ?? "",
                LandLordPhoneNumber = model.LandLordPhoneNumber ?? "",
                LandLordEmailAddress = model.LandLordEmailAddress ?? "",

                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                UpdatedBy = model.UpdatedBy ?? Guid.Empty,
                UpdatedAt = model.UpdatedAt??DateTime.Now   
            };
        }

        public static EditIRFDealLeaseCoopViewModel ToEditIRFDealLeaseCoopViewModel(this IRFDeal model)
        {
            return new EditIRFDealLeaseCoopViewModel
            {
                Id = model.Id,
                //SaleListingId = model.SaleListingId,

                PropertyAddress = model.PropertyAddress,
                FinalLeasePrice = model.FinalLeasePrice ?? 0,
                FinalClosingDate = model.FinalClosingDate,

                DepositAmount = model.DepositAmount,
                DepositDate = model.DepositDate,
                TenantName = model.TenantName ?? "",
                LandLordName = model.LandLordName,
                //ListingCommissionPercentage = model.ListingCommissionPercentage,
                BuyingCommissionPercentage = model.BuyingCommissionPercentage,
                ListingAgentName = model.ListingAgentName ?? "",
                ListingBrokerage = model.ListingBrokerage ?? "",
                ListingBrokerageFax = model.ListingBrokerageFax ?? "",
                TenantAgentName = model.TenantAgentName ?? "",
                TenantBrokerage = model.TenantBrokerage ?? "",
                TenantBrokerageFax = model.TenantBrokerageFax ?? "",
                TenantEmailAddress = model.TenantEmailAddress ?? "",
                TenantPhoneNumber = model.TenantPhoneNumber ?? "",
                LandLordPhoneNumber = model.LandLordPhoneNumber ?? "",
                LandLordEmailAddress = model.LandLordEmailAddress ?? "",

                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                UpdatedBy = model.UpdatedBy ?? Guid.Empty,
                UpdatedAt = model.UpdatedAt ?? DateTime.Now
            };
        }
        public static IEnumerable<IRFDealViewModel> ToIRFDealIEnumViewModel(this IEnumerable<IRFDeal> modelIEnum)
        {
            IEnumerable<IRFDealViewModel> viewModelIEnum = new List<IRFDealViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new IRFDealViewModel()
                {
                    Id = x.Id,
                    //SaleListingId = x.SaleListingId,

                    PropertyAddress = x.PropertyAddress,
                    FinalSalePrice = x.FinalSalePrice,
                    FinalClosingDate = x.FinalClosingDate,
                    DepositAmount = x.DepositAmount,
                    DepositDate = x.DepositDate,
                    BuyerName = x.BuyerName,
                    LandLordName = x.LandLordName,
                    ListingCommissionPercentage = x.ListingCommissionPercentage,
                    BuyingCommissionPercentage = x.BuyingCommissionPercentage,
                    ListingAgentName = x.ListingAgentName,
                    ListingBrokerage = x.ListingBrokerage,
                    ListingBrokerageFax = x.ListingBrokerageFax,
                    BuyerAgentName = x.BuyerAgentName,
                    BuyerBrokerage = x.BuyerBrokerage,
                    BuyerBrokerageFax = x.BuyerBrokerageFax,
                    SellersLawyer = x.SellersLawyer,
                    SellersLawyerAddress = x.SellersLawyerAddress,
                    SellersPhoneNumber = x.SellersPhoneNumber,
                    BuyersLawyer = x.BuyersLawyer,
                    BuyersLawyerAddress = x.BuyersLawyerAddress,
                    BuyersPhoneNumber = x.BuyersPhoneNumber,

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
        public static IRFDeal ToIRFDealModel(this CreateIRFDealSaleListingViewModel viewModel)
        {
            return new IRFDeal
            {
                //SaleListingId = viewModel.SaleListingId,

                PropertyAddress = viewModel.PropertyAddress,
                FinalSalePrice = viewModel.FinalSalePrice,
                FinalClosingDate = viewModel.FinalClosingDate,
                DepositAmount = viewModel.DepositAmount,
                DepositDate = viewModel.DepositDate,
                BuyerName = viewModel.BuyerName,
                LandLordName = viewModel.LandLordName,
                ListingCommissionPercentage = viewModel.ListingCommissionPercentage,
                BuyingCommissionPercentage = viewModel.BuyingCommissionPercentage,
                ListingAgentName = viewModel.ListingAgentName,
                ListingBrokerage = viewModel.ListingBrokerage,
                ListingBrokerageFax = viewModel.ListingBrokerageFax,
                BuyerAgentName = viewModel.BuyerAgentName,
                BuyerBrokerage = viewModel.BuyerBrokerage,
                BuyerBrokerageFax = viewModel.BuyerBrokerageFax,
                SellersLawyer = viewModel.SellersLawyer,
                SellersLawyerAddress = viewModel.SellersLawyerAddress,
                SellersPhoneNumber = viewModel.SellersPhoneNumber,
                BuyersLawyer = viewModel.BuyersLawyer,
                BuyersLawyerAddress = viewModel.BuyersLawyerAddress,
                BuyersPhoneNumber = viewModel.BuyersPhoneNumber,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static IRFDeal ToIRFDealModel(this CreateIRFDealSaleCoopViewModel viewModel)
        {
            return new IRFDeal
            {
                //SaleListingId = viewModel.SaleListingId,

                PropertyAddress = viewModel.PropertyAddress,
                FinalSalePrice = viewModel.FinalSalePrice,
                FinalClosingDate = viewModel.FinalClosingDate,
                DepositAmount = viewModel.DepositAmount,
                DepositDate = viewModel.DepositDate,
                BuyerName = viewModel.BuyerName,
                LandLordName = viewModel.LandLordName,
                //ListingCommissionPercentage = viewModel.ListingCommissionPercentage,
                BuyingCommissionPercentage = viewModel.BuyingCommissionPercentage,
                ListingAgentName = viewModel.ListingAgentName,
                ListingBrokerage = viewModel.ListingBrokerage,
                ListingBrokerageFax = viewModel.ListingBrokerageFax,
                BuyerAgentName = viewModel.BuyerAgentName,
                BuyerBrokerage = viewModel.BuyerBrokerage,
                BuyerBrokerageFax = viewModel.BuyerBrokerageFax,
                SellersLawyer = viewModel.SellersLawyer,
                SellersLawyerAddress = viewModel.SellersLawyerAddress,
                SellersPhoneNumber = viewModel.SellersPhoneNumber,
                BuyersLawyer = viewModel.BuyersLawyer,
                BuyersLawyerAddress = viewModel.BuyersLawyerAddress,
                BuyersPhoneNumber = viewModel.BuyersPhoneNumber,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static IRFDeal ToIRFDealModel(this CreateIRFDealLeaseListingViewModel viewModel)
        {
            return new IRFDeal
            {
                //SaleListingId = viewModel.SaleListingId,

                PropertyAddress = viewModel.PropertyAddress,
                FinalLeasePrice = viewModel.FinalLeasePrice,
                FinalClosingDate = viewModel.FinalClosingDate,
                DepositAmount = viewModel.DepositAmount,
                DepositDate = viewModel.DepositDate,
                TenantName = viewModel.TenantName,
                LandLordName = viewModel.LandLordName,
                ListingCommissionPercentage = viewModel.ListingCommissionPercentage,
                BuyingCommissionPercentage = viewModel.BuyingCommissionPercentage,
                ListingAgentName = viewModel.ListingAgentName,
                ListingBrokerage = viewModel.ListingBrokerage,
                ListingBrokerageFax = viewModel.ListingBrokerageFax,
                TenantAgentName = viewModel.TenantAgentName,
                TenantBrokerage = viewModel.TenantBrokerage,
                TenantBrokerageFax = viewModel.TenantBrokerageFax,

                TenantPhoneNumber = viewModel.TenantPhoneNumber ?? "",
                LandLordPhoneNumber = viewModel.LandLordPhoneNumber ?? "",
                LandLordEmailAddress = viewModel.LandLordEmailAddress ?? "",

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static IRFDeal ToIRFDealModel(this CreateIRFDealLeaseCoopViewModel viewModel)
        {
            return new IRFDeal
            {
                //SaleListingId = viewModel.SaleListingId,

                PropertyAddress = viewModel.PropertyAddress,
                FinalLeasePrice = viewModel.FinalLeasePrice,
                FinalClosingDate = viewModel.FinalClosingDate,
                DepositAmount = viewModel.DepositAmount,
                DepositDate = viewModel.DepositDate,
                TenantName = viewModel.TenantName,
                LandLordName = viewModel.LandLordName,
                //ListingCommissionPercentage = viewModel.ListingCommissionPercentage,
                BuyingCommissionPercentage = viewModel.BuyingCommissionPercentage,
                ListingAgentName = viewModel.ListingAgentName,
                ListingBrokerage = viewModel.ListingBrokerage,
                ListingBrokerageFax = viewModel.ListingBrokerageFax,
                TenantAgentName = viewModel.TenantAgentName,
                TenantBrokerage = viewModel.TenantBrokerage,
                TenantBrokerageFax = viewModel.TenantBrokerageFax,

                TenantPhoneNumber = viewModel.TenantPhoneNumber ?? "",
                LandLordPhoneNumber = viewModel.LandLordPhoneNumber ?? "",
                LandLordEmailAddress = viewModel.LandLordEmailAddress ?? "",

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static IRFDeal ToIRFDealModel(this EditIRFDealSaleListingViewModel viewModel)
        {
            return new IRFDeal
            {
                Id = viewModel.Id,
                //SaleListingId = viewModel.SaleListingId,

                PropertyAddress = viewModel.PropertyAddress,
                FinalSalePrice = viewModel.FinalSalePrice,
                FinalClosingDate = viewModel.FinalClosingDate,
                DepositAmount = viewModel.DepositAmount,
                DepositDate = viewModel.DepositDate,
                BuyerName = viewModel.BuyerName,
                LandLordName = viewModel.LandLordName,
                ListingCommissionPercentage = viewModel.ListingCommissionPercentage,
                BuyingCommissionPercentage = viewModel.BuyingCommissionPercentage,
                ListingAgentName = viewModel.ListingAgentName,
                ListingBrokerage = viewModel.ListingBrokerage,
                ListingBrokerageFax = viewModel.ListingBrokerageFax,
                BuyerAgentName = viewModel.BuyerAgentName,
                BuyerBrokerage = viewModel.BuyerBrokerage,
                BuyerBrokerageFax = viewModel.BuyerBrokerageFax,
                SellersLawyer = viewModel.SellersLawyer,
                SellersLawyerAddress = viewModel.SellersLawyerAddress,
                SellersPhoneNumber = viewModel.SellersPhoneNumber,
                BuyersLawyer = viewModel.BuyersLawyer,
                BuyersLawyerAddress = viewModel.BuyersLawyerAddress,
                BuyersPhoneNumber = viewModel.BuyersPhoneNumber,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedAt = viewModel.UpdatedAt,
                DeletedBy = viewModel.DeletedBy,
                DeletedAt = viewModel.DeletedAt,
            };
        }
        public static IRFDeal ToIRFDealModel(this EditIRFDealSaleCoopViewModel viewModel)
        {
            return new IRFDeal
            {
                Id = viewModel.Id,
                //SaleListingId = viewModel.SaleListingId,

                PropertyAddress = viewModel.PropertyAddress,
                FinalSalePrice = viewModel.FinalSalePrice,
                FinalClosingDate = viewModel.FinalClosingDate,
                DepositAmount = viewModel.DepositAmount,
                DepositDate = viewModel.DepositDate,
                BuyerName = viewModel.BuyerName,
                LandLordName = viewModel.LandLordName,
                //ListingCommissionPercentage = viewModel.ListingCommissionPercentage,
                BuyingCommissionPercentage = viewModel.BuyingCommissionPercentage,
                ListingAgentName = viewModel.ListingAgentName,
                ListingBrokerage = viewModel.ListingBrokerage,
                ListingBrokerageFax = viewModel.ListingBrokerageFax,
                BuyerAgentName = viewModel.BuyerAgentName,
                BuyerBrokerage = viewModel.BuyerBrokerage,
                BuyerBrokerageFax = viewModel.BuyerBrokerageFax,
                SellersLawyer = viewModel.SellersLawyer,
                SellersLawyerAddress = viewModel.SellersLawyerAddress,
                SellersPhoneNumber = viewModel.SellersPhoneNumber,
                BuyersLawyer = viewModel.BuyersLawyer,
                BuyersLawyerAddress = viewModel.BuyersLawyerAddress,
                BuyersPhoneNumber = viewModel.BuyersPhoneNumber,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedAt = viewModel.UpdatedAt,
                DeletedBy = viewModel.DeletedBy,
                DeletedAt = viewModel.DeletedAt,
            };
        }
        public static IRFDeal ToIRFDealModel(this EditIRFDealLeaseListingViewModel viewModel)
        {
            return new IRFDeal
            {
                Id = viewModel.Id,
                //SaleListingId = viewModel.SaleListingId,

                PropertyAddress = viewModel.PropertyAddress,
                FinalLeasePrice = viewModel.FinalLeasePrice,
                FinalClosingDate = viewModel.FinalClosingDate,
                DepositAmount = viewModel.DepositAmount,
                DepositDate = viewModel.DepositDate,
                TenantName = viewModel.TenantName,
                LandLordName = viewModel.LandLordName,
                ListingCommissionPercentage = viewModel.ListingCommissionPercentage,
                BuyingCommissionPercentage = viewModel.BuyingCommissionPercentage,
                ListingAgentName = viewModel.ListingAgentName,
                ListingBrokerage = viewModel.ListingBrokerage,
                ListingBrokerageFax = viewModel.ListingBrokerageFax,
                TenantAgentName = viewModel.TenantAgentName,
                TenantBrokerage = viewModel.TenantBrokerage,
                TenantBrokerageFax = viewModel.TenantBrokerageFax,
                TenantPhoneNumber = viewModel.TenantPhoneNumber,
                LandLordPhoneNumber = viewModel.LandLordPhoneNumber,
                LandLordEmailAddress = viewModel.LandLordEmailAddress,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedAt = viewModel.UpdatedAt,
                DeletedBy = viewModel.DeletedBy,
                DeletedAt = viewModel.DeletedAt,
            };
        }
        public static IRFDeal ToIRFDealModel(this EditIRFDealLeaseCoopViewModel viewModel)
        {
            return new IRFDeal
            {
                Id = viewModel.Id,
                //SaleListingId = viewModel.SaleListingId,

                PropertyAddress = viewModel.PropertyAddress,
                FinalLeasePrice = viewModel.FinalLeasePrice,
                FinalClosingDate = viewModel.FinalClosingDate,
                DepositAmount = viewModel.DepositAmount,
                DepositDate = viewModel.DepositDate,
                TenantName = viewModel.TenantName,
                LandLordName = viewModel.LandLordName,
                //ListingCommissionPercentage = viewModel.ListingCommissionPercentage,
                BuyingCommissionPercentage = viewModel.BuyingCommissionPercentage,
                ListingAgentName = viewModel.ListingAgentName,
                ListingBrokerage = viewModel.ListingBrokerage,
                ListingBrokerageFax = viewModel.ListingBrokerageFax,
                TenantAgentName = viewModel.TenantAgentName,
                TenantBrokerage = viewModel.TenantBrokerage,
                TenantBrokerageFax = viewModel.TenantBrokerageFax,
                TenantPhoneNumber = viewModel.TenantPhoneNumber,
                LandLordPhoneNumber = viewModel.LandLordPhoneNumber,
                LandLordEmailAddress = viewModel.LandLordEmailAddress,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedAt = viewModel.UpdatedAt,
                DeletedBy = viewModel.DeletedBy,
                DeletedAt = viewModel.DeletedAt,
            };
        }
        public static List<IRFDealExport> ToIRFDealListExport(this List<IRFDealViewModel> modelList)
        {
            List<IRFDealExport> exportList = new List<IRFDealExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new IRFDealExport
                {
                    PropertyAddress = x.PropertyAddress,
                    FinalSalePrice = x.FinalSalePrice,
                    FinalClosingDate = x.FinalClosingDate,
                    DepositAmount = x.DepositAmount,
                    DepositDate = x.DepositDate,
                    BuyerName = x.BuyerName,
                    LandLordName = x.LandLordName,
                    ListingCommissionPercentage = x.ListingCommissionPercentage,
                    BuyingCommissionPercentage = x.BuyingCommissionPercentage,
                    ListingAgentName = x.ListingAgentName,
                    ListingBrokerage = x.ListingBrokerage,
                    ListingBrokerageFax = x.ListingBrokerageFax,
                    BuyerAgentName = x.BuyerAgentName,
                    BuyerBrokerage = x.BuyerBrokerage,
                    BuyerBrokerageFax = x.BuyerBrokerageFax,
                    SellersLawyer = x.SellersLawyer,
                    SellersLawyerAddress = x.SellersLawyerAddress,
                    SellersPhoneNumber = x.SellersPhoneNumber,
                    BuyersLawyer = x.BuyersLawyer,
                    BuyersLawyerAddress = x.BuyersLawyerAddress,
                    BuyersPhoneNumber = x.BuyersPhoneNumber,

                }).ToList();
            }
            return exportList;
        }

        public static PreviewIRFDealViewModel ToPreviewIRFDealViewModel(this IRFDeal model)
        {
            return new PreviewIRFDealViewModel
            {
                Id = model.Id,
                //SaleListingId = model.SaleListingId,

                PropertyAddress = model.PropertyAddress,
                FinalSalePrice = model.FinalSalePrice,
                FinalClosingDate = model.FinalClosingDate,
                DepositAmount = model.DepositAmount,
                DepositDate = model.DepositDate,
                BuyerName = model.BuyerName,
                LandLordName = model.LandLordName,
                ListingCommissionPercentage = model.ListingCommissionPercentage,
                BuyingCommissionPercentage = model.BuyingCommissionPercentage,
                ListingAgentName = model.ListingAgentName,
                ListingBrokerage = model.ListingBrokerage,
                ListingBrokerageFax = model.ListingBrokerageFax,
                BuyerAgentName = model.BuyerAgentName,
                BuyerBrokerage = model.BuyerBrokerage,
                BuyerBrokerageFax = model.BuyerBrokerageFax,
                SellersLawyer = model.SellersLawyer,
                SellersLawyerAddress = model.SellersLawyerAddress,
                SellersPhoneNumber = model.SellersPhoneNumber,
                BuyersLawyer = model.BuyersLawyer,
                BuyersLawyerAddress = model.BuyersLawyerAddress,
                BuyersPhoneNumber = model.BuyersPhoneNumber,

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

        public static List<IRFDealViewModel> ToIRFDealListViewModel(this IEnumerable<IRFDeal> modelList)
        {
            List<IRFDealViewModel> listViewModel = new List<IRFDealViewModel>();

            if (modelList != null)
            {
                listViewModel = modelList.Select(x => new IRFDealViewModel
                {
                    Id = x.Id,
                    //SaleListingId = x.SaleListingId,

                    PropertyAddress = x.PropertyAddress,
                    FinalSalePrice = x.FinalSalePrice,
                    FinalClosingDate = x.FinalClosingDate,
                    DepositAmount = x.DepositAmount,
                    DepositDate = x.DepositDate,
                    BuyerName = x.BuyerName,
                    LandLordName = x.LandLordName,
                    ListingCommissionPercentage = x.ListingCommissionPercentage,
                    BuyingCommissionPercentage = x.BuyingCommissionPercentage,
                    ListingAgentName = x.ListingAgentName,
                    ListingBrokerage = x.ListingBrokerage,
                    ListingBrokerageFax = x.ListingBrokerageFax,
                    BuyerAgentName = x.BuyerAgentName,
                    BuyerBrokerage = x.BuyerBrokerage,
                    BuyerBrokerageFax = x.BuyerBrokerageFax,
                    SellersLawyer = x.SellersLawyer,
                    SellersLawyerAddress = x.SellersLawyerAddress,
                    SellersPhoneNumber = x.SellersPhoneNumber,
                    BuyersLawyer = x.BuyersLawyer,
                    BuyersLawyerAddress = x.BuyersLawyerAddress,
                    BuyersPhoneNumber = x.BuyersPhoneNumber,

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

        public static void UpdateFromViewModel(this IRFDeal entity, EditIRFDealLeaseCoopViewModel viewModel, Guid updatedBy)
        {
            entity.PropertyAddress = viewModel.PropertyAddress;
            entity.FinalLeasePrice = viewModel.FinalLeasePrice;
            entity.FinalClosingDate = viewModel.FinalClosingDate;
            entity.DepositAmount = viewModel.DepositAmount;
            entity.DepositDate = viewModel.DepositDate;
            entity.TenantName = viewModel.TenantName;
            entity.LandLordName = viewModel.LandLordName;
            entity.BuyingCommissionPercentage = viewModel.BuyingCommissionPercentage;
            entity.ListingAgentName = viewModel.ListingAgentName;
            entity.ListingBrokerage = viewModel.ListingBrokerage;
            entity.ListingBrokerageFax = viewModel.ListingBrokerageFax;
            entity.TenantAgentName = viewModel.TenantAgentName;
            entity.TenantEmailAddress = viewModel.TenantEmailAddress;
            entity.TenantBrokerage = viewModel.TenantBrokerage;
            entity.TenantBrokerageFax = viewModel.TenantBrokerageFax;
            entity.TenantPhoneNumber = viewModel.TenantPhoneNumber;
            entity.LandLordPhoneNumber = viewModel.LandLordPhoneNumber;
            entity.LandLordEmailAddress = viewModel.LandLordEmailAddress;
            entity.IsActive = viewModel.IsActive;
            entity.IsDeleted = viewModel.IsDeleted;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        public static void UpdateFromViewModel(this IRFDeal entity, EditIRFDealLeaseListingViewModel viewModel, Guid updatedBy)
        {
            entity.PropertyAddress = viewModel.PropertyAddress;
            entity.FinalLeasePrice = viewModel.FinalLeasePrice;
            entity.FinalClosingDate = viewModel.FinalClosingDate;
            entity.DepositAmount = viewModel.DepositAmount;
            entity.DepositDate = viewModel.DepositDate;
            entity.TenantName = viewModel.TenantName;
            entity.LandLordName = viewModel.LandLordName;
            entity.BuyingCommissionPercentage = viewModel.BuyingCommissionPercentage;
            entity.ListingAgentName = viewModel.ListingAgentName;
            entity.ListingBrokerage = viewModel.ListingBrokerage;
            entity.ListingBrokerageFax = viewModel.ListingBrokerageFax;
            entity.TenantAgentName = viewModel.TenantAgentName;
            entity.TenantEmailAddress = viewModel.TenantEmailAddress;
            entity.TenantBrokerage = viewModel.TenantBrokerage;
            entity.TenantBrokerageFax = viewModel.TenantBrokerageFax;
            entity.TenantPhoneNumber = viewModel.TenantPhoneNumber;
            entity.LandLordPhoneNumber = viewModel.LandLordPhoneNumber;
            entity.LandLordEmailAddress = viewModel.LandLordEmailAddress;
            entity.IsActive = viewModel.IsActive;
            entity.IsDeleted = viewModel.IsDeleted;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        public static void UpdateFromViewModel(this IRFDeal entity, EditIRFDealSaleCoopViewModel viewModel, Guid updatedBy)
        {

            entity.PropertyAddress = viewModel.PropertyAddress;
            entity.FinalSalePrice = viewModel.FinalSalePrice;
            entity.FinalClosingDate = viewModel.FinalClosingDate;
            entity.DepositAmount = viewModel.DepositAmount;
            entity.DepositDate = viewModel.DepositDate;
            entity.BuyerName = viewModel.BuyerName;
            entity.LandLordName = viewModel.LandLordName;
            //ListingCommissionPercentage = viewModel.ListingCommissionPercentage;
            entity.BuyingCommissionPercentage = viewModel.BuyingCommissionPercentage;
            entity.ListingAgentName = viewModel.ListingAgentName;
            entity.ListingBrokerage = viewModel.ListingBrokerage;
            entity.ListingBrokerageFax = viewModel.ListingBrokerageFax;
            entity.BuyerAgentName = viewModel.BuyerAgentName;
            entity.BuyerBrokerage = viewModel.BuyerBrokerage;
            entity.BuyerBrokerageFax = viewModel.BuyerBrokerageFax;
            entity.SellersLawyer = viewModel.SellersLawyer;
            entity.SellersLawyerAddress = viewModel.SellersLawyerAddress;
            entity.SellersPhoneNumber = viewModel.SellersPhoneNumber;
            entity.BuyersLawyer = viewModel.BuyersLawyer;
            entity.BuyersLawyerAddress = viewModel.BuyersLawyerAddress;
            entity.BuyersPhoneNumber = viewModel.BuyersPhoneNumber;

            entity.IsActive = viewModel.IsActive;
            entity.IsDeleted = viewModel.IsDeleted;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        public static void UpdateFromViewModel(this IRFDeal entity, EditIRFDealSaleListingViewModel viewModel, Guid updatedBy)
        {

            entity.PropertyAddress = viewModel.PropertyAddress;
            entity.FinalSalePrice = viewModel.FinalSalePrice;
            entity.FinalClosingDate = viewModel.FinalClosingDate;
            entity.DepositAmount = viewModel.DepositAmount;
            entity.DepositDate = viewModel.DepositDate;
            entity.BuyerName = viewModel.BuyerName;
            entity.LandLordName = viewModel.LandLordName;
            entity.ListingCommissionPercentage = viewModel.ListingCommissionPercentage;
            entity.BuyingCommissionPercentage = viewModel.BuyingCommissionPercentage;
            entity.ListingAgentName = viewModel.ListingAgentName;
            entity.ListingBrokerage = viewModel.ListingBrokerage;
            entity.ListingBrokerageFax = viewModel.ListingBrokerageFax;
            entity.BuyerAgentName = viewModel.BuyerAgentName;
            entity.BuyerBrokerage = viewModel.BuyerBrokerage;
            entity.BuyerBrokerageFax = viewModel.BuyerBrokerageFax;
            entity.SellersLawyer = viewModel.SellersLawyer;
            entity.SellersLawyerAddress = viewModel.SellersLawyerAddress;
            entity.SellersPhoneNumber = viewModel.SellersPhoneNumber;
            entity.BuyersLawyer = viewModel.BuyersLawyer;
            entity.BuyersLawyerAddress = viewModel.BuyersLawyerAddress;
            entity.BuyersPhoneNumber = viewModel.BuyersPhoneNumber;

            entity.IsActive = viewModel.IsActive;
            entity.IsDeleted = viewModel.IsDeleted;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}