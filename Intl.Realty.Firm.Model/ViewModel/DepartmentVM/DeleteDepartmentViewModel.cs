namespace Intl.Realty.Firm.Model.ViewModel.DepartmentVM
{
    public class DeleteDepartmentViewModel
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
