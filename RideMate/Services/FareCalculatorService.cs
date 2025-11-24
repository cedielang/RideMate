using System;

namespace RideMate.Services
{
    // Fare Calculator Service - Calculates ride fare in Philippine Peso
    public class FareCalculatorService
    {
        // Philippine taxi/ride-sharing rates (based on typical rates)
        private const double BASE_FARE = 40.00;           // Starting fare (₱40)
        private const double RATE_PER_KM = 13.50;         // Per kilometer (₱13.50/km)
        private const double RATE_PER_MINUTE = 2.00;      // Per minute waiting/slow traffic (₱2/min)
        private const double MINIMUM_FARE = 40.00;        // Minimum fare (₱40)
        private const double BOOKING_FEE = 0.00;          // Optional booking fee (₱0)

        // ⚠️ DEPRECATED: Standard fare models require distance AND time from the map API.
        // I recommend removing this method in a production app.
        public double CalculateFareByDistance(double distanceInKm)
        {
            // Base fare + (distance × rate per km)
            double fare = BASE_FARE + (distanceInKm * RATE_PER_KM);

            // Apply minimum fare
            if (fare < MINIMUM_FARE)
            {
                fare = MINIMUM_FARE;
            }

            return Math.Round(fare, 2);
        }

        // =======================================================
        // CORE CALCULATION METHODS (ENHANCED FOR SURGE)
        // =======================================================

        /// <summary>
        /// Calculates total fare based on distance, time, and an optional surge multiplier.
        /// </summary>
        public double CalculateFare(double distanceInKm, double timeInMinutes, double surgeMultiplier = 1.0)
        {
            // Calculate components
            double distanceFare = distanceInKm * RATE_PER_KM;
            double timeFare = timeInMinutes * RATE_PER_MINUTE;

            // Calculate Pre-Surge Total (Base + Distance + Time + Booking Fee)
            double preSurgeTotal = BASE_FARE + distanceFare + timeFare + BOOKING_FEE;

            // Apply surge pricing
            double totalFare = preSurgeTotal * surgeMultiplier;

            // Apply minimum fare
            if (totalFare < MINIMUM_FARE)
            {
                totalFare = MINIMUM_FARE;
            }

            return Math.Round(totalFare, 2);
        }

        /// <summary>
        /// Calculates total fare and provides a full breakdown, including surge.
        /// </summary>
        public FareBreakdown CalculateFareWithBreakdown(double distanceInKm, double timeInMinutes, double surgeMultiplier = 1.0)
        {
            // 1. Calculate base components
            double distanceFare = distanceInKm * RATE_PER_KM;
            double timeFare = timeInMinutes * RATE_PER_MINUTE;
            double baseTotal = BASE_FARE + distanceFare + timeFare + BOOKING_FEE;

            // 2. Apply surge
            double total = baseTotal * surgeMultiplier;

            // 3. Apply minimum fare
            if (total < MINIMUM_FARE)
            {
                total = MINIMUM_FARE;
            }

            // 4. Calculate surge amount for breakdown display
            double surgeAmount = total - baseTotal;
            if (surgeAmount < 0) surgeAmount = 0; // Ensures surge amount is not negative if minimum fare applies

            return new FareBreakdown
            {
                BaseFare = Math.Round(BASE_FARE, 2),
                DistanceFare = Math.Round(distanceFare, 2),
                TimeFare = Math.Round(timeFare, 2),
                BookingFee = Math.Round(BOOKING_FEE, 2),

                // New Surge Properties
                SurgeMultiplier = Math.Round(surgeMultiplier, 2),
                SurgeAmount = Math.Round(surgeAmount, 2),

                Subtotal = Math.Round(baseTotal, 2), // Subtotal before final adjustments (like minimum/surge)
                Total = Math.Round(total, 2),

                DistanceInKm = Math.Round(distanceInKm, 2),
                TimeInMinutes = Math.Round(timeInMinutes, 2)
            };
        }

        // =======================================================
        // UTILITIES (UNCHANGED/REFINED)
        // =======================================================

        // Calculate distance between two coordinates (Haversine formula)
        // Note: Use distance from Map API for better accuracy in final fare!
        public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371.0; // Earth's radius in kilometers

            double lat1Rad = DegreesToRadians(lat1);
            double lon1Rad = DegreesToRadians(lon1);
            double lat2Rad = DegreesToRadians(lat2);
            double lon2Rad = DegreesToRadians(lon2);

            double dLat = lat2Rad - lat1Rad;
            double dLon = lon2Rad - lon1Rad;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distance = R * c; // Distance in kilometers

            return Math.Round(distance, 2);
        }

        // Estimate time based on distance (average speed 30 km/h in city)
        public double EstimateTime(double distanceInKm)
        {
            const double AVERAGE_SPEED_KMH = 30.0; // Average city speed
            double timeInHours = distanceInKm / AVERAGE_SPEED_KMH;
            double timeInMinutes = timeInHours * 60;

            return Math.Round(timeInMinutes, 2);
        }

        // Convert degrees to radians
        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        // Get fare rates info
        public string GetRatesInfo()
        {
            return $"Base Fare: ₱{BASE_FARE:F2}\n" +
                   $"Per Kilometer: ₱{RATE_PER_KM:F2}/km\n" +
                   $"Per Minute: ₱{RATE_PER_MINUTE:F2}/min\n" +
                   $"Minimum Fare: ₱{MINIMUM_FARE:F2}\n" +
                   $"Booking Fee: ₱{BOOKING_FEE:F2}";
        }
    }

    // Fare breakdown model (MODIFIED FOR SURGE)
    public class FareBreakdown
    {
        public double BaseFare { get; set; }
        public double DistanceFare { get; set; }
        public double TimeFare { get; set; }
        public double BookingFee { get; set; }

        // New Surge Properties
        public double SurgeMultiplier { get; set; }
        public double SurgeAmount { get; set; } // The monetary increase due to surge

        public double Subtotal { get; set; }
        public double Total { get; set; }
        public double DistanceInKm { get; set; }
        public double TimeInMinutes { get; set; }

        public override string ToString()
        {
            string surgeLine = SurgeMultiplier > 1.0 ? $"Surge (x{SurgeMultiplier:F1}): ₱{SurgeAmount:F2}\n" : "";

            return $"Distance: {DistanceInKm:F2} km\n" +
                   $"Estimated Time: {TimeInMinutes:F0} minutes\n\n" +
                   $"Base Fare: ₱{BaseFare:F2}\n" +
                   $"Distance Fare: ₱{DistanceFare:F2}\n" +
                   $"Time Fare: ₱{TimeFare:F2}\n" +
                   $"Booking Fee: ₱{BookingFee:F2}\n" +
                   $"─────────────────\n" +
                   $"{surgeLine}" +
                   $"TOTAL: ₱{Total:F2}";
        }
    }
}