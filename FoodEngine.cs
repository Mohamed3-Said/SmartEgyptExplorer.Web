using System;

public class Class1
{
    public class FoodEngine
    {
        // حالياً بنستخدم Logic صاحبك في الاختيار العشوائي الذكي
        // ملاحظة: لما تجيب ملف الـ CSV هنخلي الـ Logic ده يقرأ منه
        public dynamic GetRestaurantRecommendation(string city, string tier = "Standard")
        {
            decimal basePrice = tier.ToLower() switch
            {
                "budget" => 150,
                "luxury" => 800,
                _ => 300
            };

            return new
            {
                Title = $"Local Restaurant in {city}",
                PriceEgp = basePrice,
                Description = "Authentic Egyptian food based on your tier"
            };
        }

        public string GetCulturalDishRecommendation()
        {
            string[] dishes = { "Koshary", "Molokhia", "Falafel", "Fatta", "Sayadeya" };
            return dishes[new Random().Next(dishes.Length)];
        }
    }
}
