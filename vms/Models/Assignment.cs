namespace vms.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int OpportunityId { get; set; }
        public VolunteerOpportunity Opportunity { get; set; }
        public int HoursVolunteered { get; set; }
        public DateTime DateAssigned { get; set; }
        public DateTime? DateCompleted { get; set; }
    }
}
