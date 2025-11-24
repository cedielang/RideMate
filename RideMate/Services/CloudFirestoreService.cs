using Google.Cloud.Firestore;
using RideMate.Models;
using System.Diagnostics;
using BCrypt.Net; // Import the hashing library

namespace RideMate.Services
{
    // Simple Firestore Service - connects to your Firebase database
    public class CloudFirestoreService
    {
        // Connection to Firebase
        private readonly FirestoreDb database;

        // Your collection name
        private const string COLLECTION_NAME = "database_information"; // Contains all users (drivers/passengers)
        private const string RIDES_COLLECTION = "rides";
        private const string PAYMENTS_COLLECTION = "payments";
        private const string RATINGS_COLLECTION = "ratings";

        // =======================================================
        // I. INITIALIZATION & SECURITY (MODIFIED)
        // =======================================================

        public CloudFirestoreService()
        {
            const string ProjectId = "ridemate-7c600";

            try
            {
                // Hardcode the specific Project ID here
                database = FirestoreDb.Create(ProjectId);
                Debug.WriteLine($"✓ Connected to Firestore: {ProjectId}");
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error: {error.Message}");
                throw new Exception("Failed to initialize Firestore. Ensure the GOOGLE_APPLICATION_CREDENTIALS environment variable is correctly set on the server.", error);
            }
        }

        // --- Hashing Helper ---
        // Uses BCrypt to securely hash and verify passwords.
        private string HashPassword(string password)
        {
            // Use BCrypt to hash the password securely
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hash)
        {
            // Use BCrypt to verify the password against the stored hash
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        // ========== DRIVER METHODS (MODIFIED FOR HASHING) ==========

        // Add new driver
        public async Task<bool> AddDriver(Driver driver)
        {
            try
            {
                // Create unique ID
                driver.Id = "uid" + Guid.NewGuid().ToString().Substring(0, 6);

                // CRITICAL CHANGE: HASH THE PASSWORD
                string hashedPassword = HashPassword(driver.Password);

                CollectionReference collection = database.Collection(COLLECTION_NAME);

                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "role", "driver" },
                    { "name", driver.Name },
                    { "email", driver.Email },
                    { "phone", driver.Phone },
                    { "password", hashedPassword }, // Store the HASH
                    { "licenseNumber", driver.LicenseNumber },
                    { "vehicleType", driver.VehicleType },
                    { "vehicleModel", driver.VehicleModel },
                    { "plateNumber", driver.PlateNumber },
                    { "createdAt", FieldValue.ServerTimestamp } // Use server timestamp
                };

                await collection.Document(driver.Id).SetAsync(data);
                Debug.WriteLine($"✓ Driver saved: {driver.Id}");
                return true;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error adding driver: {error.Message}");
                return false;
            }
        }

