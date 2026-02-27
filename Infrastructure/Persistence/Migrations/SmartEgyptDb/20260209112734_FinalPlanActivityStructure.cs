using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations.SmartEgyptDb
{
    /// <inheritdoc />
    public partial class FinalPlanActivityStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "PlanActivities",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PlanActivities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "PlanActivities",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "PlanActivities",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MapUrl",
                table: "PlanActivities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TransportCost",
                table: "PlanActivities",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "PlanActivities");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PlanActivities");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "PlanActivities");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "PlanActivities");

            migrationBuilder.DropColumn(
                name: "MapUrl",
                table: "PlanActivities");

            migrationBuilder.DropColumn(
                name: "TransportCost",
                table: "PlanActivities");
        }
    }
}
