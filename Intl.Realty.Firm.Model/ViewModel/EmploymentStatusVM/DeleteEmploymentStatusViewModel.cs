using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.EmploymentStatusVM
{
    public class DeleteEmploymentStatusViewModel
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
