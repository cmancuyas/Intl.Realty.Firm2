using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.CustomerVM;

namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class CustomerMapper
    {
        public static IEnumerable<DeleteCustomerViewModel> ToDeleteCustomerIEnumViewModel(this IEnumerable<Customer> modelIEnum)
        {
            IEnumerable<DeleteCustomerViewModel> viewModelIEnum = new List<DeleteCustomerViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DeleteCustomerViewModel()
                {
                    Id = x.Id,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy,
                });
            }
            return viewModelIEnum;
        }
        public static EditCustomerViewModel ToEditCustomerViewModel(this Customer model)
        {
            return new EditCustomerViewModel
            {
                Id = model.Id,

                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                TelephoneNumber = model.TelephoneNumber,
                Address = model.Address,

                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                UpdatedBy = model.UpdatedBy,
                UpdatedAt = model.UpdatedAt
            };
        }
        public static IEnumerable<CustomerViewModel> ToCustomerIEnumViewModel(this IEnumerable<Customer> modelIEnum)
        {
            IEnumerable<CustomerViewModel> viewModelIEnum = new List<CustomerViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new CustomerViewModel()
                {
                    Id = x.Id,

                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    TelephoneNumber = x.TelephoneNumber,
                    Address = x.Address,

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
        public static Customer ToCustomerModel(this CreateCustomerViewModel viewModel)
        {
            return new Customer
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Email = viewModel.Email,
                TelephoneNumber = viewModel.TelephoneNumber,
                Address = viewModel.Address,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static Customer ToCustomerModel(this EditCustomerViewModel viewModel)
        {
            return new Customer
            {
                Id = viewModel.Id,

                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Email = viewModel.Email,
                TelephoneNumber = viewModel.TelephoneNumber,
                Address = viewModel.Address,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedAt = viewModel.UpdatedAt,
            };
        }
        public static List<CustomerExport> ToCustomerListExport(this List<Customer> modelList)
        {
            List<CustomerExport> exportList = new List<CustomerExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new CustomerExport
                {

                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    TelephoneNumber = x.TelephoneNumber,
                    Address = x.Address,
                }).ToList();
            }
            return exportList;
        }

        public static PreviewCustomerViewModel ToPreviewCustomerViewModel(this Customer model)
        {
            return new PreviewCustomerViewModel
            {
                Id = model.Id,

                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                TelephoneNumber = model.TelephoneNumber,
                Address = model.Address,
            };

        }
    }
}