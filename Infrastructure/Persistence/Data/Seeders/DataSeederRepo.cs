using CsvHelper;
using CsvHelper.Configuration;
using DomainLayer.Models.InfoBankModule;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.configurations;
using System.Globalization;
using ServiceAbstraction.Services;
using DomainLayer.Contracts.Repo;

namespace Persistence.Data.Seeders
{
    public class DataSeederRepo : IDataSeederRepo
    {
        private readonly SmartEgyptDbContext _context;

        public DataSeederRepo(SmartEgyptDbContext context)
        {
            _context = context;
        }

        public async Task<string> SeedAllAsync(string csvFolderPath)
        {
            var results = new List<string>();

            try { results.Add(await SeedAttractionsAsync(csvFolderPath)); }
            catch (Exception ex) { results.Add($"❌ Attractions Error: {ex.Message}"); }

            try { results.Add(await SeedRatingsAsync(csvFolderPath)); }
            catch (Exception ex)
            {
                results.Add($"❌ Ratings Error: {ex.Message} | Inner: {ex.InnerException?.Message} | Stack: {ex.StackTrace}");
            }

            try { results.Add(await SeedHotelsAsync(csvFolderPath)); }
            catch (Exception ex) { results.Add($"❌ Hotels Error: {ex.Message}"); }

            try { results.Add(await SeedRestaurantsAsync(csvFolderPath)); }
            catch (Exception ex) { results.Add($"❌ Restaurants Error: {ex.Message}"); }

            try { results.Add(await SeedFoodRecipesAsync(csvFolderPath)); }
            catch (Exception ex) { results.Add($"❌ Food Error: {ex.Message}"); }

            return string.Join("\n", results);
        }

        // ===================== Attractions =====================
        private async Task<string> SeedAttractionsAsync(string folder)
        {
            if (await _context.AttractionInfos.AnyAsync())
                return "✅ Attractions: already seeded, skipped.";

            var path = Path.Combine(folder, "Egypt_Attractions.csv");
            if (!File.Exists(path)) return "❌ Attractions CSV not found.";

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                BadDataFound = null,
                IgnoreBlankLines = true,
                PrepareHeaderForMatch = args => args.Header.Trim()
            };

            using var reader = new StreamReader(path, System.Text.Encoding.UTF8);
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecords<AttractionCsvRow>().ToList();
            var attractions = records.Select(r => new AttractionInfo
            {
                PlaceId = r.PlaceId,
                Name = r.PlaceName,
                Category = r.Category,
                City = r.City,
                NormalizedCity = r.NormalizedCity,
                Latitude = double.TryParse(r.Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out var lat) ? lat : 0,
                Longitude = double.TryParse(r.Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out var lng) ? lng : 0,
                Description = r.Description,
                ExploreDurationMin = int.TryParse(r.ExploreDurationMin, out var dur) ? dur : null,
                ImageUrl = r.ImageUrl,
                OpeningDays = r.OpeningDays,
                SummerOpeningHours = r.SummerOpeningHours,
                WinterOpeningHours = r.WinterOpeningHours,
                RamadanOpeningHours = r.RamadanOpeningHours,
                ForeignerAdultPrice = decimal.TryParse(r.ForeignerAdultPrice, out var fap) ? fap : 0,
                ForeignerStudentPrice = decimal.TryParse(r.ForeignerStudentPrice, out var fsp) ? fsp : 0,
                ArabAdultPrice = decimal.TryParse(r.ArabAdultPrice, out var aap) ? aap : 0,
                ArabStudentPrice = decimal.TryParse(r.ArabStudentPrice, out var asp) ? asp : 0,
                EgyptianAdultPrice = decimal.TryParse(r.EgyptianAdultPrice, out var eap) ? eap : 0,
                EgyptianStudentPrice = decimal.TryParse(r.EgyptianStudentPrice, out var esp) ? esp : 0,
                GarageCarPrice = decimal.TryParse(r.GarageCarPrice, out var gcp) ? gcp : 0,
                GarageBusPrice = decimal.TryParse(r.GarageBusPrice, out var gbp) ? gbp : 0,
                FreeEntryPolicy = r.FreeEntryPolicy,
                InclusiveTicketAccess = r.InclusiveTicketAccess,
                AverageRating = 0,
                RatingCount = 0
            }).ToList();

