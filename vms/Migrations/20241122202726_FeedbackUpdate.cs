using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vms.Migrations
{
    /// <inheritdoc />
    public partial class FeedbackUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "VolunteerApplications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "VolunteerApplications");
        }
    }
}
