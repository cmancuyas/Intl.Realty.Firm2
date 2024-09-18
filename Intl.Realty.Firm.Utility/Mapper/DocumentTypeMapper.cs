using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.DocumentTypeVM;

namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class DocumentTypeMapper
    {
        public static IEnumerable<DeleteDocumentTypeViewModel> ToDeleteDocumentTypeIEnumViewModel(this IEnumerable<DocumentType> modelIEnum)
        {
            IEnumerable<DeleteDocumentTypeViewModel> viewModelIEnum = new List<DeleteDocumentTypeViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DeleteDocumentTypeViewModel()
                {
                    Id = x.Id,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy,
                });
            }
            return viewModelIEnum;
        }
        public static EditDocumentTypeViewModel ToEditDocumentTypeViewModel(this DocumentType model)
        {
            return new EditDocumentTypeViewModel
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
        public static IEnumerable<DocumentTypeViewModel> ToDocumentTypeIEnumViewModel(this IEnumerable<DocumentType> modelIEnum)
        {
            IEnumerable<DocumentTypeViewModel> viewModelIEnum = new List<DocumentTypeViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DocumentTypeViewModel()
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
        public static DocumentType ToDocumentTypeModel(this CreateDocumentTypeViewModel viewModel)
        {
            return new DocumentType
            {
                Code = viewModel.Code,
                Description = viewModel.Description,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static DocumentType ToDocumentTypeModel(this EditDocumentTypeViewModel viewModel)
        {
            return new DocumentType
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
        public static List<DocumentTypeExport> ToDocumentTypeListExport(this List<DocumentType> modelList)
        {
            List<DocumentTypeExport> exportList = new List<DocumentTypeExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new DocumentTypeExport
                {
                    Code = x.Code,
                    Description= x.Description,
                }).ToList();
            }
            return exportList;
        }

        public static PreviewDocumentTypeViewModel ToPreviewDocumentTypeViewModel(this DocumentType model)
        {
            return new PreviewDocumentTypeViewModel
            {
                Id = model.Id,

                Code = model.Code,
                Description = model.Description,
            };

        }
    }
}