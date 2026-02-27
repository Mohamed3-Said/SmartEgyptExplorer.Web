using CsvHelper;
using CsvHelper.Configuration.Attributes;
using DomainLayer.Models.PlaceModule;
using System.Globalization;
using System.IO;
using System.Text;

namespace DomainLayer.Helpers
{
    // 1. الكلاس اللي بيمثل شكل السطر في ملف الإكسيل

    public class AttractionCsvModel
    {
        [Name("place_name")]
        public string Name { get; set; } = string.Empty;

        [Name("category")]
        public string Category { get; set; } = string.Empty;

        [Name("normalized_city")]
        public string NormalizedCity { get; set; } = string.Empty;

        [Name("latitude")]
        public double Latitude { get; set; }

        [Name("longitude")]
        public double Longitude { get; set; }

        [Name("description")]
        public string Description { get; set; } = string.Empty;

        [Name("Image_url")]
        public string ImageUrl { get; set; } = string.Empty;

        [Name("egyptian_adult_price_egp")]
        public decimal Price { get; set; }

        [Name("opening_hours_day_shift")]
        public string OpeningHours { get; set; } = string.Empty;
    }
    public class RecommenderEngine
    {
        // 2. الميثود الأساسية اللي بترجع لستة من موديل الـ Place بتاعك
        public List<Place> GetScoredAttractions(List<string> userInterests, string city)
        {
            var csvRecords = LoadAttractionsFromCsv();

            // 1. تنظيف اسم المدينة اللي جاي من اليوزر
            var targetCity = city.Trim().ToLower();

            // 2. الفلترة مع التأكد من حذف أي مسافات مخفية في ملف الإكسيل
            // الفلترة باستخدام NormalizedCity لضمان مطابقة دقيقة
            var filtered = csvRecords
               .Where(a => a.NormalizedCity.Trim().ToLower().Contains(targetCity))
               .ToList();

            // Debugging :
            Console.WriteLine($"🔍 Found {filtered.Count} attractions for city: {city}");

            if (filtered.Count == 0)
            {
                // لو مش لاقي، ابعت أول 5 أماكن في الداتا عموماً بدل ما تبعت لستة فاضية للـ AI 
                // أو ارمي Exception عشان تعرف إن المشكلة في اسم المدينة
                return new List<Place>();
            }

            // حساب الـ Score وتحويل البيانات لـ Place Objects
            return filtered
                .Select(a => new {
                    Record = a,
                    Score = userInterests.Count(interest =>
                        a.Category.Contains(interest, StringComparison.OrdinalIgnoreCase))
                })
                .OrderByDescending(x => x.Score)
                .Take(12)
                .Select(x => new Place
                {
                    Name = x.Record.Name,
                    Description = x.Record.Description,
                    City = x.Record.NormalizedCity,
                    Category = "Attraction", // تصنيف عام للمكان
                    ImageURL = x.Record.ImageUrl ?? "default_image.jpg",
                    Latitude = x.Record.Latitude,
                    Longitude = x.Record.Longitude,
                    OpeningHours = x.Record.OpeningHours ?? "09:00 AM - 05:00 PM",

                    // ربط الـ Attraction بالـ Place (علاقة 1 لـ 1)
                    Attraction = new Attraction
                    {
                        AttractionType = x.Record.Category,
                        Price = x.Record.Price
                    }
                })
                .ToList();
        }

        // 3. ميثود قراءة ملف الـ CSV
        private List<AttractionCsvModel> LoadAttractionsFromCsv()
        {
            // 1. تحديد المسار: بنجرب المسار المباشر وفولدر Data
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(baseDir, "Data", "Attractions.csv");

            // Debugging: عشان تشوف المسار في الـ Console وتتأكد بنفسك
            Console.WriteLine($"📂 Checking CSV at: {filePath}");

            if (!File.Exists(filePath))
            {
                // محاولة ثانية: لو البرنامج شغال من الـ Root مباشرة
                filePath = Path.Combine(baseDir, "Attractions.csv");
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("❌ CRITICAL: Attractions.csv NOT FOUND in bin or Data folder!");
                    return new List<AttractionCsvModel>();
                }
            }

            var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                BadDataFound = null,
                MissingFieldFound = null,
                HeaderValidated = null,
                PrepareHeaderForMatch = args => args.Header.ToLower().Trim(),
            };

            try
            {
                using (var reader = new StreamReader(filePath, Encoding.UTF8))
                using (var csv = new CsvHelper.CsvReader(reader, config))
                {
                    var records = csv.GetRecords<AttractionCsvModel>().ToList();
                    Console.WriteLine($"✅ Successfully loaded {records.Count} records from CSV.");
                    return records;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 CSV Reading Error: {ex.Message}");
                return new List<AttractionCsvModel>();
            }
        }
    }
}