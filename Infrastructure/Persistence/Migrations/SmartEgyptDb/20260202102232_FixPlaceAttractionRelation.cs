using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations.SmartEgyptDb
{
    public partial class FixPlaceAttractionRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1️⃣ فك الـ FK القديم اللي كان غلط
            migrationBuilder.DropForeignKey(
                name: "FK_Attractions_Places_AttractionId",
                table: "Attractions");

            // 2️⃣ إضافة العمود الجديد PlaceId
            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "Attractions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // 3️⃣ Index + Unique (One-to-One)
            migrationBuilder.CreateIndex(
                name: "IX_Attractions_PlaceId",
                table: "Attractions",
                column: "PlaceId",
                unique: true);

            // 4️⃣ FK الجديد الصحيح
            migrationBuilder.AddForeignKey(
                name: "FK_Attractions_Places_PlaceId",
                table: "Attractions",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "PlaceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // عكس اللي فوق (للرجوع فقط)
            migrationBuilder.DropForeignKey(
                name: "FK_Attractions_Places_PlaceId",
                table: "Attractions");

            migrationBuilder.DropIndex(
                name: "IX_Attractions_PlaceId",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "Attractions");

            migrationBuilder.AddForeignKey(
                name: "FK_Attractions_Places_AttractionId",
                table: "Attractions",
                column: "AttractionId",
                principalTable: "Places",
                principalColumn: "PlaceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

