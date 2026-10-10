using Intl.Realty.Firm.Model.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intl.Realty.Firm.Model.ViewModel.AgentReferralVM
{
    public class AgentReferralViewModel : BaseModel
    {
        [Key]
        public int Id { get; set; }
        // Who is referring
        [Required]
        [MaxLength(100)]
        public string ReferrerName { get; set; } = null!;
        [Required]
        [MaxLength(100)]
        public string ReferrerPhone { get; set; } = null!;

        // Who is being referred
        public string? ReferredAgentName { get; set; }
        public string? ReferredAgentPhone { get; set; }
        public string? Notes { get; set; }

        // Submission details
        public Guid SubmittedByUserId { get; set; }
        public DateTime SubmittedAt { get; set; }

        // Internal archive flag (separate from IsDeleted)
        public bool IsArchived { get; set; }
    }
}
