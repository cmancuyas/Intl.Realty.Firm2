namespace Intl.Realty.Firm.Model.ViewModel.AgentReferralVM
{
    public class DeleteAgentReferralViewModel
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
