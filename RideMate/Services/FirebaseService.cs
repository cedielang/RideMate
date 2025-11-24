using RideMate.Models;

namespace RideMate.Services
{
    // Firebase Service - Easy way to use Cloud Firestore
    // This is a simple wrapper that makes Firebase easy to use
    public class FirebaseService
    {
        // This is our connection to Cloud Firestore
        CloudFirestoreService firestore;

        // Constructor - runs when you create FirebaseService
        public FirebaseService()
        {
            // Connect to your Firebase project using the configuration
            firestore = new CloudFirestoreService();
            
            System.Diagnostics.Debug.WriteLine($"✓ FirebaseService initialized");
            System.Diagnostics.Debug.WriteLine($"  Project ID: {FirebaseConfig.ProjectId}");
            System.Diagnostics.Debug.WriteLine($"  App ID: {FirebaseConfig.AppId}");
        }

        // ========== DRIVER METHODS ==========

        // Add a new driver to Firebase
        // Example: await firebase.AddDriver(newDriver);
        public async Task<bool> AddDriver(Driver driver)
        {
            return await firestore.AddDriver(driver);
        }

        // Find a driver by phone and password (for login)
        // Example: Driver driver = await firebase.FindDriver("+639123456789", "password123");
        public async Task<Driver> FindDriver(string phone, string password)
        {
            return await firestore.FindDriver(phone, password);
        }

        // Check if a phone number is already registered as driver
        // Example: bool exists = await firebase.DriverPhoneExists("+639123456789");
        public async Task<bool> DriverPhoneExists(string phone)
        {
            return await firestore.DriverPhoneExists(phone);
        }

        // Update driver's online/offline status
        // Example: await firebase.UpdateDriverStatus(driverId, true);
        public async Task<bool> UpdateDriverStatus(string driverId, bool isOnline)
        {
            return await firestore.UpdateDriverStatus(driverId, isOnline);
        }

        // Update driver's GPS location
        // Example: await firebase.UpdateDriverLocation(driverId, 14.5995, 120.9842);
        public async Task<bool> UpdateDriverLocation(string driverId, double latitude, double longitude)
        {
            return await firestore.UpdateDriverLocation(driverId, latitude, longitude);
        }

        // Get all drivers who are currently online
        // Example: List<Driver> drivers = await firebase.GetOnlineDrivers();
        public async Task<List<Driver>> GetOnlineDrivers()
        {
            return await firestore.GetOnlineDrivers();
        }

        // ========== PASSENGER METHODS ==========

        // Add a new passenger to Firebase
        // Example: await firebase.AddPassenger(newPassenger);
        public async Task<bool> AddPassenger(Passenger passenger)
        {
            return await firestore.AddPassenger(passenger);
        }

        // Find a passenger by phone and password (for login)
        // Example: Passenger passenger = await firebase.FindPassenger("+639123456789", "password123");
        public async Task<Passenger> FindPassenger(string phone, string password)
        {
            return await firestore.FindPassenger(phone, password);
        }

        // Check if a phone number is already registered as passenger
        // Example: bool exists = await firebase.PassengerPhoneExists("+639123456789");
        public async Task<bool> PassengerPhoneExists(string phone)
        {
            return await firestore.PassengerPhoneExists(phone);
        }

        // Update passenger's GPS location
        // Example: await firebase.UpdatePassengerLocation(passengerId, 14.5995, 120.9842);
        public async Task<bool> UpdatePassengerLocation(string passengerId, double latitude, double longitude)
        {
            return await firestore.UpdatePassengerLocation(passengerId, latitude, longitude);
        }

        // Get all passengers waiting for a ride
        // Example: List<Passenger> passengers = await firebase.GetWaitingPassengers();
        public async Task<List<Passenger>> GetWaitingPassengers()
        {
            return await firestore.GetWaitingPassengers();
        }

        // ========== PASSWORD RESET METHODS ==========

        // Reset driver password
        // Example: await firebase.ResetDriverPassword("+639123456789", "newPassword123");
        public async Task<bool> ResetDriverPassword(string phone, string newPassword)
        {
            return await firestore.ResetDriverPassword(phone, newPassword);
        }

        // Reset passenger password
        // Example: await firebase.ResetPassengerPassword("+639123456789", "newPassword123");
        public async Task<bool> ResetPassengerPassword(string phone, string newPassword)
        {
            return await firestore.ResetPassengerPassword(phone, newPassword);
        }

        // ========== VERIFICATION METHODS ==========

        // Verify account (mark as verified)
        // Example: await firebase.VerifyAccount(userId, "Driver");
        public async Task<bool> VerifyAccount(string userId, string userRole)
        {
            return await firestore.VerifyAccount(userId, userRole);
        }
    }

    // ========== SIMPLE DATABASE (For Backward Compatibility) ==========
    // This class makes your old code work with the new Firebase system
    // You don't need to change your existing code!
    public class SimpleDatabase
    {
        // This uses FirebaseService internally
        FirebaseService firebase;

        // Constructor
        public SimpleDatabase()
        {
            firebase = new FirebaseService();
        }

        // ========== DRIVER METHODS ==========

        // Add driver (works like before, but saves to Firebase now!)
        public bool AddDriver(Driver driver)
        {
            // Call the async method and wait for it
            Task<bool> task = firebase.AddDriver(driver);
            task.Wait(); // Wait for Firebase to finish
            return task.Result; // Return the result
        }

        // Find driver (works like before, but searches Firebase now!)
        public Driver FindDriver(string phone, string password)
        {
            Task<Driver> task = firebase.FindDriver(phone, password);
            task.Wait();
            return task.Result;
        }

        // Check if driver phone exists
        public bool DriverPhoneExists(string phone)
        {
            Task<bool> task = firebase.DriverPhoneExists(phone);
            task.Wait();
            return task.Result;
        }

        // Update driver status
        public bool UpdateDriverStatus(string driverId, bool isOnline)
        {
            Task<bool> task = firebase.UpdateDriverStatus(driverId, isOnline);
            task.Wait();
            return task.Result;
        }

        // ========== PASSENGER METHODS ==========

        // Add passenger (works like before, but saves to Firebase now!)
        public bool AddPassenger(Passenger passenger)
        {
            Task<bool> task = firebase.AddPassenger(passenger);
            task.Wait();
            return task.Result;
        }

        // Find passenger (works like before, but searches Firebase now!)
        public Passenger FindPassenger(string phone, string password)
        {
            Task<Passenger> task = firebase.FindPassenger(phone, password);
            task.Wait();
            return task.Result;
        }

        // Check if passenger phone exists
        public bool PassengerPhoneExists(string phone)
        {
            Task<bool> task = firebase.PassengerPhoneExists(phone);
            task.Wait();
            return task.Result;
        }
    }
}
