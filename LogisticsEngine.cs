using System;
public class Class1
{
    public class LogisticsEngine
    {
        // حساب المسافة بين نقطتين (Haversine Formula)
        public double CalculateDistanceKm(double lat1, double lon1, double lat2, double lon2)
        {
            try
            {
                if (lat1 == 0 || lon1 == 0 || lat2 == 0 || lon2 == 0) return 20.0;

                double R = 6371; // نصف قطر الأرض بالكيلومتر
                double dLat = ToRadians(lat2 - lat1);
                double dLon = ToRadians(lon2 - lon1);

                double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                           Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                           Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                return R * c;
            }
            catch { return 20.0; }
        }

        private double ToRadians(double angle) => (Math.PI / 180) * angle;

        // حساب الوقت المتوقع بناءً على نوع المواصلات
        public int CalculateTravelTimeMins(double distKm, string transportType = "Car")
        {
            var speeds = new Dictionary<string, int>
        {
            { "car", 30 }, { "uber", 30 }, { "taxi", 30 },
            { "bus", 20 }, { "walking", 4 }, { "metro", 25 }
        };

            int speed = speeds.GetValueOrDefault(transportType.ToLower(), 30);
            return (int)((distKm / speed) * 60) + 15; // +15 دقيقة زحمة مصر
        }

        // حساب تكلفة المشوار (Uber/Taxi)
        public decimal CalculateRideCost(double distKm, int durationMins)
        {
            // معادلة صاحبك: فتحة العداد + الكيلومترات + دقايق الانتظار
            return Math.Round((decimal)(25.0 + (distKm * 10.0) + (durationMins * 2.0) + 10.0), 2);
        }
    }
}
