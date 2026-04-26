using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlanDayHotelPropertiesMo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HotelImage",
                table: "PlanDays",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HotelName",
                table: "PlanDays",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HotelPrice",
                table: "PlanDays",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HotelRating",
                table: "PlanDays",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HotelImage",
                table: "PlanDays");

            migrationBuilder.DropColumn(
                name: "HotelName",
                table: "PlanDays");

            migrationBuilder.DropColumn(
                name: "HotelPrice",
                table: "PlanDays");

            migrationBuilder.DropColumn(
                name: "HotelRating",
                table: "PlanDays");
        }
    }
}
