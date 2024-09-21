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
        public static EditIRFDealViewModel ToEditIRFDealViewModel(this IRFDeal model)
        {
            return new EditIRFDealViewModel
            {
                Id = model.Id,

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
                UpdatedBy = model.UpdatedBy,
                UpdatedAt = model.UpdatedAt
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
        public static IRFDeal ToIRFDealModel(this CreateIRFDealViewModel viewModel)
        {
            return new IRFDeal
            {
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
        public static IRFDeal ToIRFDealModel(this EditIRFDealViewModel viewModel)
        {
            return new IRFDeal
            {
                Id = viewModel.Id,

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
    }
}