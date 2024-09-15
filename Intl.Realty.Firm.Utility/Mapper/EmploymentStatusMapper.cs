using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.EmploymentStatusVM;

namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class EmploymentStatusMapper
    {
        public static IEnumerable<DeleteEmploymentStatusViewModel> ToDeleteEmploymentStatusIEnumViewModel(this IEnumerable<EmploymentStatus> modelIEnum)
        {
            IEnumerable<DeleteEmploymentStatusViewModel> viewModelIEnum = new List<DeleteEmploymentStatusViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DeleteEmploymentStatusViewModel()
                {
                    Id = x.Id,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy,
                });
            }
            return viewModelIEnum;
        }
        public static EditEmploymentStatusViewModel ToEditEmploymentStatusViewModel(this EmploymentStatus model)
        {
            return new EditEmploymentStatusViewModel
            {
                Id = model.Id,

                Name = model.Name,
                Description = model.Description,

                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                UpdatedBy = model.UpdatedBy,
                UpdatedAt = model.UpdatedAt
            };
        }
        public static IEnumerable<EmploymentStatusViewModel> ToEmploymentStatusIEnumViewModel(this IEnumerable<EmploymentStatus> modelIEnum)
        {
            IEnumerable<EmploymentStatusViewModel> viewModelIEnum = new List<EmploymentStatusViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new EmploymentStatusViewModel()
                {
                    Id = x.Id,

                    Name = x.Name,
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
        public static EmploymentStatus ToEmploymentStatusModel(this CreateEmploymentStatusViewModel viewModel)
        {
            return new EmploymentStatus
            {
                Name = viewModel.Name,
                Description = viewModel.Description,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static EmploymentStatus ToEmploymentStatusModel(this EditEmploymentStatusViewModel viewModel)
        {
            return new EmploymentStatus
            {
                Id = viewModel.Id,

                Name = viewModel.Name,
                Description = viewModel.Description,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedAt = viewModel.UpdatedAt,
            };
        }
        public static List<EmploymentStatusExport> ToEmploymentStatusListExport(this List<EmploymentStatus> modelList)
        {
            List<EmploymentStatusExport> exportList = new List<EmploymentStatusExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new EmploymentStatusExport
                {
                    Name = x.Name,
                    Description= x.Description,
                }).ToList();
            }
            return exportList;
        }

        public static PreviewEmploymentStatusViewModel ToPreviewEmploymentStatusViewModel(this EmploymentStatus model)
        {
            return new PreviewEmploymentStatusViewModel
            {
                Id = model.Id,

                Name = model.Name,
                Description = model.Description,
            };

        }
    }
}