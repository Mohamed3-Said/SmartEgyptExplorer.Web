using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlanMeal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecommendedFood",
                table: "PlanActivities");

            migrationBuilder.RenameColumn(
                name: "MustTryFood",
                table: "PlanDays",
                newName: "MustTryFoodTitle");

            migrationBuilder.RenameColumn(
                name: "MustTryDishTitle",
                table: "PlanDays",
                newName: "MustTryFoodImage");

            migrationBuilder.RenameColumn(
                name: "MustTryDishDesc",
                table: "PlanDays",
                newName: "MustTryFoodDescription");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "PlanDays",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "PlanDays",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PlanMeal",
                columns: table => new
                {
                    PlanMealId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanDayId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanMeal", x => x.PlanMealId);
                    table.ForeignKey(
                        name: "FK_PlanMeal_PlanDays_PlanDayId",
                        column: x => x.PlanDayId,
                        principalTable: "PlanDays",
                        principalColumn: "PlanDayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanDays_HotelId",
                table: "PlanDays",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanMeal_PlanDayId",
                table: "PlanMeal",
                column: "PlanDayId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanDays_Hotels_HotelId",
                table: "PlanDays",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "HotelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanDays_Hotels_HotelId",
                table: "PlanDays");

            migrationBuilder.DropTable(
                name: "PlanMeal");

            migrationBuilder.DropIndex(
                name: "IX_PlanDays_HotelId",
                table: "PlanDays");

            migrationBuilder.DropColumn(
                name: "City",
                table: "PlanDays");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "PlanDays");

            migrationBuilder.RenameColumn(
                name: "MustTryFoodTitle",
                table: "PlanDays",
                newName: "MustTryFood");

            migrationBuilder.RenameColumn(
                name: "MustTryFoodImage",
                table: "PlanDays",
                newName: "MustTryDishTitle");

            migrationBuilder.RenameColumn(
                name: "MustTryFoodDescription",
                table: "PlanDays",
                newName: "MustTryDishDesc");

            migrationBuilder.AddColumn<string>(
                name: "RecommendedFood",
                table: "PlanActivities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
