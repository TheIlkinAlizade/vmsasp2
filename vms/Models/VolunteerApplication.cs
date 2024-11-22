using System.ComponentModel.DataAnnotations;

namespace vms.Models
{
    public class VolunteerApplication
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }  

        public int VolunteerOpportunityId { get; set; }
        public VolunteerOpportunity? VolunteerOpportunity { get; set; } 
        public bool IsAccepted { get; set; }
        public string? Feedback { get; set; }
    }
}
