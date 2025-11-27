using System;
using System.Collections.Generic;
using System.Linq; // Required for tax calculations later

namespace RideMate.Services
{
    // Fare Calculator Service - Calculates ride fare in Philippine Peso
    public class FareCalculatorService
    {
        // Philippine taxi/ride-sharing rates (Based on lower, more competitive estimates)
        private const double BASE_FARE = 35.00;           // Starting fare (₱35 - slightly reduced)
        private const double RATE_PER_KM = 12.00;         // Per kilometer (₱12.00/km - reduced)
        private const double RATE_PER_MINUTE = 1.80;      // Per minute waiting/slow traffic (₱1.80/min - reduced)
        private const double MINIMUM_FARE = 40.00;        // Minimum fare (₱40)
        private const double BOOKING_FEE = 5.00;          // Small booking fee added (₱5)

        // BUSINESS LOGIC CONSTANTS
        private const double VAT_RATE = 0.12; // 12% VAT (Value Added Tax) in the Philippines
        private const double COMPANY_COMMISSION_RATE = 0.30; // 30% Company Commission

        // ⚠️ DEPRECATED: Standard fare models require distance AND time from the map API.
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
        // CORE CALCULATION METHODS
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
            if (surgeAmount < 0) surgeAmount = 0;

            return new FareBreakdown
            {
                BaseFare = Math.Round(BASE_FARE, 2),
                DistanceFare = Math.Round(distanceFare, 2),
                TimeFare = Math.Round(timeFare, 2),
                BookingFee = Math.Round(BOOKING_FEE, 2),

                SurgeMultiplier = Math.Round(surgeMultiplier, 2),
                SurgeAmount = Math.Round(surgeAmount, 2),

                Subtotal = Math.Round(baseTotal, 2),
                Total = Math.Round(total, 2),

                DistanceInKm = Math.Round(distanceInKm, 2),
                TimeInMinutes = Math.Round(timeInMinutes, 2)
            };
        }

        // =======================================================
        // ADMIN PANEL/BACKEND LOGIC (Tax and Payout)
        // =======================================================

        /// <summary>
        /// Calculates the driver's net payout and the company's commission, 
        /// including mandatory VAT on the commission.
        /// </summary>
        /// <param name="totalFareReceived">The total amount the passenger paid.</param>
        public PayoutDetails CalculateDriverPayoutAndTax(double totalFareReceived)
        {
            // 1. Calculate the Company's Commission (30%)
            double companyCommission = totalFareReceived * COMPANY_COMMISSION_RATE;

            // 2. Calculate VAT (12%) on the commission amount
            // VAT applies only to the service fee (commission) charged by the company.
            double vatAmount = companyCommission * VAT_RATE;

            // 3. Net Commission for Company (After VAT calculation)
            double netCommission = companyCommission + vatAmount;

            // 4. Driver's Payout (What the driver keeps)
            double driverPayout = totalFareReceived - netCommission;

            return new PayoutDetails
            {
                TotalFare = Math.Round(totalFareReceived, 2),
                DriverPayout = Math.Round(driverPayout, 2),
                CompanyCommission = Math.Round(companyCommission, 2),
                VatOnCommission = Math.Round(vatAmount, 2),
                NetCompanyRevenue = Math.Round(netCommission, 2)
            };
        }

      

        // Calculate distance between two coordinates (Haversine formula)
        public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371.0;

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

            double distance = R * c;

            return Math.Round(distance, 2);
        }

        // Estimate time based on distance (average speed 30 km/h in city)
        public double EstimateTime(double distanceInKm)
        {
            const double AVERAGE_SPEED_KMH = 30.0;
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

    // Fare breakdown model
    public class FareBreakdown
    {
        public double BaseFare { get; set; }
        public double DistanceFare { get; set; }
        public double TimeFare { get; set; }
        public double BookingFee { get; set; }

        // New Surge Properties
        public double SurgeMultiplier { get; set; }
        public double SurgeAmount { get; set; }

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

    // Model for Admin/Backend Payout Calculation
    public class PayoutDetails
    {
        public double TotalFare { get; set; }
        public double CompanyCommission { get; set; }
        public double VatOnCommission { get; set; }
        public double NetCompanyRevenue { get; set; } // Commission + VAT
        public double DriverPayout { get; set; } // Driver keeps this
    }
}