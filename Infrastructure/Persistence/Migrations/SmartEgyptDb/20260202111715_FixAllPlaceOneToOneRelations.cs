using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations.SmartEgyptDb
{
    public partial class FixAllPlaceOneToOneRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // فك العلاقات القديمة الغلط
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Places_HotelId",
                table: "Hotels");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Places_RestaurantId",
                table: "Restaurants");

            // إضافة PlaceId كـ FK جديد (بدون لمس الـ PK)
            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Unique Index (One-to-One)
            migrationBuilder.CreateIndex(
                name: "IX_Hotels_PlaceId",
                table: "Hotels",
                column: "PlaceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_PlaceId",
                table: "Restaurants",
                column: "PlaceId",
                unique: true);

            // العلاقات الجديدة الصحيحة
            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Places_PlaceId",
                table: "Hotels",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "PlaceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Places_PlaceId",
                table: "Restaurants",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "PlaceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Places_PlaceId",
                table: "Hotels");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Places_PlaceId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_PlaceId",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_PlaceId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "Restaurants");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Places_HotelId",
                table: "Hotels",
                column: "HotelId",
                principalTable: "Places",
                principalColumn: "PlaceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Places_RestaurantId",
                table: "Restaurants",
                column: "RestaurantId",
                principalTable: "Places",
                principalColumn: "PlaceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
