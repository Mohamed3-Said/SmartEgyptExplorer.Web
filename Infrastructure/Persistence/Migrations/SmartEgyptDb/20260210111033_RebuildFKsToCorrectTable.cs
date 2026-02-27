using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations.SmartEgyptDb
{
    public partial class RebuildFKsToCorrectTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. حذفنا الأوامر اللي بتمسح الـ Constraints القديمة لأننا مسحناها يدوي بالسكريبت
            // 2. حذفنا أمر RenameTable لأن جدول AppUser اتمسح فعلاً بالسكريبت

            // 3. بناء الروابط الجديدة مباشرة مع جدول Users الصح
            migrationBuilder.AddForeignKey(
                name: "FK_Plans_Users_UserId",
                table: "Plans",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_UserId",
                table: "Tickets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFormSubmissions_Users_UserId",
                table: "UserFormSubmissions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VoiceTranslationSessions_Users_AppUserId",
                table: "VoiceTranslationSessions",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // في حالة الـ Rollback نلغي الروابط الجديدة
            migrationBuilder.DropForeignKey(name: "FK_Plans_Users_UserId", table: "Plans");
            migrationBuilder.DropForeignKey(name: "FK_Reviews_Users_UserId", table: "Reviews");
            migrationBuilder.DropForeignKey(name: "FK_Tickets_Users_UserId", table: "Tickets");
            migrationBuilder.DropForeignKey(name: "FK_UserFormSubmissions_Users_UserId", table: "UserFormSubmissions");
            migrationBuilder.DropForeignKey(name: "FK_VoiceTranslationSessions_Users_AppUserId", table: "VoiceTranslationSessions");
        }
    }
}
