using Microsoft.EntityFrameworkCore;
using vms.Models;

namespace vms.Data
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<VolunteerOpportunity> VolunteerOpportunities { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<VolunteerApplication> VolunteerApplications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: Fluent API configurations
            modelBuilder.Entity<VolunteerApplication>()
                .HasOne(v => v.User)
                .WithMany()  // Adjust based on your relationship (one-to-many or many-to-one)
                .HasForeignKey(v => v.UserId)
                .IsRequired(false);  // User navigation property is optional

            modelBuilder.Entity<VolunteerApplication>()
                .HasOne(v => v.VolunteerOpportunity)
                .WithMany()  // Adjust based on your relationship (one-to-many or many-to-one)
                .HasForeignKey(v => v.VolunteerOpportunityId)
                .IsRequired(false);  // VolunteerOpportunity navigation property is optional

            modelBuilder.Entity<User>()
                .HasMany(u => u.VolunteerApplications)  // Assuming a User can have multiple VolunteerApplications
                .WithOne(v => v.User)  // The navigation property in VolunteerApplication
                .HasForeignKey(v => v.UserId)
                .IsRequired(false);  // Make User optional in VolunteerApplication (i.e., just use UserId)

            modelBuilder.Entity<VolunteerOpportunity>()
                .HasMany(v => v.VolunteerApplications)  // Assuming a VolunteerOpportunity can have multiple applications
                .WithOne(a => a.VolunteerOpportunity)  // The navigation property in VolunteerApplication
                .HasForeignKey(a => a.VolunteerOpportunityId)
                .IsRequired(false);  // Make VolunteerOpportunity optional in VolunteerApplication (i.e., just use VolunteerOpportunityId)
        }
    }
}