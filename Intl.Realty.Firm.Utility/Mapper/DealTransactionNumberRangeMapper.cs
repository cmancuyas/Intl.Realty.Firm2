using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.DealTransactionNumberRangeVM;

namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class DealTransactionNumberRangeMapper
    {
        public static IEnumerable<DeleteDealTransactionNumberRangeViewModel> ToDeleteDealTransactionNumberRangeIEnumViewModel(this IEnumerable<DealTransactionNumberRange> modelIEnum)
        {
            IEnumerable<DeleteDealTransactionNumberRangeViewModel> viewModelIEnum = new List<DeleteDealTransactionNumberRangeViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DeleteDealTransactionNumberRangeViewModel()
                {
                    Id = x.Id,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy,
                });
            }
            return viewModelIEnum;
        }
        public static EditDealTransactionNumberRangeViewModel ToEditDealTransactionNumberRangeViewModel(this DealTransactionNumberRange model)
        {
            return new EditDealTransactionNumberRangeViewModel
            {
                Id = model.Id,

                Year = model.Year,
                CurrentNumber = model.CurrentNumber,
                NumberRange = model.NumberRange,

                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                UpdatedBy = model.UpdatedBy,
                UpdatedAt = model.UpdatedAt
            };
        }
        public static IEnumerable<DealTransactionNumberRangeViewModel> ToDealTransactionNumberRangeIEnumViewModel(this IEnumerable<DealTransactionNumberRange> modelIEnum)
        {
            IEnumerable<DealTransactionNumberRangeViewModel> viewModelIEnum = new List<DealTransactionNumberRangeViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DealTransactionNumberRangeViewModel()
                {
                    Id = x.Id,

                    Year = x.Year,
                    CurrentNumber = x.CurrentNumber,
                    NumberRange = x.NumberRange,

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
        public static DealTransactionNumberRange ToDealTransactionNumberRangeModel(this CreateDealTransactionNumberRangeViewModel viewModel)
        {
            return new DealTransactionNumberRange
            {
                Year = viewModel.Year,
                CurrentNumber = viewModel.CurrentNumber,
                NumberRange = viewModel.NumberRange,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static DealTransactionNumberRange ToDealTransactionNumberRangeModel(this EditDealTransactionNumberRangeViewModel viewModel)
        {
            return new DealTransactionNumberRange
            {
                Id = viewModel.Id,

                Year = viewModel.Year,
                CurrentNumber = viewModel.CurrentNumber,
                NumberRange = viewModel.NumberRange,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedAt = viewModel.UpdatedAt,
            };
        }
        public static List<DealTransactionNumberRangeExport> ToDealTransactionNumberRangeListExport(this List<DealTransactionNumberRange> modelList)
        {
            List<DealTransactionNumberRangeExport> exportList = new List<DealTransactionNumberRangeExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new DealTransactionNumberRangeExport
                {
                    Year = x.Year,
                    CurrentNumber = x.CurrentNumber,
                    NumberRange = x.NumberRange,
                }).ToList();
            }
            return exportList;
        }

        public static PreviewDealTransactionNumberRangeViewModel ToPreviewDealTransactionNumberRangeViewModel(this DealTransactionNumberRange model)
        {
            return new PreviewDealTransactionNumberRangeViewModel
            {
                Id = model.Id,

                Year = model.Year,
                CurrentNumber = model.CurrentNumber,
                NumberRange = model.NumberRange,
            };

        }
    }
}