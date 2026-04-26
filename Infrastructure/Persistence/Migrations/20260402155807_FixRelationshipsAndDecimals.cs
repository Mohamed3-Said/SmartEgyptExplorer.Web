using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationshipsAndDecimals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserFormSubmissionId",
                table: "Plans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plans_UserFormSubmissionId",
                table: "Plans",
                column: "UserFormSubmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_UserFormSubmissions_UserFormSubmissionId",
                table: "Plans",
                column: "UserFormSubmissionId",
                principalTable: "UserFormSubmissions",
                principalColumn: "UserFormSubmissionId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_UserFormSubmissions_UserFormSubmissionId",
                table: "Plans");

            migrationBuilder.DropIndex(
                name: "IX_Plans_UserFormSubmissionId",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "UserFormSubmissionId",
                table: "Plans");
        }
    }
}
