using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations.SmartEgyptDb
{
    /// <inheritdoc />
    public partial class FinalFixAllFormRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AIResults_UserFormSubmissions_SubmissionId",
                table: "AIResults");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_UserFormSubmissions_SubmissionId",
                table: "UserAnswers");

            migrationBuilder.RenameColumn(
                name: "SubmissionId",
                table: "UserFormSubmissions",
                newName: "UserFormSubmissionId");

            migrationBuilder.RenameColumn(
                name: "SubmissionId",
                table: "UserAnswers",
                newName: "UserFormSubmissionId");

            migrationBuilder.RenameColumn(
                name: "AnswerId",
                table: "UserAnswers",
                newName: "UserAnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnswers_SubmissionId",
                table: "UserAnswers",
                newName: "IX_UserAnswers_UserFormSubmissionId");

            migrationBuilder.RenameColumn(
                name: "SubmissionId",
                table: "AIResults",
                newName: "UserFormSubmissionId");

            migrationBuilder.RenameIndex(
                name: "IX_AIResults_SubmissionId",
                table: "AIResults",
                newName: "IX_AIResults_UserFormSubmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AIResults_UserFormSubmissions_UserFormSubmissionId",
                table: "AIResults",
                column: "UserFormSubmissionId",
                principalTable: "UserFormSubmissions",
                principalColumn: "UserFormSubmissionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_UserFormSubmissions_UserFormSubmissionId",
                table: "UserAnswers",
                column: "UserFormSubmissionId",
                principalTable: "UserFormSubmissions",
                principalColumn: "UserFormSubmissionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AIResults_UserFormSubmissions_UserFormSubmissionId",
                table: "AIResults");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_UserFormSubmissions_UserFormSubmissionId",
                table: "UserAnswers");

            migrationBuilder.RenameColumn(
                name: "UserFormSubmissionId",
                table: "UserFormSubmissions",
                newName: "SubmissionId");

            migrationBuilder.RenameColumn(
                name: "UserFormSubmissionId",
                table: "UserAnswers",
                newName: "SubmissionId");

            migrationBuilder.RenameColumn(
                name: "UserAnswerId",
                table: "UserAnswers",
                newName: "AnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnswers_UserFormSubmissionId",
                table: "UserAnswers",
                newName: "IX_UserAnswers_SubmissionId");

            migrationBuilder.RenameColumn(
                name: "UserFormSubmissionId",
                table: "AIResults",
                newName: "SubmissionId");

            migrationBuilder.RenameIndex(
                name: "IX_AIResults_UserFormSubmissionId",
                table: "AIResults",
                newName: "IX_AIResults_SubmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AIResults_UserFormSubmissions_SubmissionId",
                table: "AIResults",
                column: "SubmissionId",
                principalTable: "UserFormSubmissions",
                principalColumn: "SubmissionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_UserFormSubmissions_SubmissionId",
                table: "UserAnswers",
                column: "SubmissionId",
                principalTable: "UserFormSubmissions",
                principalColumn: "SubmissionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
