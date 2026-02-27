using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations.SmartEgyptDb
{
    public partial class FinalizeAttractionTicketStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. التأكد من مسح الجدول القديم لو موجود عشان نبدأ على نظافة
            migrationBuilder.Sql("IF OBJECT_ID(N'[AttractionTickets]', N'U') IS NOT NULL DROP TABLE [AttractionTickets];");

            // 2. إنشاء الجدول بالهيكل الجديد المعتمد على Id مستقل
            migrationBuilder.CreateTable(
                name: "AttractionTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttractionId = table.Column<int>(type: "int", nullable: false),
                    TicketCategory = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Standard"),
                    VisitorType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionTickets", x => x.Id);
                    // الربط هنا على AttractionId كما هو في كلاس الـ Attraction عندك
                    table.ForeignKey(
                        name: "FK_AttractionTickets_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "AttractionId",
                        onDelete: ReferentialAction.Cascade);
                });

            // 3. عمل Index لتسريع البحث بالـ AttractionId
            migrationBuilder.CreateIndex(
                name: "IX_AttractionTickets_AttractionId",
                table: "AttractionTickets",
                column: "AttractionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // في حالة التراجع عن المايجريشن
            migrationBuilder.DropTable(
                name: "AttractionTickets");
        }
    }
}