using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.EmailStatusVM;

namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class EmailStatusMapper
    {
        public static IEnumerable<DeleteEmailStatusViewModel> ToDeleteEmailStatusIEnumViewModel(this IEnumerable<EmailStatus> modelIEnum)
        {
            IEnumerable<DeleteEmailStatusViewModel> viewModelIEnum = new List<DeleteEmailStatusViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DeleteEmailStatusViewModel()
                {
                    Id = x.Id,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy,
                });
            }
            return viewModelIEnum;
        }
        public static EditEmailStatusViewModel ToEditEmailStatusViewModel(this EmailStatus model)
        {
            return new EditEmailStatusViewModel
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
        public static IEnumerable<EmailStatusViewModel> ToEmailStatusIEnumViewModel(this IEnumerable<EmailStatus> modelIEnum)
        {
            IEnumerable<EmailStatusViewModel> viewModelIEnum = new List<EmailStatusViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new EmailStatusViewModel()
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
        public static EmailStatus ToEmailStatusModel(this CreateEmailStatusViewModel viewModel)
        {
            return new EmailStatus
            {
                Code = viewModel.Code,
                Description = viewModel.Description,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static EmailStatus ToEmailStatusModel(this EditEmailStatusViewModel viewModel)
        {
            return new EmailStatus
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
        public static List<EmailStatusExport> ToEmailStatusListExport(this List<EmailStatus> modelList)
        {
            List<EmailStatusExport> exportList = new List<EmailStatusExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new EmailStatusExport
                {
                    Code = x.Code,
                    Description= x.Description,
                }).ToList();
            }
            return exportList;
        }

        public static PreviewEmailStatusViewModel ToPreviewEmailStatusViewModel(this EmailStatus model)
        {
            return new PreviewEmailStatusViewModel
            {
                Id = model.Id,

                Code = model.Code,
                Description = model.Description,
            };

        }
    }
}