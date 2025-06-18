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
            var viewModel = new EditIRFDealSaleListingViewModel
            {
                Id = model.Id,
                PropertyAddress = model.PropertyAddress,
                FinalSalePrice = model.FinalSalePrice ?? 0,
                FinalClosingDate = model.FinalClosingDate,
                DepositAmount = model.DepositAmount,
                DepositDate = model.DepositDate,
                Commission = new()
                {
                    ListingCommissionPercentage = model.ListingCommissionPercentage,
                    BuyingCommissionPercentage = model.BuyingCommissionPercentage
                },
                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                UpdatedBy = model.UpdatedBy ?? Guid.Empty,
                UpdatedAt = model.UpdatedAt ?? DateTime.Now,
                BuyerDetails = new()
                {
                    BuyerName = model.BuyerName ?? string.Empty,
                    BuyerAgentName = model.BuyerAgentName ?? string.Empty,
                    BuyerBrokerage = model.BuyerBrokerage ?? string.Empty,
                    BuyerBrokerageFax = model.BuyerBrokerageFax ?? string.Empty,
                    BuyerLawyer = model.BuyerLawyer ?? string.Empty,
                    BuyerLawyerAddress = model.BuyerLawyerAddress ?? string.Empty,
                    BuyerLawyerEmail = model.BuyerLawyerEmail ?? string.Empty,
                    BuyerLawyerPhone = model.BuyerLawyerPhone ?? string.Empty
                },
                SellerDetails = new()
                {
                    SellerName = model.SellerName ?? string.Empty,
                    SellerAgentName = model.SellerAgentName ?? string.Empty,
                    SellerLawyer = model.SellerLawyer ?? string.Empty,
                    SellerLawyerAddress = model.SellerLawyerAddress ?? string.Empty,
                    SellerEmail = model.SellerEmail ?? string.Empty,
                    SellerLawyerEmail = model.SellerLawyerEmail ?? string.Empty,
                    SellerLawyerPhone = model.SellerLawyerPhone ?? string.Empty,
                    SellerBrokerage = model.SellerBrokerage ?? string.Empty,    
                    SellerBrokerageEmail = model.SellerBrokerageEmail ?? string.Empty,
                    SellerBrokerageFax = model.SellerBrokerageFax ?? string.Empty,
                    SellerBrokeragePhoneNumber = model.SellerBrokeragePhoneNumber ?? string.Empty,
                }
            };

            return viewModel;
        }

        public static EditIRFDealSaleCoopViewModel ToEditIRFDealSaleCoopViewModel(this IRFDeal model)
        {
            return new EditIRFDealSaleCoopViewModel
            {
                Id = model.Id,
                PropertyAddress = model.PropertyAddress,
                FinalSalePrice = model.FinalSalePrice ?? 0,
                FinalClosingDate = model.FinalClosingDate,
                DepositAmount = model.DepositAmount,
                DepositDate = model.DepositDate,
                Commission = new()
                {
                    BuyingCommissionPercentage = model.BuyingCommissionPercentage
                },
                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                UpdatedBy = model.UpdatedBy ?? Guid.Empty,
                UpdatedAt = model.UpdatedAt ?? DateTime.Now,

                BuyerDetails = new()
                {
                    BuyerName = model.BuyerName ?? string.Empty,
                    BuyerAgentName = model.BuyerAgentName ?? string.Empty,
                    BuyerBrokerage = model.BuyerBrokerage ?? string.Empty,
                    BuyerBrokerageFax = model.BuyerBrokerageFax ?? string.Empty,
                    BuyerEmail = model.BuyerEmail ?? string.Empty,
                    BuyerLawyer = model.BuyerLawyer ?? string.Empty,
                    BuyerLawyerAddress = model.BuyerLawyerAddress ?? string.Empty,
                    BuyerLawyerEmail = model.BuyerLawyerEmail ?? string.Empty,
                    BuyerLawyerPhone = model.BuyerLawyerPhone ?? string.Empty
                },
                SellerDetails = new()
                {
                    SellerName = model.SellerName ?? string.Empty,
                    SellerAgentName = model.SellerAgentName ?? string.Empty,
                    SellerLawyer = model.SellerLawyer ?? string.Empty,
                    SellerLawyerAddress = model.SellerLawyerAddress ?? string.Empty,
                    SellerLawyerEmail = model.SellerLawyerEmail ?? string.Empty,
                    SellerLawyerPhone = model.SellerLawyerPhone ?? string.Empty,
                    SellerBrokerage = model.SellerBrokerage ?? string.Empty,
                    SellerBrokerageEmail = model.SellerBrokerageEmail ?? string.Empty,
                    SellerBrokerageFax = model.SellerBrokerageFax ?? string.Empty,
                    SellerBrokeragePhoneNumber = model.SellerBrokeragePhoneNumber ?? string.Empty,
                }
            };
        }

        public static EditIRFDealLeaseListingViewModel ToEditIRFDealLeaseListingViewModel(this IRFDeal model)
        {
            return new EditIRFDealLeaseListingViewModel
            {
                Id = model.Id,
                PropertyAddress = model.PropertyAddress,
                FinalLeasePrice = model.FinalLeasePrice ?? 0,
                FinalClosingDate = model.FinalClosingDate,
                DepositAmount = model.DepositAmount,
                DepositDate = model.DepositDate,

                // 🧑‍💼 Tenant Info
                TenantDetails = new()
                {
                    TenantName = model.TenantName ?? string.Empty,
                    TenantAgentName = model.TenantAgentName ?? string.Empty,
                    TenantBrokerage = model.TenantBrokerage ?? string.Empty,
                    TenantBrokerageFax = model.TenantBrokerageFax ?? string.Empty,
                    TenantPhoneNumber = model.TenantPhoneNumber ?? string.Empty,
                    TenantEmailAddress = model.TenantEmailAddress ?? string.Empty
                },

                // 🏠 Landlord Info
                LandLordDetails = new()
                {
                    LandLordName = model.LandLordName ?? string.Empty,
                    LandLordPhoneNumber = model.LandLordPhoneNumber ?? string.Empty,
                    LandLordEmailAddress = model.LandLordEmailAddress ?? string.Empty
                },

                // 💼 Commission Info
                Commission = new()
                {
                    ListingCommissionPercentage = model.ListingCommissionPercentage,
                    BuyingCommissionPercentage = model.BuyingCommissionPercentage
                },

                // 📋 Listing Info
                ListingInfo = new()
                {
                    ListingAgentName = model.ListingAgentName ?? string.Empty,
                    ListingBrokerage = model.ListingBrokerage ?? string.Empty,
                    ListingBrokerageFax = model.ListingBrokerageFax ?? string.Empty
                },

                // 🕵️‍♀️ Audit
                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                UpdatedBy = model.UpdatedBy ?? Guid.Empty,
                UpdatedAt = model.UpdatedAt ?? DateTime.UtcNow
            };
        }


        public static EditIRFDealLeaseCoopViewModel ToEditIRFDealLeaseCoopViewModel(this IRFDeal model)
        {
            return new EditIRFDealLeaseCoopViewModel
            {
                Id = model.Id,
                PropertyAddress = model.PropertyAddress,
                FinalLeasePrice = model.FinalLeasePrice ?? 0,
                FinalClosingDate = model.FinalClosingDate,
                DepositAmount = model.DepositAmount,
                DepositDate = model.DepositDate,

                // 👤 Tenant
                TenantDetails = new()
                {
                    TenantName = model.TenantName ?? string.Empty,
                    TenantAgentName = model.TenantAgentName ?? string.Empty,
                    TenantBrokerage = model.TenantBrokerage ?? string.Empty,
                    TenantBrokerageFax = model.TenantBrokerageFax ?? string.Empty,
                    TenantPhoneNumber = model.TenantPhoneNumber ?? string.Empty,
                    TenantEmailAddress = model.TenantEmailAddress ?? string.Empty
                },

                // 👤 Landlord
                LandLordDetails = new()
                {
                    LandLordName = model.LandLordName ?? string.Empty,
                    LandLordPhoneNumber = model.LandLordPhoneNumber ?? string.Empty,
                    LandLordEmailAddress = model.LandLordEmailAddress ?? string.Empty
                },
                Commission = new()
                {
                    BuyingCommissionPercentage = model.BuyingCommissionPercentage
                },

                // ✅ Status
                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                UpdatedBy = model.UpdatedBy ?? Guid.Empty,
                UpdatedAt = model.UpdatedAt ?? DateTime.UtcNow
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

                    SellerName = x.SellerName,
                    SellerAgentName = x.SellerAgentName,
                    SellerBrokerageEmail = x.SellerBrokerageEmail,
                    SellerBrokerage = x.SellerBrokerage,
                    SellerBrokerageFax = x.SellerBrokerageFax,
                    SellerBrokeragePhoneNumber = x.SellerBrokeragePhoneNumber,

                    SellerLawyer = x.SellerLawyer,
                    SellerLawyerAddress = x.SellerLawyerAddress,
                    SellerEmail = x.SellerEmail,
                    SellerLawyerPhone = x.SellerLawyerPhone,
                    SellerLawyerEmail = x.SellerLawyerEmail,

                    BuyerAgentName = x.BuyerAgentName,
                    BuyerBrokerage = x.BuyerBrokerage,
                    BuyerBrokerageFax = x.BuyerBrokerageFax,
                    BuyerBrokerageEmail = x.BuyerBrokerageEmail,
                    BuyerBrokeragePhoneNumber = x.BuyerBrokeragePhoneNumber,

                    BuyerLawyer = x.BuyerLawyer,
                    BuyerLawyerAddress = x.BuyerLawyerAddress,
                    BuyerEmail = x.BuyerEmail,
                    BuyerLawyerPhone = x.BuyerLawyerPhone,
                    BuyerLawyerEmail = x.BuyerLawyerEmail,

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
                PropertyAddress = viewModel.PropertyAddress,
                FinalSalePrice = viewModel.FinalSalePrice,
                FinalClosingDate = viewModel.FinalClosingDate,
                DepositAmount = viewModel.DepositAmount,
                DepositDate = viewModel.DepositDate,

                // 👤 Buyer
                BuyerName = viewModel.BuyerDetails.BuyerName,
                BuyerAgentName = viewModel.BuyerDetails.BuyerAgentName,
                BuyerBrokerage = viewModel.BuyerDetails.BuyerBrokerage,
                BuyerBrokerageFax = viewModel.BuyerDetails.BuyerBrokerageFax,
                BuyerBrokerageEmail = viewModel.BuyerDetails.BuyerBrokerageEmail,
                BuyerBrokeragePhoneNumber = viewModel.BuyerDetails.BuyerBrokeragePhoneNumber,

                BuyerLawyer = viewModel.BuyerDetails.BuyerLawyer,
                BuyerLawyerAddress = viewModel.BuyerDetails.BuyerLawyerAddress,
                BuyerLawyerEmail = viewModel.BuyerDetails.BuyerLawyerEmail,
                BuyerLawyerPhone = viewModel.BuyerDetails.BuyerLawyerPhone,

                // 👤 Seller
                SellerName = viewModel.SellerDetails.SellerName,
                SellerAgentName = viewModel.SellerDetails.SellerAgentName,
                SellerLawyer = viewModel.SellerDetails.SellerLawyer,
                SellerLawyerAddress = viewModel.SellerDetails.SellerLawyerAddress,
                SellerEmail = viewModel.SellerDetails.SellerEmail,
                SellerLawyerEmail = viewModel.SellerDetails.SellerLawyerEmail,
                SellerLawyerPhone = viewModel.SellerDetails.SellerLawyerPhone ,
                SellerBrokerage = viewModel.SellerDetails.SellerBrokerage,
                SellerBrokerageEmail = viewModel.SellerDetails.SellerBrokerageEmail,
                SellerBrokerageFax = viewModel.SellerDetails.SellerBrokerageFax,
                SellerBrokeragePhoneNumber = viewModel.SellerDetails.SellerBrokeragePhoneNumber,
                // 💼 Commission

                ListingCommissionPercentage = viewModel.Commission.ListingCommissionPercentage,
                BuyingCommissionPercentage = viewModel.Commission.BuyingCommissionPercentage,

                // 🧾 Metadata
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
                PropertyAddress = viewModel.PropertyAddress,
                FinalSalePrice = viewModel.FinalSalePrice,
                FinalClosingDate = viewModel.FinalClosingDate,
                DepositAmount = viewModel.DepositAmount,
                DepositDate = viewModel.DepositDate,

                // 👤 Buyer
                BuyerName = viewModel.BuyerDetails.BuyerName,
                BuyerAgentName = viewModel.BuyerDetails.BuyerAgentName,
                BuyerBrokerage = viewModel.BuyerDetails.BuyerBrokerage,
                BuyerBrokerageFax = viewModel.BuyerDetails.BuyerBrokerageFax,
                BuyerBrokerageEmail = viewModel.BuyerDetails.BuyerBrokerageEmail,
                BuyerBrokeragePhoneNumber = viewModel.BuyerDetails.BuyerBrokeragePhoneNumber,

                BuyerLawyer = viewModel.BuyerDetails.BuyerLawyer,
                BuyerLawyerAddress = viewModel.BuyerDetails.BuyerLawyerAddress,
                BuyerEmail = viewModel.BuyerDetails.BuyerEmail,
                BuyerLawyerEmail = viewModel.BuyerDetails.BuyerLawyerEmail,
                BuyerLawyerPhone = viewModel.BuyerDetails.BuyerLawyerPhone,

                // 👤 Seller
                SellerName = viewModel.SellerDetails.SellerName,
                SellerAgentName = viewModel.SellerDetails.SellerAgentName,
                SellerLawyer = viewModel.SellerDetails.SellerLawyer,
                SellerLawyerAddress = viewModel.SellerDetails.SellerLawyerAddress,
                SellerLawyerEmail = viewModel.SellerDetails.SellerLawyerEmail,
                SellerLawyerPhone = viewModel.SellerDetails.SellerLawyerPhone,
                SellerBrokerage = viewModel.SellerDetails.SellerBrokerage,
                SellerBrokerageEmail = viewModel.SellerDetails.SellerBrokerageEmail,
                SellerBrokerageFax = viewModel.SellerDetails.SellerBrokerageFax,
                SellerBrokeragePhoneNumber = viewModel.SellerDetails.SellerBrokeragePhoneNumber,

                // 💼 Commission (Only Buying)
                BuyingCommissionPercentage = viewModel.Commission.BuyingCommissionPercentage,

                // 🧾 Metadata
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
                PropertyAddress = viewModel.PropertyAddress,
                FinalLeasePrice = viewModel.FinalLeasePrice,
                FinalClosingDate = viewModel.FinalClosingDate,
                DepositAmount = viewModel.DepositAmount,
                DepositDate = viewModel.DepositDate,

                // 👤 Tenant
                TenantName = viewModel.TenantDetails.TenantName,
                TenantAgentName = viewModel.TenantDetails.TenantAgentName,
                TenantBrokerage = viewModel.TenantDetails.TenantBrokerage,
                TenantBrokerageFax = viewModel.TenantDetails.TenantBrokerageFax,
                TenantPhoneNumber = viewModel.TenantDetails.TenantPhoneNumber,
                TenantEmailAddress = viewModel.TenantDetails.TenantEmailAddress,

                // 👤 Landlord
                LandLordName = viewModel.LandLordDetails.LandLordName,
                LandLordPhoneNumber = viewModel.LandLordDetails.LandLordPhoneNumber,
                LandLordEmailAddress = viewModel.LandLordDetails.LandLordEmailAddress,

                // 🏢 Listing Info
                ListingAgentName = viewModel.ListingInfo.ListingAgentName,
                ListingBrokerage = viewModel.ListingInfo.ListingBrokerage,
                ListingBrokerageFax = viewModel.ListingInfo.ListingBrokerageFax,

                // 💼 Commission Info
                ListingCommissionPercentage = viewModel.Commission.ListingCommissionPercentage,
                BuyingCommissionPercentage = viewModel.Commission.BuyingCommissionPercentage,

                // 📌 Metadata
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
                TenantName = viewModel.TenantDetails.TenantName,
                LandLordName = viewModel.LandLordDetails.LandLordName,
                //ListingCommissionPercentage = viewModel.ListingCommissionPercentage,
                BuyingCommissionPercentage = viewModel.Commission.BuyingCommissionPercentage,
                ListingAgentName = viewModel.ListingInfo.ListingAgentName,
                ListingBrokerage = viewModel.ListingInfo.ListingBrokerage,
                ListingBrokerageFax = viewModel.ListingInfo.ListingBrokerageFax,
                TenantAgentName = viewModel.TenantDetails.TenantAgentName,
                TenantBrokerage = viewModel.TenantDetails.TenantBrokerage,
                TenantBrokerageFax = viewModel.TenantDetails.TenantBrokerageFax,

                TenantPhoneNumber = viewModel.TenantDetails.TenantPhoneNumber ?? "",
                LandLordPhoneNumber = viewModel.LandLordDetails.LandLordPhoneNumber ?? "",
                LandLordEmailAddress = viewModel.LandLordDetails.LandLordEmailAddress ?? "",

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
                BuyerName = viewModel.BuyerDetails.BuyerName,
                ListingCommissionPercentage = viewModel.Commission.ListingCommissionPercentage,
                BuyingCommissionPercentage = viewModel.Commission.BuyingCommissionPercentage,

                // 👤 Buyer
                BuyerAgentName = viewModel.BuyerDetails.BuyerAgentName,
                BuyerBrokerage = viewModel.BuyerDetails.BuyerBrokerage,
                BuyerBrokerageFax = viewModel.BuyerDetails.BuyerBrokerageFax,
                BuyerBrokerageEmail = viewModel.BuyerDetails.BuyerBrokerageEmail,
                BuyerBrokeragePhoneNumber = viewModel.BuyerDetails.BuyerBrokeragePhoneNumber,

                BuyerLawyer = viewModel.BuyerDetails.BuyerLawyer,
                BuyerLawyerAddress = viewModel.BuyerDetails.BuyerLawyerAddress,
                BuyerLawyerEmail = viewModel.BuyerDetails.BuyerLawyerEmail,
                BuyerLawyerPhone = viewModel.BuyerDetails.BuyerLawyerPhone,

                // 👤 Seller
                SellerName = viewModel.SellerDetails.SellerName,
                SellerAgentName = viewModel.SellerDetails.SellerAgentName,
                SellerLawyer = viewModel.SellerDetails.SellerLawyer,
                SellerLawyerAddress = viewModel.SellerDetails.SellerLawyerAddress,
                SellerEmail = viewModel.SellerDetails.SellerEmail,
                SellerLawyerEmail = viewModel.SellerDetails.SellerLawyerEmail,
                SellerLawyerPhone = viewModel.SellerDetails.SellerLawyerPhone,
                SellerBrokerage = viewModel.SellerDetails.SellerBrokerage,
                SellerBrokerageEmail = viewModel.SellerDetails.SellerBrokerageEmail,
                SellerBrokerageFax = viewModel.SellerDetails.SellerBrokerageFax,
                SellerBrokeragePhoneNumber = viewModel.SellerDetails.SellerBrokeragePhoneNumber,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedAt = viewModel.UpdatedAt,
            };
        }
        public static IRFDeal ToIRFDealModel(this EditIRFDealSaleCoopViewModel viewModel)
        {
            return new IRFDeal
            {
                Id = viewModel.Id,
                PropertyAddress = viewModel.PropertyAddress,
                FinalSalePrice = viewModel.FinalSalePrice,
                FinalClosingDate = viewModel.FinalClosingDate,
                DepositAmount = viewModel.DepositAmount,
                DepositDate = viewModel.DepositDate,

                // 👤 Buyer
                BuyerName = viewModel.BuyerDetails.BuyerName,
                BuyerAgentName = viewModel.BuyerDetails.BuyerAgentName,
                BuyerBrokerage = viewModel.BuyerDetails.BuyerBrokerage,
                BuyerBrokerageFax = viewModel.BuyerDetails.BuyerBrokerageFax,
                BuyerBrokerageEmail = viewModel.BuyerDetails.BuyerBrokerageEmail,
                BuyerBrokeragePhoneNumber = viewModel.BuyerDetails.BuyerBrokeragePhoneNumber,

                BuyerLawyer = viewModel.BuyerDetails.BuyerLawyer,
                BuyerLawyerAddress = viewModel.BuyerDetails.BuyerLawyerAddress,
                BuyerEmail = viewModel.BuyerDetails.BuyerEmail,
                BuyerLawyerEmail = viewModel.BuyerDetails.BuyerLawyerEmail,
                BuyerLawyerPhone = viewModel.BuyerDetails.BuyerLawyerPhone,

                // 👤 Seller
                SellerName = viewModel.SellerDetails.SellerName,
                SellerAgentName = viewModel.SellerDetails.SellerAgentName,
                SellerLawyer = viewModel.SellerDetails.SellerLawyer,
                SellerLawyerAddress = viewModel.SellerDetails.SellerLawyerAddress,
                SellerLawyerEmail = viewModel.SellerDetails.SellerLawyerEmail,
                SellerLawyerPhone = viewModel.SellerDetails.SellerLawyerPhone,
                SellerBrokerage = viewModel.SellerDetails.SellerBrokerage,
                SellerBrokerageEmail = viewModel.SellerDetails.SellerBrokerageEmail,
                SellerBrokerageFax = viewModel.SellerDetails.SellerBrokerageFax,
                SellerBrokeragePhoneNumber = viewModel.SellerDetails.SellerBrokeragePhoneNumber,

                // 💼 Commission (only buying for SaleCoop)
                BuyingCommissionPercentage = viewModel.Commission.BuyingCommissionPercentage,

                // 📌 Audit
                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedAt = viewModel.UpdatedAt
            };
        }

        public static IRFDeal ToIRFDealModel(this EditIRFDealLeaseListingViewModel viewModel)
        {
            return new IRFDeal
            {
                Id = viewModel.Id,
                PropertyAddress = viewModel.PropertyAddress,
                FinalLeasePrice = viewModel.FinalLeasePrice,
                FinalClosingDate = viewModel.FinalClosingDate,
                DepositAmount = viewModel.DepositAmount,
                DepositDate = viewModel.DepositDate,

                // 👤 Tenant Info
                TenantName = viewModel.TenantDetails.TenantName,
                TenantAgentName = viewModel.TenantDetails.TenantAgentName,
                TenantBrokerage = viewModel.TenantDetails.TenantBrokerage,
                TenantBrokerageFax = viewModel.TenantDetails.TenantBrokerageFax,
                TenantPhoneNumber = viewModel.TenantDetails.TenantPhoneNumber,
                TenantEmailAddress = viewModel.TenantDetails.TenantEmailAddress,

                // 🧑‍💼 Landlord Info
                LandLordName = viewModel.LandLordDetails.LandLordName,
                LandLordPhoneNumber = viewModel.LandLordDetails.LandLordPhoneNumber,
                LandLordEmailAddress = viewModel.LandLordDetails.LandLordEmailAddress,

                // 📋 Listing Info
                ListingAgentName = viewModel.ListingInfo.ListingAgentName,
                ListingBrokerage = viewModel.ListingInfo.ListingBrokerage,
                ListingBrokerageFax = viewModel.ListingInfo.ListingBrokerageFax,

                // 💼 Commission Info
                ListingCommissionPercentage = viewModel.Commission.ListingCommissionPercentage,
                BuyingCommissionPercentage = viewModel.Commission.BuyingCommissionPercentage,

                // ✅ Audit
                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedAt = viewModel.UpdatedAt,
            };
        }

        public static IRFDeal ToIRFDealModel(this EditIRFDealLeaseCoopViewModel viewModel)
        {
            return new IRFDeal
            {
                Id = viewModel.Id,
                PropertyAddress = viewModel.PropertyAddress,
                FinalLeasePrice = viewModel.FinalLeasePrice,
                FinalClosingDate = viewModel.FinalClosingDate,
                DepositAmount = viewModel.DepositAmount,
                DepositDate = viewModel.DepositDate,

                // 👤 Tenant Info
                TenantName = viewModel.TenantDetails.TenantName,
                TenantAgentName = viewModel.TenantDetails.TenantAgentName,
                TenantBrokerage = viewModel.TenantDetails.TenantBrokerage,
                TenantBrokerageFax = viewModel.TenantDetails.TenantBrokerageFax,
                TenantPhoneNumber = viewModel.TenantDetails.TenantPhoneNumber,
                TenantEmailAddress = viewModel.TenantDetails.TenantEmailAddress,

                // 🧑‍💼 Landlord Info
                LandLordName = viewModel.LandLordDetails.LandLordName,
                LandLordPhoneNumber = viewModel.LandLordDetails.LandLordPhoneNumber,
                LandLordEmailAddress = viewModel.LandLordDetails.LandLordEmailAddress,

                // 💼 Commission Info (only buying for LeaseCoop)
                BuyingCommissionPercentage = viewModel.Commission.BuyingCommissionPercentage,

                // ListingInfo is ignored here as it's not required in LeaseCoop

                // ✅ Audit
                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedAt = viewModel.UpdatedAt,
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

                    SellerName = x.SellerName,
                    SellerAgentName = x.SellerAgentName,

                    SellerLawyer = x.SellerLawyer,
                    SellerLawyerAddress = x.SellerLawyerAddress,
                    SellerEmail = x.SellerEmail,
                    SellerLawyerEmail = x.SellerLawyerEmail,
                    SellerLawyerPhone = x.SellerLawyerPhone,
                    SellerBrokerage = x.SellerBrokerage,
                    SellerBrokerageEmail = x.SellerBrokerageEmail,
                    SellerBrokerageFax = x.SellerBrokerageFax,
                    SellerBrokeragePhoneNumber = x.SellerBrokeragePhoneNumber,
                    
                    BuyerAgentName = x.BuyerAgentName,
                    BuyerBrokerage = x.BuyerBrokerage,
                    BuyerBrokerageFax = x.BuyerBrokerageFax,

                    BuyerLawyer = x.BuyerLawyer,
                    BuyerLawyerAddress = x.BuyerLawyerAddress,
                    BuyerEmail = x.BuyerEmail,
                    BuyerLawyerEmail = x.BuyerLawyerEmail,
                    BuyerLawyerPhone = x.BuyerLawyerPhone,
                    BuyerBrokerageEmail = x.BuyerBrokerageEmail,
                    BuyerBrokeragePhoneNumber = x.BuyerBrokeragePhoneNumber,
                    


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
                BuyerLawyer = model.BuyerLawyer,
                BuyerLawyerAddress = model.BuyerLawyerAddress,
                BuyerEmail = model.BuyerEmail,
                BuyerLawyerEmail = model.BuyerLawyerEmail,
                BuyerLawyerPhone = model.BuyerLawyerPhone,
                BuyerBrokerageEmail = model.BuyerBrokerageEmail,
                BuyerBrokeragePhoneNumber = model.BuyerBrokeragePhoneNumber,

                SellerBrokerageEmail = model.SellerBrokerageEmail,
                SellerBrokerage = model.SellerBrokerage,
                SellerBrokerageFax = model.SellerBrokerageFax,
                SellerBrokeragePhoneNumber = model.SellerBrokeragePhoneNumber,

                SellerName = model.SellerName,
                SellerAgentName = model.SellerAgentName,

                SellerLawyer = model.SellerLawyer,
                SellerLawyerAddress = model.SellerLawyerAddress,
                SellerEmail = model.SellerEmail,
                SellerLawyerEmail = model.SellerLawyerEmail,
                SellerLawyerPhone = model.SellerLawyerPhone,

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
                    BuyerLawyer = x.BuyerLawyer,
                    BuyerLawyerAddress = x.BuyerLawyerAddress,
                    BuyerEmail = x.BuyerEmail,
                    BuyerLawyerEmail = x.BuyerLawyerEmail,
                    BuyerLawyerPhone = x.BuyerLawyerPhone,
                    BuyerBrokerageEmail = x.BuyerBrokerageEmail,
                    BuyerBrokeragePhoneNumber = x.BuyerBrokeragePhoneNumber,

                    SellerName = x.SellerName,
                    SellerAgentName = x.SellerAgentName,
                    SellerLawyer = x.SellerLawyer,
                    SellerLawyerAddress = x.SellerLawyerAddress,
                    SellerEmail = x.SellerEmail,
                    SellerLawyerEmail = x.SellerLawyerEmail,
                    SellerLawyerPhone = x.SellerLawyerPhone,
                    SellerBrokerage = x.SellerBrokerage,
                    SellerBrokerageEmail = x.SellerBrokerageEmail,
                    SellerBrokerageFax = x.SellerBrokerageFax,
                    SellerBrokeragePhoneNumber = x.SellerBrokeragePhoneNumber,


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

            // 👤 Tenant
            entity.TenantName = viewModel.TenantDetails.TenantName;
            entity.TenantAgentName = viewModel.TenantDetails.TenantAgentName;
            entity.TenantBrokerage = viewModel.TenantDetails.TenantBrokerage;
            entity.TenantBrokerageFax = viewModel.TenantDetails.TenantBrokerageFax;
            entity.TenantPhoneNumber = viewModel.TenantDetails.TenantPhoneNumber;
            entity.TenantEmailAddress = viewModel.TenantDetails.TenantEmailAddress;

            // 🧑‍💼 Landlord
            entity.LandLordName = viewModel.LandLordDetails.LandLordName;
            entity.LandLordPhoneNumber = viewModel.LandLordDetails.LandLordPhoneNumber;
            entity.LandLordEmailAddress = viewModel.LandLordDetails.LandLordEmailAddress;

            // Listing
            entity.ListingAgentName = viewModel.ListingInfo.ListingAgentName;
            entity.BuyerBrokerageFax = viewModel.ListingInfo.ListingBrokerageFax;
            entity.BuyerBrokerage = viewModel.ListingInfo.ListingBrokerage;


            // 💼 Commission (only BuyingCommission is used for LeaseCoop)
            entity.BuyingCommissionPercentage = viewModel.Commission.BuyingCommissionPercentage;

            // ❌ Listing Info not included for LeaseCoop by requirement

            // ✅ Audit
            entity.IsActive = viewModel.IsActive;
            entity.IsDeleted = viewModel.IsDeleted;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedAt = DateTime.UtcNow;
        }

        public static void UpdateFromViewModel(this IRFDeal entity, EditIRFDealLeaseListingViewModel viewModel, Guid updatedBy)
        {
            // 🏠 Deal Info
            entity.PropertyAddress = viewModel.PropertyAddress;
            entity.FinalLeasePrice = viewModel.FinalLeasePrice;
            entity.FinalClosingDate = viewModel.FinalClosingDate;
            entity.DepositAmount = viewModel.DepositAmount;
            entity.DepositDate = viewModel.DepositDate;

            // 👤 Tenant
            entity.TenantName = viewModel.TenantDetails.TenantName;
            entity.TenantAgentName = viewModel.TenantDetails.TenantAgentName;
            entity.TenantBrokerage = viewModel.TenantDetails.TenantBrokerage;
            entity.TenantBrokerageFax = viewModel.TenantDetails.TenantBrokerageFax;
            entity.TenantPhoneNumber = viewModel.TenantDetails.TenantPhoneNumber;
            entity.TenantEmailAddress = viewModel.TenantDetails.TenantEmailAddress;

            // 🧑‍💼 Landlord
            entity.LandLordName = viewModel.LandLordDetails.LandLordName;
            entity.LandLordPhoneNumber = viewModel.LandLordDetails.LandLordPhoneNumber;
            entity.LandLordEmailAddress = viewModel.LandLordDetails.LandLordEmailAddress;

            // 🏢 Listing
            entity.ListingAgentName = viewModel.ListingInfo.ListingAgentName;
            entity.ListingBrokerage = viewModel.ListingInfo.ListingBrokerage;
            entity.ListingBrokerageFax = viewModel.ListingInfo.ListingBrokerageFax;

            // 💼 Commission
            entity.BuyingCommissionPercentage = viewModel.Commission.BuyingCommissionPercentage;
            entity.ListingCommissionPercentage = viewModel.Commission.ListingCommissionPercentage;

            // ✅ Audit
            entity.IsActive = viewModel.IsActive;
            entity.IsDeleted = viewModel.IsDeleted;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedAt = DateTime.UtcNow;
        }

        public static void UpdateFromViewModel(this IRFDeal entity, EditIRFDealSaleCoopViewModel viewModel, Guid updatedBy)
        {
            // 🏠 Deal Info
            entity.PropertyAddress = viewModel.PropertyAddress;
            entity.FinalSalePrice = viewModel.FinalSalePrice;
            entity.FinalClosingDate = viewModel.FinalClosingDate;
            entity.DepositAmount = viewModel.DepositAmount;
            entity.DepositDate = viewModel.DepositDate;

            // 👤 Buyer
            entity.BuyerName = viewModel.BuyerDetails.BuyerName;
            entity.BuyerAgentName = viewModel.BuyerDetails.BuyerAgentName;
            entity.BuyerBrokerage = viewModel.BuyerDetails.BuyerBrokerage;
            entity.BuyerBrokerageFax = viewModel.BuyerDetails.BuyerBrokerageFax;
            entity.BuyerEmail = viewModel.BuyerDetails.BuyerEmail;
            entity.BuyerLawyer = viewModel.BuyerDetails.BuyerLawyer;
            entity.BuyerLawyerAddress = viewModel.BuyerDetails.BuyerLawyerAddress;
            entity.BuyerLawyerEmail = viewModel.BuyerDetails.BuyerLawyerEmail;
            entity.BuyerLawyerPhone = viewModel.BuyerDetails.BuyerLawyerPhone;
            entity.BuyerBrokerageEmail = viewModel.BuyerDetails.BuyerBrokerageEmail;

            // 🧑‍💼 Seller
            entity.SellerName = viewModel.SellerDetails.SellerName;
            entity.SellerAgentName = viewModel.SellerDetails.SellerAgentName;
            entity.SellerLawyer = viewModel.SellerDetails.SellerLawyer;
            entity.SellerLawyerAddress = viewModel.SellerDetails.SellerLawyerAddress;
            entity.SellerLawyerEmail = viewModel.SellerDetails.SellerLawyerEmail;
            entity.SellerLawyerPhone = viewModel.SellerDetails.SellerLawyerPhone;
            entity.SellerBrokerage = viewModel.SellerDetails.SellerBrokerage;
            entity.SellerBrokerageEmail = viewModel.SellerDetails.SellerBrokerageEmail;
            entity.SellerBrokerageFax = viewModel.SellerDetails.SellerBrokerageFax;
            entity.SellerBrokeragePhoneNumber = viewModel.SellerDetails.SellerBrokeragePhoneNumber;

            // 💼 Commission (only buying for SaleCoop)
            entity.BuyingCommissionPercentage = viewModel.Commission.BuyingCommissionPercentage;

            // ✅ Audit
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
            entity.BuyerName = viewModel.BuyerDetails.BuyerName;
            entity.ListingCommissionPercentage = viewModel.Commission.ListingCommissionPercentage;
            entity.BuyingCommissionPercentage = viewModel.Commission.BuyingCommissionPercentage;

            entity.BuyerAgentName = viewModel.BuyerDetails.BuyerAgentName;
            entity.BuyerBrokerage = viewModel.BuyerDetails.BuyerBrokerage;
            entity.BuyerBrokerageFax = viewModel.BuyerDetails.BuyerBrokerageFax;
            entity.BuyerLawyerEmail = viewModel.BuyerDetails.BuyerLawyerEmail;
            entity.BuyerLawyerPhone = viewModel.BuyerDetails.BuyerLawyerPhone;
            entity.BuyerLawyer = viewModel.BuyerDetails.BuyerLawyer;
            entity.BuyerLawyerAddress = viewModel.BuyerDetails.BuyerLawyerAddress;
            entity.BuyerBrokerageEmail = viewModel.BuyerDetails.BuyerBrokerageEmail;
            entity.BuyerBrokeragePhoneNumber = viewModel.BuyerDetails.BuyerBrokeragePhoneNumber;

            entity.SellerName = viewModel.SellerDetails.SellerName;
            entity.SellerAgentName = viewModel.SellerDetails.SellerAgentName;
            entity.SellerLawyer = viewModel.SellerDetails.SellerLawyer;
            entity.SellerLawyerAddress = viewModel.SellerDetails.SellerLawyerAddress;
            entity.SellerEmail= viewModel.SellerDetails.SellerEmail;
            entity.SellerBrokerage = viewModel.SellerDetails.SellerBrokerage;
            entity.SellerBrokerageEmail = viewModel.SellerDetails.SellerBrokerageEmail;
            entity.SellerBrokerageFax = viewModel.SellerDetails.SellerBrokerageFax;
            entity.SellerBrokeragePhoneNumber = viewModel.SellerDetails.SellerBrokeragePhoneNumber;
            entity.SellerLawyerEmail = viewModel.SellerDetails.SellerLawyerEmail;
            entity.SellerLawyerPhone = viewModel.SellerDetails.SellerLawyerPhone;

            entity.IsActive = viewModel.IsActive;
            entity.IsDeleted = viewModel.IsDeleted;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedAt = DateTime.UtcNow;
        }

    }
}