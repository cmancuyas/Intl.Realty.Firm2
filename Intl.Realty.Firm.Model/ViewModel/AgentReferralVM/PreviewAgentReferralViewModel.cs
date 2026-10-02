using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.AgentReferralVM
{
    public class PreviewAgentReferralViewModel
    {
        public int Id { get; set; }
        // Who is referring
        [Required(ErrorMessage = "Referrer's Name is required.")]
        public string ReferrerName { get; set; } = null!;
        [Required(ErrorMessage = "Referrer's Phone is required.")]
        public string ReferrerPhone { get; set; } = null!;

        // Who is being referred
        public string? ReferredAgentName { get; set; }
        public string? ReferredAgentPhone { get; set; }
        public string? Notes { get; set; }

        // Submission details
        public Guid SubmittedByUserId { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string SubmittedByFullName { get; set; } = string.Empty;

        // Internal archive flag (separate from IsDeleted)
        public bool IsArchived { get; set; }
    }
}
