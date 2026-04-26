using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClaudEntitesandDtos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HotelLocation",
                table: "PlanDays",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HotelMetroAccess",
                table: "PlanDays",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HotelReviews",
                table: "PlanDays",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HotelRules",
                table: "PlanDays",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MustTryFoodPriceState",
                table: "PlanDays",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StartTime",
                table: "PlanActivities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<string>(
                name: "EndTime",
                table: "PlanActivities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HotelLocation",
                table: "PlanDays");

            migrationBuilder.DropColumn(
                name: "HotelMetroAccess",
                table: "PlanDays");

            migrationBuilder.DropColumn(
                name: "HotelReviews",
                table: "PlanDays");

            migrationBuilder.DropColumn(
                name: "HotelRules",
                table: "PlanDays");

            migrationBuilder.DropColumn(
                name: "MustTryFoodPriceState",
                table: "PlanDays");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "PlanActivities",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "PlanActivities",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
