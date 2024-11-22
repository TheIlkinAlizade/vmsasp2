using System.ComponentModel.DataAnnotations;

namespace vms.Models
{
    public class VolunteerOpportunity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }

        public List<VolunteerApplication>? VolunteerApplications { get; set; }

        public Organization? Organization { get; set; }
        public int OrganizationId { get; set; }
    }

}
