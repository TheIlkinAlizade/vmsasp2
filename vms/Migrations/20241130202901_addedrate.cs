using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vms.Migrations
{
    /// <inheritdoc />
    public partial class addedrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "Feedbacks",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Feedbacks");
        }
    }
}
