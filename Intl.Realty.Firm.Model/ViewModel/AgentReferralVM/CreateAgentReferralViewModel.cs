using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.AgentReferralVM
{
    public class CreateAgentReferralViewModel
    {
        // Who is referring
        [Required(ErrorMessage = "Referrer's Name is required.")]
        public string ReferrerName { get; set; } = null!;
        [Required(ErrorMessage = "Referrer's Phone is required.")]
        public string ReferrerPhone { get; set; } = null!;

        // Who is being referred
        public string? ReferredAgentName { get; set; }
        public string? Notes { get; set; }

        // Submission details
        public Guid SubmittedByUserId { get; set; }
        public DateTime SubmittedAt { get; set; }

        // Internal archive flag (separate from IsDeleted)
        public bool IsArchived { get; set; }

        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public Guid CreatedBy { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
