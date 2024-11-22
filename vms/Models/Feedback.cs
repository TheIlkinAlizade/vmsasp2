namespace vms.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int OpportunityId { get; set; }
        public VolunteerOpportunity Opportunity { get; set; }
        public DateTime DateGiven { get; set; }
    }
}
