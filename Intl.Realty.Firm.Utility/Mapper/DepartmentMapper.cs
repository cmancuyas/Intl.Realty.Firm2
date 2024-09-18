using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.DepartmentVM;

namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class DepartmentMapper
    {
        public static IEnumerable<DeleteDepartmentViewModel> ToDeleteDepartmentIEnumViewModel(this IEnumerable<Department> modelIEnum)
        {
            IEnumerable<DeleteDepartmentViewModel> viewModelIEnum = new List<DeleteDepartmentViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DeleteDepartmentViewModel()
                {
                    Id = x.Id,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy,
                });
            }
            return viewModelIEnum;
        }
        public static EditDepartmentViewModel ToEditDepartmentViewModel(this Department model)
        {
            return new EditDepartmentViewModel
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
        public static IEnumerable<DepartmentViewModel> ToDepartmentIEnumViewModel(this IEnumerable<Department> modelIEnum)
        {
            IEnumerable<DepartmentViewModel> viewModelIEnum = new List<DepartmentViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DepartmentViewModel()
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
        public static Department ToDepartmentModel(this CreateDepartmentViewModel viewModel)
        {
            return new Department
            {
                Code = viewModel.Code,
                Description = viewModel.Description,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static Department ToDepartmentModel(this EditDepartmentViewModel viewModel)
        {
            return new Department
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
        public static List<DepartmentExport> ToDepartmentListExport(this List<Department> modelList)
        {
            List<DepartmentExport> exportList = new List<DepartmentExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new DepartmentExport
                {
                    Code = x.Code,
                    Description= x.Description,
                }).ToList();
            }
            return exportList;
        }

        public static PreviewDepartmentViewModel ToPreviewDepartmentViewModel(this Department model)
        {
            return new PreviewDepartmentViewModel
            {
                Id = model.Id,

                Code = model.Code,
                Description = model.Description,
            };

        }
    }
}