using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vms.Migrations
{
    /// <inheritdoc />
    public partial class AddTagToVolunteerOpportunity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "VolunteerOpportunities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tag",
                table: "VolunteerOpportunities");
        }
    }
}
