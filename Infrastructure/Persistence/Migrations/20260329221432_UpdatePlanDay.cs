using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlanDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MustTryFoodIngredients",
                table: "PlanDays",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MustTryFoodInstructions",
                table: "PlanDays",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MustTryFoodPriceRange",
                table: "PlanDays",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MustTryFoodIngredients",
                table: "PlanDays");

            migrationBuilder.DropColumn(
                name: "MustTryFoodInstructions",
                table: "PlanDays");

            migrationBuilder.DropColumn(
                name: "MustTryFoodPriceRange",
                table: "PlanDays");
        }
    }
}
