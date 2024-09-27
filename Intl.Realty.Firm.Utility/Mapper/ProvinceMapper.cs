using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.ProvinceVM;

namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class ProvinceMapper
    {
        public static IEnumerable<DeleteProvinceViewModel> ToDeleteProvinceIEnumViewModel(this IEnumerable<Province> modelIEnum)
        {
            IEnumerable<DeleteProvinceViewModel> viewModelIEnum = new List<DeleteProvinceViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DeleteProvinceViewModel()
                {
                    Id = x.Id,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy,
                });
            }
            return viewModelIEnum;
        }
        public static EditProvinceViewModel ToEditProvinceViewModel(this Province model)
        {
            return new EditProvinceViewModel
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
        public static IEnumerable<ProvinceViewModel> ToProvinceIEnumViewModel(this IEnumerable<Province> modelIEnum)
        {
            IEnumerable<ProvinceViewModel> viewModelIEnum = new List<ProvinceViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new ProvinceViewModel()
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
        public static Province ToProvinceModel(this CreateProvinceViewModel viewModel)
        {
            return new Province
            {
                Code = viewModel.Code,
                Description = viewModel.Description,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static Province ToProvinceModel(this EditProvinceViewModel viewModel)
        {
            return new Province
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
        public static List<ProvinceExport> ToProvinceListExport(this List<Province> modelList)
        {
            List<ProvinceExport> exportList = new List<ProvinceExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new ProvinceExport
                {
                    Code = x.Code,
                    Description= x.Description,
                }).ToList();
            }
            return exportList;
        }

        public static PreviewProvinceViewModel ToPreviewProvinceViewModel(this Province model)
        {
            return new PreviewProvinceViewModel
            {
                Id = model.Id,

                Code = model.Code,
                Description = model.Description,
            };

        }
    }
}