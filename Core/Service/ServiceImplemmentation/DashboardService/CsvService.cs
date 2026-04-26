using DomainLayer.DashboardModule;
using System.Text.Json;

public class CsvService
{
    private readonly string _restaurantPath =
        @"D:\DOTNET H7MADA\Graduation Project\Update SEE\SmartEgyptExplorer.Web\SmartEgyptExplorer\wwwroot\csv-data\Egyptian_Restaurants.csv";

    private readonly string _hotelPath =
        @"D:\DOTNET H7MADA\Graduation Project\Update SEE\SmartEgyptExplorer.Web\SmartEgyptExplorer\wwwroot\csv-data\Egypt_Hotels.csv";

    public CsvService() { }

    // ─── Restaurants (مش بنعدل فيه) ───────────────────
    public void AppendRestaurant(DashboardUser user)
    {
        var line = $"{user.DashboardUserId}," +
                   $"{Escape(user.Title)}," +
                   $"{Escape(user.Category)}," +
                   $"{Escape(user.City)}," +
                   $"{Escape(user.Location)}," +
                   $"{user.Longitude}," +
                   $"{user.Latitude}";

        File.AppendAllText(_restaurantPath, line + Environment.NewLine);
    }

    // ─── Hotels (20 columns بالترتيب بالظبط) ──────────
    public void AppendHotel(DashboardUser user)
    {
        // 1. url
        var url = Escape(user.BookingUrl ?? "");

        // 2. hotel_id
        var hotelId = user.DashboardUserId.ToString();

        // 3. title
        var title = Escape(user.Title);

        // 4. location
        var location = Escape(user.Location ?? "");

        // 5. country
        var country = Escape(user.Country ?? "Egypt");

        // 6. city
        var city = Escape(user.City);

        // 7. min_egp_price_per_night
        var minPrice = (user.MinPricePerNight ?? 0).ToString();

        // 8. max_egp_price_per_night
        var maxPrice = (user.MaxPricePerNight ?? 0).ToString();

        // 9. metro_railway_access
        var metro = (user.MetroAccess ?? false).ToString();

        // 10. images — JSON array
        var images = Escape(user.Images ?? "[]");

        // 11. number_of_reviews
        var numReviews = "0";

        // 12. review_score
        var reviewScore = "0";

        // 13. description
        var description = Escape(user.Description ?? "");

        // 14. property_highlights
        var highlights = Escape(user.PropertyHighlights ?? "");

        // 15. most_popular_facilities — JSON array
        var facilities = Escape("[]");

        // 16. availability — JSON array
        var availability = Escape("[]");

        // 17. manager_language_spoken — JSON array
        var languages = Escape(user.LanguagesSpoken ?? "[]");

        // 18. popular_facilities — JSON object
        var popularFacilities = Escape("{}");

        // 19. house_rules — JSON array
        var houseRules = Escape(user.HouseRules ?? "[]");

        // 20. coordinates — JSON object بـ lan و lon
        var coords = Escape($"{{\"lan\":{user.Latitude},\"lon\":{user.Longitude}}}");

        var line = string.Join(",", new[]
        {
            url,          // 1
            hotelId,      // 2
            title,        // 3
            location,     // 4
            country,      // 5
            city,         // 6
            minPrice,     // 7
            maxPrice,     // 8
            metro,        // 9
            images,       // 10
            numReviews,   // 11
            reviewScore,  // 12
            description,  // 13
            highlights,   // 14
            facilities,   // 15
            availability, // 16
            languages,    // 17
            popularFacilities, // 18
            houseRules,   // 19
            coords        // 20
        });

        File.AppendAllText(_hotelPath, line + Environment.NewLine);
    }

    // ─── CSV escape ────────────────────────────────────
    private string Escape(string? value)
    {
        if (string.IsNullOrEmpty(value)) return "\"\"";
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            return $"\"{value.Replace("\"", "\"\"")}\"";
        return value;
    }
}