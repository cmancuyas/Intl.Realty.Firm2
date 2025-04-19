using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.AgentReferralVM;

namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class AgentReferralMapper
    {
        public static IEnumerable<DeleteAgentReferralViewModel> ToDeleteAgentReferralIEnumViewModel(this IEnumerable<AgentReferral> modelIEnum)
        {
            IEnumerable<DeleteAgentReferralViewModel> viewModelIEnum = new List<DeleteAgentReferralViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new DeleteAgentReferralViewModel()
                {
                    Id = x.Id,
                    DeletedAt = x.DeletedAt,
                    DeletedBy = x.DeletedBy,
                });
            }
            return viewModelIEnum;
        }
        public static EditAgentReferralViewModel ToEditAgentReferralViewModel(this AgentReferral model)
        {
            return new EditAgentReferralViewModel
            {
                Id = model.Id,

                ReferrerName = model.ReferrerName,
                ReferrerPhone = model.ReferrerPhone,
                ReferredAgentName = model.ReferredAgentName,
                Notes = model.Notes,
                SubmittedByUserId = model.SubmittedByUserId,
                SubmittedAt = model.SubmittedAt,
                IsArchived = model.IsArchived,

                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                UpdatedBy = model.UpdatedBy,
                UpdatedAt = model.UpdatedAt
            };
        }
        public static IEnumerable<AgentReferralViewModel> ToAgentReferralIEnumViewModel(this IEnumerable<AgentReferral> modelIEnum)
        {
            IEnumerable<AgentReferralViewModel> viewModelIEnum = new List<AgentReferralViewModel>();
            if (modelIEnum != null)
            {
                viewModelIEnum = modelIEnum.Select(x => new AgentReferralViewModel()
                {
                    Id = x.Id,

                    ReferrerName = x.ReferrerName,
                    ReferrerPhone = x.ReferrerPhone,
                    ReferredAgentName = x.ReferredAgentName,
                    Notes = x.Notes,
                    SubmittedByUserId = x.SubmittedByUserId,
                    SubmittedAt = x.SubmittedAt,
                    IsArchived = x.IsArchived,

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
        public static AgentReferral ToAgentReferralModel(this CreateAgentReferralViewModel viewModel)
        {
            return new AgentReferral
            {
                ReferrerName = viewModel.ReferrerName,
                ReferrerPhone = viewModel.ReferrerPhone,
                ReferredAgentName = viewModel.ReferredAgentName,
                Notes = viewModel.Notes,
                SubmittedByUserId = viewModel.SubmittedByUserId,
                SubmittedAt = viewModel.SubmittedAt,
                IsArchived = viewModel.IsArchived,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                CreatedAt = viewModel.CreatedAt,
                CreatedBy = viewModel.CreatedBy,
            };
        }
        public static AgentReferral ToAgentReferralModel(this EditAgentReferralViewModel viewModel)
        {
            return new AgentReferral
            {
                Id = viewModel.Id,

                ReferrerName = viewModel.ReferrerName,
                ReferrerPhone = viewModel.ReferrerPhone,
                ReferredAgentName = viewModel.ReferredAgentName,
                Notes = viewModel.Notes,
                SubmittedByUserId = viewModel.SubmittedByUserId,
                SubmittedAt = viewModel.SubmittedAt,
                IsArchived = viewModel.IsArchived,

                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedAt = viewModel.UpdatedAt,
            };
        }
        public static List<AgentReferralExport> ToAgentReferralListExport(this List<AgentReferral> modelList)
        {
            List<AgentReferralExport> exportList = new List<AgentReferralExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new AgentReferralExport
                {
                    ReferrerName = x.ReferrerName,
                    ReferrerPhone = x.ReferrerPhone,
                    ReferredAgentName = x.ReferredAgentName,
                    Notes = x.Notes,
                    SubmittedByUserId = x.SubmittedByUserId,
                    SubmittedAt = x.SubmittedAt,
                    IsArchived = x.IsArchived,

                }).ToList();
            }
            return exportList;
        }

        public static PreviewAgentReferralViewModel ToPreviewAgentReferralViewModel(this AgentReferral model)
        {
            return new PreviewAgentReferralViewModel
            {
                Id = model.Id,

                ReferrerName = model.ReferrerName,
                ReferrerPhone = model.ReferrerPhone,
                ReferredAgentName = model.ReferredAgentName,
                Notes = model.Notes,
                SubmittedByUserId = model.SubmittedByUserId,
                SubmittedAt = model.SubmittedAt,
                IsArchived = model.IsArchived,
            };

        }
    }
}