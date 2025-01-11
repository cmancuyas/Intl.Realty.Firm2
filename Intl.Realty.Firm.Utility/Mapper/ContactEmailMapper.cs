using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.ContactEmailVM;

namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class ContactEmailMapper
    {
        public static IEnumerable<DeleteContactEmailViewModel> ToDeleteContactEmailIEnumViewModel(this IEnumerable<ContactEmail> modelIEnum)
        {
            IEnumerable<DeleteContactEmailViewModel> viewModelIEnum = new List<DeleteContactEmailViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DeleteContactEmailViewModel()
                {
                    Id = x.Id,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy,
                });
            }
            return viewModelIEnum;
        }
        public static EditContactEmailViewModel ToEditContactEmailViewModel(this ContactEmail model)
        {
            return new EditContactEmailViewModel
            {
                Id = model.Id,

                EmailCategoryId = model.EmailCategoryId,
                Email = model.Email,

                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                UpdatedBy = model.UpdatedBy,
                UpdatedAt = model.UpdatedAt
            };
        }
        public static IEnumerable<ContactEmailViewModel> ToContactEmailIEnumViewModel(this IEnumerable<ContactEmail> modelIEnum)
        {
            IEnumerable<ContactEmailViewModel> viewModelIEnum = new List<ContactEmailViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new ContactEmailViewModel()
                {
                    Id = x.Id,

                    EmailCategoryId = x.EmailCategoryId,
                    Email = x.Email,

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
        public static ContactEmail ToContactEmailModel(this CreateContactEmailViewModel viewModel)
        {
            return new ContactEmail
            {
                EmailCategoryId = viewModel.EmailCategoryId??0,
                Email = viewModel.Email,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static ContactEmail ToContactEmailModel(this EditContactEmailViewModel viewModel)
        {
            return new ContactEmail
            {
                Id = viewModel.Id,

                EmailCategoryId = viewModel.EmailCategoryId,
                Email = viewModel.Email,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedAt = viewModel.UpdatedAt,
            };
        }
        public static List<ContactEmailExport> ToContactEmailListExport(this List<ContactEmail> modelList)
        {
            List<ContactEmailExport> exportList = new List<ContactEmailExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new ContactEmailExport
                {
                    EmailCategory = x.EmailCategory!.Description,
                    Email = x.Email,
                }).ToList();
            }
            return exportList;
        }

        public static PreviewContactEmailViewModel ToPreviewContactEmailViewModel(this ContactEmail model)
        {
            return new PreviewContactEmailViewModel
            {
                Id = model.Id,

                EmailCategoryId = model.EmailCategoryId,
                Email = model.Email,
                EmailCategory = model.EmailCategory,
            };

        }
    }
}