            await _context.AttractionInfos.AddRangeAsync(attractions);
            await _context.SaveChangesAsync();
            return $"✅ Attractions: {attractions.Count} records seeded.";
        }

        // ===================== Ratings =====================
        private async Task<string> SeedRatingsAsync(string folder)
        {
            if (await _context.AttractionRatings.AnyAsync())
                return "✅ Ratings: already seeded, skipped.";

            var path = Path.Combine(folder, "Egypt_Attractions_Ratings.csv");
            if (!File.Exists(path)) return "❌ Ratings CSV not found.";

            var attractions = await _context.AttractionInfos
                .Select(a => new { a.AttractionInfoId, a.PlaceId })
                .ToListAsync();

            // ✅ كده هيتجاهل الـ duplicates:
            var attractionDict = attractions
                .GroupBy(a => a.PlaceId)
                .ToDictionary(g => g.Key, g => g.First().AttractionInfoId);

            var ratings = new List<AttractionRating>();

            // ✅ بنقرأ بدون CsvHelper خالص
            var allLines = File.ReadAllLines(path, System.Text.Encoding.UTF8);

            foreach (var line in allLines.Skip(1)) // skip header
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var cols = line.Trim().Split(',');
                if (cols.Length < 3) continue;

                var userId = cols[0].Trim().Trim('"');
                var placeId = cols[1].Trim().Trim('"');
                var ratingStr = cols[2].Trim().Trim('"');

                if (string.IsNullOrEmpty(placeId)) continue;
                if (!attractionDict.TryGetValue(placeId, out var attractionId)) continue;
                if (!double.TryParse(ratingStr,
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var rat)) continue;

                ratings.Add(new AttractionRating
                {
                    UserId = userId,
                    PlaceId = placeId,
                    Rating = rat,
                    AttractionInfoId = attractionId
                });
            }

            await _context.AttractionRatings.AddRangeAsync(ratings);
            await _context.SaveChangesAsync();

