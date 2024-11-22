namespace vms.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Location { get; set; }
        public string? SocialAccounts { get; set; }
    }

}
