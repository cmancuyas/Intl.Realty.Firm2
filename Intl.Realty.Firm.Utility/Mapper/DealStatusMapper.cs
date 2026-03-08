using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.DealStatusVM;

namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class DealStatusMapper
    {
        public static IEnumerable<DeleteDealStatusViewModel> ToDeleteDealStatusIEnumViewModel(this IEnumerable<DealStatus> modelIEnum)
        {
            IEnumerable<DeleteDealStatusViewModel> viewModelIEnum = new List<DeleteDealStatusViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DeleteDealStatusViewModel()
                {
                    Id = x.Id,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy,
                });
            }
            return viewModelIEnum;
        }
        public static EditDealStatusViewModel ToEditDealStatusViewModel(this DealStatus model)
        {
            return new EditDealStatusViewModel
            {
                Id = model.Id,

                Code = model.Code,
                Description = model.Description,

                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                UpdatedBy = model.UpdatedBy,
                UpdatedAt = model.UpdatedAt
            };
        }
        public static IEnumerable<DealStatusViewModel> ToDealStatusIEnumViewModel(this IEnumerable<DealStatus> modelIEnum)
        {
            IEnumerable<DealStatusViewModel> viewModelIEnum = new List<DealStatusViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DealStatusViewModel()
                {
                    Id = x.Id,

                    Code = x.Code,
                    Description= x.Description,

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
        public static DealStatus ToDealStatusModel(this CreateDealStatusViewModel viewModel)
        {
            return new DealStatus
            {
                Code = viewModel.Code,
                Description = viewModel.Description,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static DealStatus ToDealStatusModel(this EditDealStatusViewModel viewModel)
        {
            return new DealStatus
            {
                Id = viewModel.Id,

                Code = viewModel.Code,
                Description = viewModel.Description,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedAt = viewModel.UpdatedAt,
            };
        }
        public static List<DealStatusExport> ToDealStatusListExport(this List<DealStatus> modelList)
        {
            List<DealStatusExport> exportList = new List<DealStatusExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new DealStatusExport
                {
                    Code = x.Code,
                    Description= x.Description,
                }).ToList();
            }
            return exportList;
        }

        public static PreviewDealStatusViewModel ToPreviewDealStatusViewModel(this DealStatus model)
        {
            return new PreviewDealStatusViewModel
            {
                Id = model.Id,

                Code = model.Code,
                Description = model.Description,
            };

        }
    }
}