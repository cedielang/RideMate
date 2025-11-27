using System;


namespace RideMate.Models
{
   
    public class Driver
    {
        public string Role { get; set; } = "driver";
        // Basic Info
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";

       
        public string Password { get; set; } = "";

        // Driver Specific Info
        public string LicenseNumber { get; set; } = "";
        public string VehicleType { get; set; } = "";
        public string VehicleModel { get; set; } = "";
        public string PlateNumber { get; set; } = "";

        // Status
        public bool IsOnline { get; set; } = false;

        // Verification
        public bool IsVerified { get; set; } = false;
        public string VerificationCode { get; set; } = "";

        // Location
        public double Latitude { get; set; } = 0.0;
        public double Longitude { get; set; } = 0.0;

        // Admin Blocking Status (Crucial for Admin Panel integration)
        public bool IsBlocked { get; set; } = false;
    }

    // Passenger class without explicit Firestore attributes
    public class Passenger
    {
        public string Role { get; set; } = "passenger";
        // Basic Info
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";

        // IMPORTANT: Password is now handled by Firebase Auth.
        public string Password { get; set; } = "";

        // Passenger Specific Info
        public string Address { get; set; } = "";

        // Verification
        public bool IsVerified { get; set; } = false;
        public string VerificationCode { get; set; } = "";

        // Location
        public double Latitude { get; set; } = 0.0;
        public double Longitude { get; set; } = 0.0;

        // Ride Status
        public bool IsWaitingForRide { get; set; } = false;

        // Admin Blocking Status (Crucial for Admin Panel integration)
        public bool IsBlocked { get; set; } = false;
    }
}