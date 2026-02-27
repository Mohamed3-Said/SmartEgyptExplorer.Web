using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations.SmartEgyptDb
{
    /// <inheritdoc />
    public partial class DisconnectUserFromVoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoiceTranslationSessions_AppUser_UserId",
                table: "VoiceTranslationSessions");

            migrationBuilder.DropIndex(
                name: "IX_VoiceTranslationSessions_UserId",
                table: "VoiceTranslationSessions");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "VoiceTranslationSessions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "VoiceTranslationSessions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoiceTranslationSessions_AppUserId",
                table: "VoiceTranslationSessions",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoiceTranslationSessions_AppUser_AppUserId",
                table: "VoiceTranslationSessions",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoiceTranslationSessions_AppUser_AppUserId",
                table: "VoiceTranslationSessions");

            migrationBuilder.DropIndex(
                name: "IX_VoiceTranslationSessions_AppUserId",
                table: "VoiceTranslationSessions");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "VoiceTranslationSessions");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "VoiceTranslationSessions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_VoiceTranslationSessions_UserId",
                table: "VoiceTranslationSessions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoiceTranslationSessions_AppUser_UserId",
                table: "VoiceTranslationSessions",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
