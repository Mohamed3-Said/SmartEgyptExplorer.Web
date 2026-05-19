using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTicketToUseAttractionInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Places_PlaceId",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "PlaceId",
                table: "Tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AttractionInfoId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AttractionInfoId",
                table: "Tickets",
                column: "AttractionInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AttractionInfos_AttractionInfoId",
                table: "Tickets",
                column: "AttractionInfoId",
                principalTable: "AttractionInfos",
                principalColumn: "AttractionInfoId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Places_PlaceId",
                table: "Tickets",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "PlaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AttractionInfos_AttractionInfoId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Places_PlaceId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_AttractionInfoId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "AttractionInfoId",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "PlaceId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Places_PlaceId",
                table: "Tickets",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "PlaceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