        // Find driver by phone and password (MODIFIED FOR HASHING)
        public async Task<Driver?> FindDriver(string phone, string password)
        {
            try
            {
                CollectionReference collection = database.Collection(COLLECTION_NAME);

                // Search for driver by role and phone (NO PASSWORD IN QUERY)
                // REQUIRES a composite index on (role, phone)
                Query query = collection
                    .WhereEqualTo("role", "driver")
                    .WhereEqualTo("phone", phone);

                QuerySnapshot results = await query.GetSnapshotAsync();

                if (results.Documents.Count > 0)
                {
                    DocumentSnapshot doc = results.Documents[0];
                    string storedHash = doc.GetValue<string>("password");

                    // CRITICAL CHANGE: VERIFY PASSWORD HASH
                    if (VerifyPassword(password, storedHash))
                    {
                        // Create Driver object from Firestore data
                        Driver driver = new Driver
                        {
                            Id = doc.Id,
                            Name = doc.ContainsField("name") ? doc.GetValue<string>("name") : "",
                            Email = doc.ContainsField("email") ? doc.GetValue<string>("email") : "",
                            Phone = doc.ContainsField("phone") ? doc.GetValue<string>("phone") : "",
                            // DO NOT RETURN THE HASHED PASSWORD TO THE CLIENT
                            Password = "",
                            LicenseNumber = doc.ContainsField("licenseNumber") ? doc.GetValue<string>("licenseNumber") : "",
                            VehicleType = doc.ContainsField("vehicleType") ? doc.GetValue<string>("vehicleType") : "",
                            VehicleModel = doc.ContainsField("vehicleModel") ? doc.GetValue<string>("vehicleModel") : "",
                            PlateNumber = doc.ContainsField("plateNumber") ? doc.GetValue<string>("plateNumber") : ""
                        };

                        Debug.WriteLine($"✓ Driver found: {driver.Name}");
                        return driver;
                    }
                    // If password verification fails, fall through to return null
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error finding driver: {error.Message}");
            }

            return null;
        }

        // Check if driver phone exists (UNCHANGED)
        public async Task<bool> DriverPhoneExists(string phone)
        {
            // ... (Logic is correct, only needs the Debug reference changed to Debug)
            // ...
            try
            {
                CollectionReference collection = database.Collection(COLLECTION_NAME);
                Query query = collection
                    .WhereEqualTo("role", "driver")
                    .WhereEqualTo("phone", phone);

                QuerySnapshot results = await query.GetSnapshotAsync();
                return results.Documents.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        // Update driver status (UNCHANGED)
        public async Task<bool> UpdateDriverStatus(string driverId, bool isOnline)
        {
            // ... (Logic is correct)
            try
            {
                DocumentReference doc = database.Collection(COLLECTION_NAME).Document(driverId);
                await doc.UpdateAsync("isOnline", isOnline);
                return true;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error updating status: {error.Message}");
                return false;
            }
        }

        // Update driver location (UNCHANGED)
        public async Task<bool> UpdateDriverLocation(string driverId, double latitude, double longitude)
        {
            // ... (Logic is correct)
            try
            {
                DocumentReference doc = database.Collection(COLLECTION_NAME).Document(driverId);

                Dictionary<string, object> updates = new Dictionary<string, object>
                {
                    { "latitude", latitude },
                    { "longitude", longitude }
                };

                await doc.UpdateAsync(updates);
                return true;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error updating location: {error.Message}");
                return false;
            }
        }

        // Get all online drivers (UNCHANGED)
        public async Task<List<Driver>> GetOnlineDrivers()
        {
            // ... (Logic is correct, only needs Debug reference change)
            try
            {
                CollectionReference collection = database.Collection(COLLECTION_NAME);
                Query query = collection
                    .WhereEqualTo("role", "driver")
                    .WhereEqualTo("isOnline", true);

                QuerySnapshot results = await query.GetSnapshotAsync();
                List<Driver> drivers = new List<Driver>();

                foreach (DocumentSnapshot doc in results.Documents)
                {
                    // Assuming you have the Driver model defined correctly to accept these fields
                    Driver driver = doc.ConvertTo<Driver>(); // Use ConvertTo<T>() if using [FirestoreProperty] attributes
                    drivers.Add(driver);
                }

                return drivers;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error getting drivers: {error.Message}");
                return new List<Driver>();
            }
        }

        // ========== PASSENGER METHODS (MODIFIED FOR HASHING) ==========

        // Add new passenger (MODIFIED FOR HASHING)
        public async Task<bool> AddPassenger(Passenger passenger)
        {
            try
            {
                passenger.Id = "uid" + Guid.NewGuid().ToString().Substring(0, 6);
                string hashedPassword = HashPassword(passenger.Password); // CRITICAL CHANGE: HASH THE PASSWORD

                CollectionReference collection = database.Collection(COLLECTION_NAME);

                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "role", "passenger" },
                    { "name", passenger.Name },
                    { "email", passenger.Email },
                    { "phone", passenger.Phone },
                    { "password", hashedPassword }, // Store the HASH
                    { "createdAt", FieldValue.ServerTimestamp } // Use server timestamp
                };

                await collection.Document(passenger.Id).SetAsync(data);
                Debug.WriteLine($"✓ Passenger saved: {passenger.Id}");
                return true;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error adding passenger: {error.Message}");
                return false;
            }
        }

        // Find passenger by phone and password (MODIFIED FOR HASHING)
        public async Task<Passenger?> FindPassenger(string phone, string password)
        {
            try
            {
                CollectionReference collection = database.Collection(COLLECTION_NAME);

                // Search for passenger by role and phone (NO PASSWORD IN QUERY)
                // REQUIRES a composite index on (role, phone)
                Query query = collection
                    .WhereEqualTo("role", "passenger")
                    .WhereEqualTo("phone", phone);

                QuerySnapshot results = await query.GetSnapshotAsync();

                if (results.Documents.Count > 0)
                {
                    DocumentSnapshot doc = results.Documents[0];
                    string storedHash = doc.GetValue<string>("password");

                    // CRITICAL CHANGE: VERIFY PASSWORD HASH
                    if (VerifyPassword(password, storedHash))
                    {
                        // Create Passenger object (excluding hash from the return object)
                        Passenger passenger = doc.ConvertTo<Passenger>();
                        passenger.Password = "";

                        Debug.WriteLine($"✓ Passenger found: {passenger.Name}");
                        return passenger;
                    }
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error finding passenger: {error.Message}");
            }

            return null;
        }

        // Check if passenger phone exists (UNCHANGED)
        public async Task<bool> PassengerPhoneExists(string phone)
        {
            // ... (Logic is correct, needs Debug reference change)
            try
            {
                CollectionReference collection = database.Collection(COLLECTION_NAME);
                Query query = collection
                    .WhereEqualTo("role", "passenger")
                    .WhereEqualTo("phone", phone);

                QuerySnapshot results = await query.GetSnapshotAsync();
                return results.Documents.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        // Update passenger location (UNCHANGED)
        public async Task<bool> UpdatePassengerLocation(string passengerId, double latitude, double longitude)
        {
            // ... (Logic is correct, needs Debug reference change)
            try
            {
                DocumentReference doc = database.Collection(COLLECTION_NAME).Document(passengerId);

                Dictionary<string, object> updates = new Dictionary<string, object>
                {
                    { "latitude", latitude },
                    { "longitude", longitude }
                };

                await doc.UpdateAsync(updates);
                return true;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error updating location: {error.Message}");
                return false;
            }
        }

        // Get waiting passengers (UNCHANGED)
        public async Task<List<Passenger>> GetWaitingPassengers()
        {
            // ... (Logic is correct, needs Debug reference change and ConvertTo<T>)
            try
            {
                CollectionReference collection = database.Collection(COLLECTION_NAME);
                Query query = collection
                    .WhereEqualTo("role", "passenger")
                    .WhereEqualTo("isWaitingForRide", true);

                QuerySnapshot results = await query.GetSnapshotAsync();
                List<Passenger> passengers = new List<Passenger>();

                foreach (DocumentSnapshot doc in results.Documents)
                {
                    // Use ConvertTo<T>() if using [FirestoreProperty] attributes
                    Passenger passenger = doc.ConvertTo<Passenger>();
                    passengers.Add(passenger);
                }

                return passengers;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error getting passengers: {error.Message}");
                return new List<Passenger>();
            }
        }

        // ========== PASSWORD RESET METHODS (MODIFIED FOR HASHING) ==========

        // Reset driver password (MODIFIED FOR HASHING)
        public async Task<bool> ResetDriverPassword(string phone, string newPassword)
        {
            try
            {
                CollectionReference collection = database.Collection(COLLECTION_NAME);
                Query query = collection
                    .WhereEqualTo("role", "driver")
                    .WhereEqualTo("phone", phone);

                QuerySnapshot results = await query.GetSnapshotAsync();

                if (results.Documents.Count > 0)
                {
                    DocumentSnapshot doc = results.Documents[0];
                    string newHashedPassword = HashPassword(newPassword); // CRITICAL CHANGE: HASH the new password

                    await database.Collection(COLLECTION_NAME).Document(doc.Id).UpdateAsync("password", newHashedPassword);

                    Debug.WriteLine($"✓ Driver password reset");
                    return true;
                }

                return false;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error resetting password: {error.Message}");
                return false;
            }
        }

        // Reset passenger password (MODIFIED FOR HASHING)
        public async Task<bool> ResetPassengerPassword(string phone, string newPassword)
        {
            try
            {
                CollectionReference collection = database.Collection(COLLECTION_NAME);
                Query query = collection
                    .WhereEqualTo("role", "passenger")
                    .WhereEqualTo("phone", phone);

                QuerySnapshot results = await query.GetSnapshotAsync();

                if (results.Documents.Count > 0)
                {
                    DocumentSnapshot doc = results.Documents[0];
                    string newHashedPassword = HashPassword(newPassword); // CRITICAL CHANGE: HASH the new password

                    await database.Collection(COLLECTION_NAME).Document(doc.Id).UpdateAsync("password", newHashedPassword);

                    Debug.WriteLine($"✓ Passenger password reset");
                    return true;
                }

                return false;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error resetting password: {error.Message}");
                return false;
            }
        }

        // Verify account (UNCHANGED)
        public async Task<bool> VerifyAccount(string userId, string userRole)
        {
            // ... (Logic is correct, needs Debug reference change)
            try
            {
                DocumentReference doc = database.Collection(COLLECTION_NAME).Document(userId);
                await doc.UpdateAsync("isVerified", true);
                return true;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error verifying account: {error.Message}");
                return false;
            }
        }

        // ========== RIDE METHODS (COLLECTION NAMES MODIFIED) ==========

        // Create new ride request (COLLECTION NAME MODIFIED)
        public async Task<string?> CreateRideRequest(RideRequest ride)
        {
            try
            {
                CollectionReference collection = database.Collection(RIDES_COLLECTION);

                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "passengerId", ride.PassengerId },
                    { "passengerName", ride.PassengerName },
                    { "passengerPhone", ride.PassengerPhone },
                    { "driverId", ride.DriverId ?? "" },
                    { "driverName", ride.DriverName ?? "" },
                    { "pickupLatitude", ride.PickupLatitude },
                    { "pickupLongitude", ride.PickupLongitude },
                    { "destinationLatitude", ride.DestinationLatitude },
                    { "destinationLongitude", ride.DestinationLongitude },
                    { "destinationAddress", ride.DestinationAddress },
                    { "status", ride.Status },
                    { "fare", 0.0 },
                    { "requestTime", FieldValue.ServerTimestamp }, // Use server timestamp
                    { "acceptTime", null },
                    { "startTime", null },
                    { "completeTime", null }
                };

                DocumentReference docRef = await collection.AddAsync(data);
                Debug.WriteLine($"✓ Ride created: {docRef.Id}");
                return docRef.Id;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error creating ride: {error.Message}");
                return null;
            }
        }

        // Update ride status (COLLECTION NAME MODIFIED)
        public async Task<bool> UpdateRideStatus(string rideId, string status)
        {
            try
            {
                DocumentReference doc = database.Collection(RIDES_COLLECTION).Document(rideId);

                Dictionary<string, object> updates = new Dictionary<string, object>
                {
                    { "status", status }
                };

                if (status == "Accepted")
                    updates.Add("acceptTime", FieldValue.ServerTimestamp);
                else if (status == "InProgress")
                    updates.Add("startTime", FieldValue.ServerTimestamp);
                else if (status == "Completed")
                    updates.Add("completeTime", FieldValue.ServerTimestamp);

                await doc.UpdateAsync(updates);
                Debug.WriteLine($"✓ Ride status updated: {status}");
                return true;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error updating ride status: {error.Message}");
                return false;
            }
        }

        // Assign driver to ride (COLLECTION NAME MODIFIED)
        public async Task<bool> AssignDriverToRide(string rideId, string driverId, string driverName)
        {
            try
            {
                DocumentReference doc = database.Collection(RIDES_COLLECTION).Document(rideId);

                Dictionary<string, object> updates = new Dictionary<string, object>
                {
                    { "driverId", driverId },
                    { "driverName", driverName },
                    { "status", "Accepted" },
                    { "acceptTime", FieldValue.ServerTimestamp }
                };

                await doc.UpdateAsync(updates);
                Debug.WriteLine($"✓ Driver assigned to ride");
                return true;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error assigning driver: {error.Message}");
                return false;
            }
        }

        // Update ride fare (COLLECTION NAME MODIFIED)
        public async Task<bool> UpdateRideFare(string rideId, double fare)
        {
            try
            {
                DocumentReference doc = database.Collection(RIDES_COLLECTION).Document(rideId);
                await doc.UpdateAsync("fare", fare);
                Debug.WriteLine($"✓ Ride fare updated: ₱{fare:F2}");
                return true;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error updating fare: {error.Message}");
                return false;
            }
        }

        // Get active rides for passenger (COLLECTION NAME MODIFIED)
        public async Task<List<RideRequest>> GetPassengerActiveRides(string passengerId)
        {
            try
            {
                CollectionReference collection = database.Collection(RIDES_COLLECTION);
                Query query = collection
                    .WhereEqualTo("passengerId", passengerId)
                    .WhereIn("status", new[] { "Requested", "Accepted", "InProgress" });

                QuerySnapshot results = await query.GetSnapshotAsync();
                List<RideRequest> rides = new List<RideRequest>();

                foreach (DocumentSnapshot doc in results.Documents)
                {
                    RideRequest ride = doc.ConvertTo<RideRequest>(); // Use ConvertTo<T>()
                    rides.Add(ride);
                }

                return rides;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error getting rides: {error.Message}");
                return new List<RideRequest>();
            }
        }

        // Get active rides for driver (COLLECTION NAME MODIFIED)
        public async Task<List<RideRequest>> GetDriverActiveRides(string driverId)
        {
            try
            {
                CollectionReference collection = database.Collection(RIDES_COLLECTION);
                Query query = collection
                    .WhereEqualTo("driverId", driverId)
                    .WhereIn("status", new[] { "Accepted", "InProgress" });

                QuerySnapshot results = await query.GetSnapshotAsync();
                List<RideRequest> rides = new List<RideRequest>();

                foreach (DocumentSnapshot doc in results.Documents)
                {
                    RideRequest ride = doc.ConvertTo<RideRequest>(); // Use ConvertTo<T>()
                    rides.Add(ride);
                }

                return rides;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error getting rides: {error.Message}");
                return new List<RideRequest>();
            }
        }

        // ========== PAYMENT METHODS (COLLECTION NAMES MODIFIED) ==========

        // Save payment record (COLLECTION NAME MODIFIED)
        public async Task<string?> SavePayment(string rideId, string passengerId, string driverId, double amount, string method)
        {
            try
            {
                CollectionReference collection = database.Collection(PAYMENTS_COLLECTION);

                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "rideId", rideId },
                    { "passengerId", passengerId },
                    { "driverId", driverId },
                    { "amount", amount },
                    { "method", method },
                    { "status", "Completed" },
                    { "timestamp", FieldValue.ServerTimestamp }
                };

                DocumentReference docRef = await collection.AddAsync(data);
                Debug.WriteLine($"✓ Payment saved: {docRef.Id} - ₱{amount:F2} via {method}");
                return docRef.Id;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error saving payment: {error.Message}");
                return null;
            }
        }

