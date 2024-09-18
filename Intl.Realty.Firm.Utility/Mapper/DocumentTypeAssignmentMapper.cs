using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.DocumentTypeAssignmentVM;

namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class DocumentTypeAssignmentMapper
    {
        public static IEnumerable<DeleteDocumentTypeAssignmentViewModel> ToDeleteDocumentTypeAssignmentIEnumViewModel(this IEnumerable<DocumentTypeAssignment> modelIEnum)
        {
            IEnumerable<DeleteDocumentTypeAssignmentViewModel> viewModelIEnum = new List<DeleteDocumentTypeAssignmentViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DeleteDocumentTypeAssignmentViewModel()
                {
                    Id = x.Id,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy,
                });
            }
            return viewModelIEnum;
        }
        public static EditDocumentTypeAssignmentViewModel ToEditDocumentTypeAssignmentViewModel(this DocumentTypeAssignment model)
        {
            return new EditDocumentTypeAssignmentViewModel
            {
                Id = model.Id,

                DocumentTypeId = model.DocumentTypeId,
                TransactionTypeId = model.TransactionTypeId,

                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                UpdatedBy = model.UpdatedBy,
                UpdatedAt = model.UpdatedAt
            };
        }
        public static IEnumerable<DocumentTypeAssignmentViewModel> ToDocumentTypeAssignmentIEnumViewModel(this IEnumerable<DocumentTypeAssignment> modelIEnum)
        {
            IEnumerable<DocumentTypeAssignmentViewModel> viewModelIEnum = new List<DocumentTypeAssignmentViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DocumentTypeAssignmentViewModel()
                {
                    Id = x.Id,

                    DocumentTypeId = x.DocumentTypeId,
                    TransactionTypeId= x.TransactionTypeId,

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
        public static DocumentTypeAssignment ToDocumentTypeAssignmentModel(this CreateDocumentTypeAssignmentViewModel viewModel)
        {
            return new DocumentTypeAssignment
            {
                DocumentTypeId = viewModel.DocumentTypeId,
                TransactionTypeId = viewModel.TransactionTypeId,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static DocumentTypeAssignment ToDocumentTypeAssignmentModel(this EditDocumentTypeAssignmentViewModel viewModel)
        {
            return new DocumentTypeAssignment
            {
                Id = viewModel.Id,

                DocumentTypeId = viewModel.DocumentTypeId,
                TransactionTypeId = viewModel.TransactionTypeId,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedAt = viewModel.UpdatedAt,
            };
        }
        public static List<DocumentTypeAssignmentExport> ToDocumentTypeAssignmentListExport(this List<DocumentTypeAssignment> modelList)
        {
            List<DocumentTypeAssignmentExport> exportList = new List<DocumentTypeAssignmentExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new DocumentTypeAssignmentExport
                {
                    DocumentType = x.DocumentType!.Description,
                    TransactionType= x.TransactionType!.Description,
                }).ToList();
            }
            return exportList;
        }

        public static PreviewDocumentTypeAssignmentViewModel ToPreviewDocumentTypeAssignmentViewModel(this DocumentTypeAssignment model)
        {
            return new PreviewDocumentTypeAssignmentViewModel
            {
                Id = model.Id,

                DocumentTypeId = model.DocumentTypeId,
                TransactionTypeId = model.TransactionTypeId,
            };

        }
    }
}