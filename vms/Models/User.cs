using System.ComponentModel.DataAnnotations;

namespace vms.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? CV { get; set; }  // Optional
        public string? Experience { get; set; }  // Optional
        public bool IsOrganization { get; set; }  // To distinguish between user and organization accounts
        public List<VolunteerApplication>? VolunteerApplications { get; set; }
    }

}