        // Get payment by ride ID (COLLECTION NAME MODIFIED)
        public async Task<Dictionary<string, object>?> GetPaymentByRideId(string rideId)
        {
            try
            {
                CollectionReference collection = database.Collection(PAYMENTS_COLLECTION);
                Query query = collection.WhereEqualTo("rideId", rideId);

                QuerySnapshot results = await query.GetSnapshotAsync();

                if (results.Documents.Count > 0)
                {
                    DocumentSnapshot doc = results.Documents[0];
                    return new Dictionary<string, object>
                    {
                        { "id", doc.Id },
                        { "amount", doc.GetValue<double>("amount") },
                        { "method", doc.GetValue<string>("method") },
                        { "status", doc.GetValue<string>("status") }
                    };
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error getting payment: {error.Message}");
            }

            return null;
        }

        // ========== RATING METHODS (COLLECTION NAMES MODIFIED) ==========

        // Save driver rating (COLLECTION NAME MODIFIED)
        public async Task<string?> SaveRating(string rideId, string passengerId, string driverId, int stars, List<string> feedbackTags, string comment)
        {
            try
            {
                CollectionReference collection = database.Collection(RATINGS_COLLECTION);

                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "rideId", rideId },
                    { "passengerId", passengerId },
                    { "driverId", driverId },
                    { "stars", stars },
                    { "feedbackTags", feedbackTags ?? new List<string>() },
                    { "comment", comment ?? "" },
                    { "timestamp", FieldValue.ServerTimestamp }
                };

                DocumentReference docRef = await collection.AddAsync(data);

                await UpdateDriverAverageRating(driverId);

                Debug.WriteLine($"✓ Rating saved: {stars} stars");
                return docRef.Id;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error saving rating: {error.Message}");
                return null;
            }
        }

        // Update driver's average rating (COLLECTION NAME MODIFIED)
        private async Task UpdateDriverAverageRating(string driverId)
        {
            try
            {
                CollectionReference collection = database.Collection(RATINGS_COLLECTION);
                Query query = collection.WhereEqualTo("driverId", driverId);

                QuerySnapshot results = await query.GetSnapshotAsync();

                if (results.Documents.Count > 0)
                {
                    double totalStars = 0;
                    int count = 0;

                    foreach (DocumentSnapshot doc in results.Documents)
                    {
                        totalStars += doc.GetValue<int>("stars");
                        count++;
                    }

                    double averageRating = totalStars / count;

                    // Update driver document
                    DocumentReference driverDoc = database.Collection(COLLECTION_NAME).Document(driverId);
                    await driverDoc.UpdateAsync(new Dictionary<string, object>
                    {
                        { "averageRating", averageRating },
                        { "totalRatings", count }
                    });

                    Debug.WriteLine($"✓ Driver rating updated: {averageRating:F2} ({count} ratings)");
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error updating average rating: {error.Message}");
            }
        }

        // Get driver ratings (COLLECTION NAME MODIFIED)
        public async Task<List<Dictionary<string, object>>> GetDriverRatings(string driverId)
        {
            try
            {
                CollectionReference collection = database.Collection(RATINGS_COLLECTION);
                Query query = collection
                    .WhereEqualTo("driverId", driverId)
                    .OrderByDescending("timestamp")
                    .Limit(50);

                QuerySnapshot results = await query.GetSnapshotAsync();
                List<Dictionary<string, object>> ratings = new List<Dictionary<string, object>>();

                foreach (DocumentSnapshot doc in results.Documents)
                {
                    ratings.Add(new Dictionary<string, object>
                    {
                        { "id", doc.Id },
                        { "stars", doc.GetValue<int>("stars") },
                        { "comment", doc.GetValue<string>("comment") },
                        { "feedbackTags", doc.GetValue<List<string>>("feedbackTags") },
                        { "timestamp", doc.GetValue<DateTime>("timestamp") }
                    });
                }

                return ratings;
            }
            catch (Exception error)
            {
                Debug.WriteLine($"❌ Error getting ratings: {error.Message}");
                return new List<Dictionary<string, object>>();
            }
        }
    }
}