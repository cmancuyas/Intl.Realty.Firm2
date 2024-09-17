using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.TransactionTypeVM;

namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class TransactionTypeMapper
    {
        public static IEnumerable<DeleteTransactionTypeViewModel> ToDeleteTransactionTypeIEnumViewModel(this IEnumerable<TransactionType> modelIEnum)
        {
            IEnumerable<DeleteTransactionTypeViewModel> viewModelIEnum = new List<DeleteTransactionTypeViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DeleteTransactionTypeViewModel()
                {
                    Id = x.Id,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy,
                });
            }
            return viewModelIEnum;
        }
        public static EditTransactionTypeViewModel ToEditTransactionTypeViewModel(this TransactionType model)
        {
            return new EditTransactionTypeViewModel
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
        public static IEnumerable<TransactionTypeViewModel> ToTransactionTypeIEnumViewModel(this IEnumerable<TransactionType> modelIEnum)
        {
            IEnumerable<TransactionTypeViewModel> viewModelIEnum = new List<TransactionTypeViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new TransactionTypeViewModel()
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
        public static TransactionType ToTransactionTypeModel(this CreateTransactionTypeViewModel viewModel)
        {
            return new TransactionType
            {
                Name = viewModel.Name,
                Description = viewModel.Description,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static TransactionType ToTransactionTypeModel(this EditTransactionTypeViewModel viewModel)
        {
            return new TransactionType
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
        public static List<TransactionTypeExport> ToTransactionTypeListExport(this List<TransactionType> modelList)
        {
            List<TransactionTypeExport> exportList = new List<TransactionTypeExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new TransactionTypeExport
                {
                    Name = x.Name,
                    Description= x.Description,
                }).ToList();
            }
            return exportList;
        }

        public static PreviewTransactionTypeViewModel ToPreviewTransactionTypeViewModel(this TransactionType model)
        {
            return new PreviewTransactionTypeViewModel
            {
                Id = model.Id,

                Name = model.Name,
                Description = model.Description,
            };

        }
    }
}