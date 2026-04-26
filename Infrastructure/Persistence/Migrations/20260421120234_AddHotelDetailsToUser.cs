using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelDetailsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookingUrl",
                table: "DashboardUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DashboardUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HouseRules",
                table: "DashboardUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "DashboardUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguagesSpoken",
                table: "DashboardUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxPricePerNight",
                table: "DashboardUsers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MetroAccess",
                table: "DashboardUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinPricePerNight",
                table: "DashboardUsers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropertyHighlights",
                table: "DashboardUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingUrl",
                table: "DashboardUsers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "DashboardUsers");

            migrationBuilder.DropColumn(
                name: "HouseRules",
                table: "DashboardUsers");

            migrationBuilder.DropColumn(
                name: "Images",
                table: "DashboardUsers");

            migrationBuilder.DropColumn(
                name: "LanguagesSpoken",
                table: "DashboardUsers");

            migrationBuilder.DropColumn(
                name: "MaxPricePerNight",
                table: "DashboardUsers");

            migrationBuilder.DropColumn(
                name: "MetroAccess",
                table: "DashboardUsers");

            migrationBuilder.DropColumn(
                name: "MinPricePerNight",
                table: "DashboardUsers");

            migrationBuilder.DropColumn(
                name: "PropertyHighlights",
                table: "DashboardUsers");
        }
    }
}
