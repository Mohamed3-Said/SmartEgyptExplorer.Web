using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelCoordinatesAndMealDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "PlanMeal",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaxPrice",
                table: "PlanMeal",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MinPrice",
                table: "PlanMeal",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HotelLatitude",
                table: "PlanDays",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HotelLongitude",
                table: "PlanDays",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "PlanMeal");

            migrationBuilder.DropColumn(
                name: "MaxPrice",
                table: "PlanMeal");

            migrationBuilder.DropColumn(
                name: "MinPrice",
                table: "PlanMeal");

            migrationBuilder.DropColumn(
                name: "HotelLatitude",
                table: "PlanDays");

            migrationBuilder.DropColumn(
                name: "HotelLongitude",
                table: "PlanDays");
        }
    }
}
