using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vms.Migrations
{
    /// <inheritdoc />
    public partial class AddMaxApplicantsToVolunteerOpportunity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxApplicants",
                table: "VolunteerOpportunities",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxApplicants",
                table: "VolunteerOpportunities");
        }
    }
}
