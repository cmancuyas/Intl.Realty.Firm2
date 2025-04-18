using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intl.Realty.Firm.Model.ViewModel.UserProfileVM
{
    public class UserProfileViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? DepartmentName { get; set; }
        public string EmploymentStatusName { get; set; } = string.Empty;
        public int? ProfilePictureId { get; set; }
        public bool EmailConfirmed { get; set; }

        // Roles & Claims
        public List<string> Claims { get; set; } = new List<string>();
        public List<string> Roles { get; set; } = new List<string>();

        // 🔹 Team Leader Selection
        public Guid? TeamLeaderId { get; set; } // Stores the selected TeamLeader ID
        public List<SelectListItem> TeamLeaders { get; set; } = new List<SelectListItem>(); // Dropdown list

        // 🔹 Team Members List (Fixed)
        public List<TeamMemberViewModel> TeamMembers { get; set; } = new List<TeamMemberViewModel>(); // List of actual team members

        [NotMapped]
        public string? ProfileImageUrl { get; set; }


    }

    // ✅ New ViewModel for Team Members
    public class TeamMemberViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