            // حساب الـ AverageRating
            var grouped = ratings.GroupBy(r => r.AttractionInfoId);
            foreach (var group in grouped)
            {
                var attraction = await _context.AttractionInfos.FindAsync(group.Key);
                if (attraction == null) continue;
                attraction.AverageRating = Math.Round(group.Average(r => r.Rating), 2);
                attraction.RatingCount = group.Count();
            }
            await _context.SaveChangesAsync();
            return $"✅ Ratings: {ratings.Count} records seeded.";

            
        }

        // ===================== Hotels =====================
        private async Task<string> SeedHotelsAsync(string folder)
        {
            if (await _context.HotelInfos.AnyAsync())
                return "✅ Hotels: already seeded, skipped.";

            var path = Path.Combine(folder, "Egypt_Hotels.csv");
            if (!File.Exists(path)) return "❌ Hotels CSV not found.";

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                BadDataFound = null ,
                PrepareHeaderForMatch = args => args.Header.Trim().ToLower()
            };

            using var reader = new StreamReader(path, System.Text.Encoding.UTF8);
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecords<dynamic>().ToList();
            var hotels = new List<HotelInfo>();

            foreach (var r in records)
            {
                try
                {
                    // استخراج lat/lng من الـ coordinates JSON
                    double lat = 0, lng = 0;
                    try
                    {
                        var coords = System.Text.Json.JsonSerializer
                            .Deserialize<Dictionary<string, double>>(
                                (string)r.coordinates ?? "{}");
                        lat = coords?.GetValueOrDefault("lan") ?? 0;
                        lng = coords?.GetValueOrDefault("lon") ?? 0;
                    }
                    catch { }

                    // تنظيف السعر من الـ commas
                    var minPrice = (string)r.min_egp_price_per_night ?? "0";
                    var maxPrice = (string)r.max_egp_price_per_night ?? "0";
                    minPrice = minPrice.Replace(",", "");
                    maxPrice = maxPrice.Replace(",", "");

                    hotels.Add(new HotelInfo
                    {
                        HotelExternalId = r.hotel_id?.ToString() ?? "",
                        Name = r.title ?? "",
                        Location = r.location,
                        City = r.city ?? "",
                        Country = r.country,
                        Latitude = lat,
                        Longitude = lng,
                        MinPricePerNight = decimal.TryParse(minPrice, out var minP) ? minP : 0,
                        MaxPricePerNight = decimal.TryParse(maxPrice, out var maxP) ? maxP : 0,
                        MetroAccess = (string)r.metro_railway_access == "True",
                        ReviewScore = double.TryParse((string)r.review_score, out var rs) ? rs : 0,
                        NumberOfReviews = int.TryParse((string)r.number_of_reviews, out var nr) ? nr : 0,
                        Description = r.description,
                        PropertyHighlights = r.property_highlights,
                        PopularFacilities = r.most_popular_facilities,
                        Images = r.images,
                        Availability = r.availability,
                        HouseRules = r.house_rules,
                        LanguagesSpoken = r.manager_language_spoken,
                        BookingUrl = r.url
                    });
                }
                catch { }
            }

            await _context.HotelInfos.AddRangeAsync(hotels);
            await _context.SaveChangesAsync();
            return $"✅ Hotels: {hotels.Count} records seeded.";
        }

        // ===================== Restaurants =====================
        private async Task<string> SeedRestaurantsAsync(string folder)
        {
            if (await _context.RestaurantInfos.AnyAsync())
                return "✅ Restaurants: already seeded, skipped.";

            var path = Path.Combine(folder, "Egyptian_Restaurants.csv");
            if (!File.Exists(path)) return "❌ Restaurants CSV not found.";

            var lines = await File.ReadAllTextAsync(path, System.Text.Encoding.UTF8);
            var rows = lines.Split('\n')
                            .Skip(1)
                            .Where(l => !string.IsNullOrWhiteSpace(l))
                            .ToList();

            var restaurants = new List<RestaurantInfo>();

            foreach (var line in rows)
            {
                try
                {
                    var cols = line.Split(',');
                    if (cols.Length < 7) continue;

                    restaurants.Add(new RestaurantInfo
                    {
                        Name = cols[1].Trim(),
                        Category = cols[2].Trim(),
                        City = cols[3].Trim(),
                        Area = cols[4].Trim(),
                        Longitude = double.TryParse(cols[5].Trim(), out var lng) ? lng : 0,
                        Latitude = double.TryParse(cols[6].Trim(), out var lat) ? lat : 0,
                        ImageUrl = cols.Length > 7 ? cols[7].Trim().Trim('"') : null,
                        MinPrice = cols.Length > 8 && decimal.TryParse(cols[8].Trim(), out var min) ? min : 0,
                        MaxPrice = cols.Length > 9 && decimal.TryParse(cols[9].Trim(), out var max) ? max : 0
                    });
                }
                catch { continue; }
            }

            await _context.RestaurantInfos.AddRangeAsync(restaurants);
            await _context.SaveChangesAsync();
            return $"✅ Restaurants: {restaurants.Count} records seeded.";
        }

        // ===================== Food Recipes =====================
        private async Task<string> SeedFoodRecipesAsync(string folder)
        {
            if (await _context.FoodRecipes.AnyAsync())
                return "✅ Food Recipes: already seeded, skipped.";

            var path = Path.Combine(folder, "Egypt_Food_Recipe_Data.csv");
            if (!File.Exists(path)) return "❌ Food CSV not found.";

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                BadDataFound = null,
                IgnoreBlankLines = true,
                PrepareHeaderForMatch = args => args.Header.Trim()
            };

            using var reader = new StreamReader(path, System.Text.Encoding.UTF8);
            using var csv = new CsvReader(reader, config);

            // ✅ بنقرأ كـ Dictionary عشان نتجنب مشكلة الـ spaces في الأسماء
            var records = new List<Dictionary<string, string>>();
            await csv.ReadAsync();
            csv.ReadHeader();
            var headers = csv.HeaderRecord!.Select(h => h.Trim()).ToList();

            while (await csv.ReadAsync())
            {
                var dict = new Dictionary<string, string>();
                foreach (var header in headers)
                {
                    if (!string.IsNullOrEmpty(header))
                        dict[header] = csv.GetField(headers.IndexOf(header)) ?? "";
                }
                records.Add(dict);
            }

            var recipes = new List<FoodRecipe>();
            foreach (var r in records)
            {
                try
                {
                    recipes.Add(new FoodRecipe
                    {
                        RecipeExternalId = r.GetValueOrDefault("Recipe_ID", ""),
                        Title = r.GetValueOrDefault("Recipe Title", ""),
                        Description = r.GetValueOrDefault("Description", ""),
                        Ingredients = r.GetValueOrDefault("Ingredients", ""),
                        Instructions = r.GetValueOrDefault("Instructions", ""),
                        ServeNotes = r.GetValueOrDefault("Serve", ""),
                        Categories = r.GetValueOrDefault("Categories", ""),
                        MinPriceEGP = decimal.TryParse(r.GetValueOrDefault("Min_EGP_Price", "0"), out var minP) ? minP : 0,
                        MaxPriceEGP = decimal.TryParse(r.GetValueOrDefault("Max_EGP_Price", "0"), out var maxP) ? maxP : 0,
                        PriceState = r.GetValueOrDefault("Price_State", ""),
                        ImageUrl = r.GetValueOrDefault("image_url", "")
                    });
                }
                catch { continue; }
            }

            await _context.FoodRecipes.AddRangeAsync(recipes);
            await _context.SaveChangesAsync();
            return $"✅ Food Recipes: {recipes.Count} records seeded.";
        }


        // في نفس ملف DataSeederRepo
        public class AttractionCsvRow
        {
            [CsvHelper.Configuration.Attributes.Name("place_id")]
            public string PlaceId { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("place_name")]
            public string PlaceName { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("category")]
            public string Category { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("city")]
            public string City { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("latitude")]
            public string Latitude { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("longitude")]
            public string Longitude { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("description")]
            public string Description { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("explore_duration_min")]
            public string ExploreDurationMin { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("opening_days_1")]
            public string OpeningDays { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("summer_opening_hours_day_shift")]
            public string SummerOpeningHours { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("winter_opening_hours_day_shift")]
            public string WinterOpeningHours { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("ramadan_opening_hours_day_shift")]
            public string RamadanOpeningHours { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("foreigner_adult_price_egp")]
            public string ForeignerAdultPrice { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("foreigner_student_price_egp")]
            public string ForeignerStudentPrice { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("arab_adult_price_egp")]
            public string ArabAdultPrice { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("arab_student_price_egp")]
            public string ArabStudentPrice { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("egyptian_adult_price_egp")]
            public string EgyptianAdultPrice { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("egyptian_student_price_egp")]
            public string EgyptianStudentPrice { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("garage_car_price_egp")]
            public string GarageCarPrice { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("garage_bus_price_egp")]
            public string GarageBusPrice { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("free_entry_policy")]
            public string FreeEntryPolicy { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("inclusive_ticket_access")]
            public string InclusiveTicketAccess { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("normalized_city")]
            public string NormalizedCity { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("Image_url")]
            public string ImageUrl { get; set; } = "";
        }

        public class RatingCsvRow
        {
            [CsvHelper.Configuration.Attributes.Name("user_id")]
            public string UserId { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("place_id")]
            public string PlaceId { get; set; } = "";

            [CsvHelper.Configuration.Attributes.Name("rating")]
            public string Rating { get; set; } = "";
        }

    }
}