using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.EmailCategoryVM;

namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class EmailCategoryMapper
    {
        public static IEnumerable<DeleteEmailCategoryViewModel> ToDeleteEmailCategoryIEnumViewModel(this IEnumerable<EmailCategory> modelIEnum)
        {
            IEnumerable<DeleteEmailCategoryViewModel> viewModelIEnum = new List<DeleteEmailCategoryViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DeleteEmailCategoryViewModel()
                {
                    Id = x.Id,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy,
                });
            }
            return viewModelIEnum;
        }
        public static EditEmailCategoryViewModel ToEditEmailCategoryViewModel(this EmailCategory model)
        {
            return new EditEmailCategoryViewModel
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
        public static IEnumerable<EmailCategoryViewModel> ToEmailCategoryIEnumViewModel(this IEnumerable<EmailCategory> modelIEnum)
        {
            IEnumerable<EmailCategoryViewModel> viewModelIEnum = new List<EmailCategoryViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new EmailCategoryViewModel()
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
        public static EmailCategory ToEmailCategoryModel(this CreateEmailCategoryViewModel viewModel)
        {
            return new EmailCategory
            {
                Code = viewModel.Code,
                Description = viewModel.Description,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static EmailCategory ToEmailCategoryModel(this EditEmailCategoryViewModel viewModel)
        {
            return new EmailCategory
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
        public static List<EmailCategoryExport> ToEmailCategoryListExport(this List<EmailCategory> modelList)
        {
            List<EmailCategoryExport> exportList = new List<EmailCategoryExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new EmailCategoryExport
                {
                    Code = x.Code,
                    Description= x.Description,
                }).ToList();
            }
            return exportList;
        }

        public static PreviewEmailCategoryViewModel ToPreviewEmailCategoryViewModel(this EmailCategory model)
        {
            return new PreviewEmailCategoryViewModel
            {
                Id = model.Id,

                Code = model.Code,
                Description = model.Description,
            };

        }
    }
}