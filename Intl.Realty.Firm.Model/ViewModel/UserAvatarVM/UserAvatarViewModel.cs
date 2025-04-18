namespace Intl.Realty.Firm.Model.ViewModel.UserAvatarVM
{
    public class UserAvatarViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; } = "/files/img/avatar-blank.png";
        public List<string> Roles { get; set; } = new();
        public Guid UserId { get; set; }
    }
}
