using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vms.Migrations
{
    /// <inheritdoc />
    public partial class InitialContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerApplications_Users_UserId",
                table: "VolunteerApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerApplications_VolunteerOpportunities_VolunteerOpportunityId",
                table: "VolunteerApplications");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerApplications_Users_UserId",
                table: "VolunteerApplications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerApplications_VolunteerOpportunities_VolunteerOpportunityId",
                table: "VolunteerApplications",
                column: "VolunteerOpportunityId",
                principalTable: "VolunteerOpportunities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerApplications_Users_UserId",
                table: "VolunteerApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerApplications_VolunteerOpportunities_VolunteerOpportunityId",
                table: "VolunteerApplications");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerApplications_Users_UserId",
                table: "VolunteerApplications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerApplications_VolunteerOpportunities_VolunteerOpportunityId",
                table: "VolunteerApplications",
                column: "VolunteerOpportunityId",
                principalTable: "VolunteerOpportunities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
