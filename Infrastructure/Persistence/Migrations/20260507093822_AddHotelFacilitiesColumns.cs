using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelFacilitiesColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Availability",
                table: "DashboardUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MostPopularFacilities",
                table: "DashboardUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfReviews",
                table: "DashboardUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PopularFacilities",
                table: "DashboardUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ReviewScore",
                table: "DashboardUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Availability",
                table: "DashboardUsers");

            migrationBuilder.DropColumn(
                name: "MostPopularFacilities",
                table: "DashboardUsers");

            migrationBuilder.DropColumn(
                name: "NumberOfReviews",
                table: "DashboardUsers");

            migrationBuilder.DropColumn(
                name: "PopularFacilities",
                table: "DashboardUsers");

            migrationBuilder.DropColumn(
                name: "ReviewScore",
                table: "DashboardUsers");
        }
    }
}
