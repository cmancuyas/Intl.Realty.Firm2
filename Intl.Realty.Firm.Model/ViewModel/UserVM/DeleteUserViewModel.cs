namespace Intl.Realty.Firm.Model.ViewModel.UserVM
{
    public class DeleteUserViewModel
    {
        public Guid Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
