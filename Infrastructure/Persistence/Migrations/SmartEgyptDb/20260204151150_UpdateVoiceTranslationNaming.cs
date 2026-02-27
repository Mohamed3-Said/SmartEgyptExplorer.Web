using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations.SmartEgyptDb
{
    /// <inheritdoc />
    public partial class UpdateVoiceTranslationNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoiceTranslationMessages_VoiceTranslationSessions_SessionId",
                table: "VoiceTranslationMessages");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "VoiceTranslationSessions",
                newName: "VoiceTranslationSessionId");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "VoiceTranslationMessages",
                newName: "VoiceTranslationSessionId");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "VoiceTranslationMessages",
                newName: "VoiceTranslationMessageId");

            migrationBuilder.RenameIndex(
                name: "IX_VoiceTranslationMessages_SessionId",
                table: "VoiceTranslationMessages",
                newName: "IX_VoiceTranslationMessages_VoiceTranslationSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoiceTranslationMessages_VoiceTranslationSessions_VoiceTranslationSessionId",
                table: "VoiceTranslationMessages",
                column: "VoiceTranslationSessionId",
                principalTable: "VoiceTranslationSessions",
                principalColumn: "VoiceTranslationSessionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoiceTranslationMessages_VoiceTranslationSessions_VoiceTranslationSessionId",
                table: "VoiceTranslationMessages");

            migrationBuilder.RenameColumn(
                name: "VoiceTranslationSessionId",
                table: "VoiceTranslationSessions",
                newName: "SessionId");

            migrationBuilder.RenameColumn(
                name: "VoiceTranslationSessionId",
                table: "VoiceTranslationMessages",
                newName: "SessionId");

            migrationBuilder.RenameColumn(
                name: "VoiceTranslationMessageId",
                table: "VoiceTranslationMessages",
                newName: "MessageId");

            migrationBuilder.RenameIndex(
                name: "IX_VoiceTranslationMessages_VoiceTranslationSessionId",
                table: "VoiceTranslationMessages",
                newName: "IX_VoiceTranslationMessages_SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoiceTranslationMessages_VoiceTranslationSessions_SessionId",
                table: "VoiceTranslationMessages",
                column: "SessionId",
                principalTable: "VoiceTranslationSessions",
                principalColumn: "SessionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
