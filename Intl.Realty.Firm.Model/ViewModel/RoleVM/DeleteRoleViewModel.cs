namespace Intl.Realty.Firm.Model.ViewModel.RoleVM
{
    public class DeleteRoleViewModel
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
