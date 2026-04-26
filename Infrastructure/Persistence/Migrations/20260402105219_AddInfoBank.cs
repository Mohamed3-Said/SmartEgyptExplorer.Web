using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInfoBank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttractionInfos",
                columns: table => new
                {
                    AttractionInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExploreDurationMin = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpeningDays = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummerOpeningHours = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WinterOpeningHours = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RamadanOpeningHours = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForeignerAdultPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ForeignerStudentPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ArabAdultPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ArabStudentPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EgyptianAdultPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EgyptianStudentPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GarageCarPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GarageBusPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FreeEntryPolicy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InclusiveTicketAccess = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AverageRating = table.Column<double>(type: "float", nullable: false),
                    RatingCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionInfos", x => x.AttractionInfoId);
                });

            migrationBuilder.CreateTable(
                name: "FoodRecipes",
                columns: table => new
                {
                    FoodRecipeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ingredients = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServeNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Categories = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinPriceEGP = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxPriceEGP = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodRecipes", x => x.FoodRecipeId);
                });

            migrationBuilder.CreateTable(
                name: "HotelInfos",
                columns: table => new
                {
                    HotelInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelExternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    MinPricePerNight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxPricePerNight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MetroAccess = table.Column<bool>(type: "bit", nullable: false),
                    ReviewScore = table.Column<double>(type: "float", nullable: false),
                    NumberOfReviews = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyHighlights = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PopularFacilities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Availability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseRules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguagesSpoken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelInfos", x => x.HotelInfoId);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantInfos",
                columns: table => new
                {
                    RestaurantInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantInfos", x => x.RestaurantInfoId);
                });

            migrationBuilder.CreateTable(
                name: "AttractionRatings",
                columns: table => new
                {
                    AttractionRatingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlaceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    AttractionInfoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionRatings", x => x.AttractionRatingId);
                    table.ForeignKey(
                        name: "FK_AttractionRatings_AttractionInfos_AttractionInfoId",
                        column: x => x.AttractionInfoId,
                        principalTable: "AttractionInfos",
                        principalColumn: "AttractionInfoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttractionRatings_AttractionInfoId",
                table: "AttractionRatings",
                column: "AttractionInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttractionRatings");

            migrationBuilder.DropTable(
                name: "FoodRecipes");

            migrationBuilder.DropTable(
                name: "HotelInfos");

            migrationBuilder.DropTable(
                name: "RestaurantInfos");

            migrationBuilder.DropTable(
                name: "AttractionInfos");
        }
    }
}
