using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.AgentReferralVM
{
    public class AgentReferralExport
    {
        // Who is referring
        public string ReferrerName { get; set; } = null!;
        public string ReferrerPhone { get; set; } = null!;

        // Who is being referred
        public string? ReferredAgentName { get; set; }
        public string? Notes { get; set; }

        // Submission details
        public Guid SubmittedByUserId { get; set; }
        public DateTime SubmittedAt { get; set; }

        // Internal archive flag (separate from IsDeleted)
        public bool IsArchived { get; set; }
    }
}
