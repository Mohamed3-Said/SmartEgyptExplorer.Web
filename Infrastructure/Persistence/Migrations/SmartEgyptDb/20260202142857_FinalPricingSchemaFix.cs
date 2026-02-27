using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations.SmartEgyptDb
{
    public partial class FinalPricingSchemaFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // إزالة الـ PK القديم
            migrationBuilder.DropPrimaryKey(
                name: "PK_AttractionTickets",
                table: "AttractionTickets");

            // إضافة Composite Primary Key الجديد
            migrationBuilder.AddPrimaryKey(
                name: "PK_AttractionTickets",
                table: "AttractionTickets",
                columns: new[] { "AttractionId", "TicketId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // إزالة الـ Composite PK
            migrationBuilder.DropPrimaryKey(
                name: "PK_AttractionTickets",
                table: "AttractionTickets");

            // إعادة الـ PK القديم (TicketId فقط)
            migrationBuilder.AddPrimaryKey(
                name: "PK_AttractionTickets",
                table: "AttractionTickets",
                column: "TicketId");
        }
    }
}
