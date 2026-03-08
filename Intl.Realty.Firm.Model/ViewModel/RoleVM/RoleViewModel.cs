namespace Intl.Realty.Firm.Model.ViewModel.RoleVM
{
    public class RoleViewModel 
    {
        public RoleViewModel()
        {
            Users = new List<string>();
        }
        public Guid Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;

        public List<string> Users { get; set; }
    }
}
