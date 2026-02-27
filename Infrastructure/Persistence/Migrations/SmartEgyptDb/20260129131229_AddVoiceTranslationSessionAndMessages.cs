using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations.SmartEgyptDb
{
    /// <inheritdoc />
    public partial class AddVoiceTranslationSessionAndMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoiceTranslationSessions_AppUser_UserId",
                table: "VoiceTranslationSessions");

            migrationBuilder.DropColumn(
                name: "InputAudioUrl",
                table: "VoiceTranslationSessions");

            migrationBuilder.DropColumn(
                name: "OutputAudioUrl",
                table: "VoiceTranslationSessions");

            migrationBuilder.DropColumn(
                name: "SourceLanguage",
                table: "VoiceTranslationSessions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "VoiceTranslationSessions");

            migrationBuilder.DropColumn(
                name: "TargetLanguage",
                table: "VoiceTranslationSessions");

            migrationBuilder.DropColumn(
                name: "TranscribedText",
                table: "VoiceTranslationSessions");

            migrationBuilder.DropColumn(
                name: "TranslatedText",
                table: "VoiceTranslationSessions");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndedAt",
                table: "VoiceTranslationSessions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "VoiceTranslationSessions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "VoiceTranslationMessages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    SourceLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InputAudioUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranscribedText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranslatedText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutputAudioUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoiceTranslationMessages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_VoiceTranslationMessages_VoiceTranslationSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "VoiceTranslationSessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VoiceTranslationMessages_SessionId",
                table: "VoiceTranslationMessages",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoiceTranslationSessions_AppUser_UserId",
                table: "VoiceTranslationSessions",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoiceTranslationSessions_AppUser_UserId",
                table: "VoiceTranslationSessions");

            migrationBuilder.DropTable(
                name: "VoiceTranslationMessages");

            migrationBuilder.DropColumn(
                name: "EndedAt",
                table: "VoiceTranslationSessions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "VoiceTranslationSessions");

            migrationBuilder.AddColumn<string>(
                name: "InputAudioUrl",
                table: "VoiceTranslationSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OutputAudioUrl",
                table: "VoiceTranslationSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceLanguage",
                table: "VoiceTranslationSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "VoiceTranslationSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TargetLanguage",
                table: "VoiceTranslationSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TranscribedText",
                table: "VoiceTranslationSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TranslatedText",
                table: "VoiceTranslationSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_VoiceTranslationSessions_AppUser_UserId",
                table: "VoiceTranslationSessions",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
