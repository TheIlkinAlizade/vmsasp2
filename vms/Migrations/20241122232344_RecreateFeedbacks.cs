using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vms.Migrations
{
    /// <inheritdoc />
    public partial class RecreateFeedbacks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Users_UserId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_VolunteerOpportunities_OpportunityId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_OpportunityId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_UserId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Feedbacks",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "DateGiven",
                table: "Feedbacks",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "Feedbacks",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Feedbacks",
                newName: "DateGiven");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_OpportunityId",
                table: "Feedbacks",
                column: "OpportunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_UserId",
                table: "Feedbacks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Users_UserId",
                table: "Feedbacks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_VolunteerOpportunities_OpportunityId",
                table: "Feedbacks",
                column: "OpportunityId",
                principalTable: "VolunteerOpportunities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
