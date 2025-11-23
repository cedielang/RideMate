using Google.Cloud.Firestore;

namespace RideMate.Models
{
    // Driver class with Firestore attributes
    [FirestoreData]
    public class Driver
    {
        // Basic Info
        [FirestoreProperty]
        public string Id { get; set; } = "";
        
        [FirestoreProperty]
        public string Name { get; set; } = "";
        
        [FirestoreProperty]
        public string Phone { get; set; } = "";
        
        [FirestoreProperty]
        public string Email { get; set; } = "";
        
        [FirestoreProperty]
        public string Password { get; set; } = "";
        
        // Driver Specific Info
        [FirestoreProperty]
        public string LicenseNumber { get; set; } = "";
        
        [FirestoreProperty]
        public string VehicleType { get; set; } = "";
        
        [FirestoreProperty]
        public string VehicleModel { get; set; } = "";
        
        [FirestoreProperty]
        public string PlateNumber { get; set; } = "";
        
        // Status
        [FirestoreProperty]
        public bool IsOnline { get; set; } = false;
        
        // Verification
        [FirestoreProperty]
        public bool IsVerified { get; set; } = false;
        
        [FirestoreProperty]
        public string VerificationCode { get; set; } = "";
        
        // Location
        [FirestoreProperty]
        public double Latitude { get; set; } = 0.0;
        
        [FirestoreProperty]
        public double Longitude { get; set; } = 0.0;
    }

    // Passenger class with Firestore attributes
    [FirestoreData]
    public class Passenger
    {
        // Basic Info
        [FirestoreProperty]
        public string Id { get; set; } = "";
        
        [FirestoreProperty]
        public string Name { get; set; } = "";
        
        [FirestoreProperty]
        public string Phone { get; set; } = "";
        
        [FirestoreProperty]
        public string Email { get; set; } = "";
        
        [FirestoreProperty]
        public string Password { get; set; } = "";
        
        // Passenger Specific Info
        [FirestoreProperty]
        public string Address { get; set; } = "";
        
        // Verification
        [FirestoreProperty]
        public bool IsVerified { get; set; } = false;
        
        [FirestoreProperty]
        public string VerificationCode { get; set; } = "";
        
        // Location
        [FirestoreProperty]
        public double Latitude { get; set; } = 0.0;
        
        [FirestoreProperty]
        public double Longitude { get; set; } = 0.0;
        
        // Ride Status
        [FirestoreProperty]
        public bool IsWaitingForRide { get; set; } = false;
    }
}
