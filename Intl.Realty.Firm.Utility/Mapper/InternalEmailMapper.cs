using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.InternalEmailVM;

namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class InternalEmailMapper
    {
        public static IEnumerable<DeleteInternalEmailViewModel> ToDeleteInternalEmailIEnumViewModel(this IEnumerable<InternalEmail> modelIEnum)
        {
            IEnumerable<DeleteInternalEmailViewModel> viewModelIEnum = new List<DeleteInternalEmailViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DeleteInternalEmailViewModel()
                {
                    Id = x.Id,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy,
                });
            }
            return viewModelIEnum;
        }
        public static EditInternalEmailViewModel ToEditInternalEmailViewModel(this InternalEmail model)
        {
            return new EditInternalEmailViewModel
            {
                Id = model.Id,

                RoleId = model.RoleId,
                RoleName = model.RoleName,
                Email = model.Email,

                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                UpdatedBy = model.UpdatedBy,
                UpdatedAt = model.UpdatedAt
            };
        }
        public static IEnumerable<InternalEmailViewModel> ToInternalEmailIEnumViewModel(this IEnumerable<InternalEmail> modelIEnum)
        {
            IEnumerable<InternalEmailViewModel> viewModelIEnum = new List<InternalEmailViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new InternalEmailViewModel()
                {
                    Id = x.Id,

                    RoleId = x.RoleId,
                    RoleName = x.RoleName,
                    Email= x.Email,

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
        public static InternalEmail ToInternalEmailModel(this CreateInternalEmailViewModel viewModel)
        {
            return new InternalEmail
            {
                RoleId = viewModel.RoleId,
                RoleName = viewModel.RoleName!,
                Email = viewModel.Email,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static InternalEmail ToInternalEmailModel(this EditInternalEmailViewModel viewModel)
        {
            return new InternalEmail
            {
                Id = viewModel.Id,

                RoleId = viewModel.RoleId,
                RoleName = viewModel.RoleName!,
                Email = viewModel.Email,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedAt = viewModel.UpdatedAt,
            };
        }
        public static List<InternalEmailExport> ToInternalEmailListExport(this List<InternalEmail> modelList)
        {
            List<InternalEmailExport> exportList = new List<InternalEmailExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new InternalEmailExport
                {
                    RoleName = x.RoleName,
                    Email = x.Email,
                }).ToList();
            }
            return exportList;
        }

        public static PreviewInternalEmailViewModel ToPreviewInternalEmailViewModel(this InternalEmail model)
        {
            return new PreviewInternalEmailViewModel
            {
                Id = model.Id,

                RoleId = model.RoleId,
                RoleName = model.RoleName,
                Email = model.Email,
            };

        }
    }
}