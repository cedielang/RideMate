using Google.Cloud.Firestore;
using RideMate.Models;
using System.Reflection;

namespace RideMate.Services
{
    // Simple Firestore Service - connects to your Firebase database
    public class CloudFirestoreService
    {
        // Connection to Firebase
        FirestoreDb database;

        // Your collection name
        const string COLLECTION_NAME = "database_information";

        public CloudFirestoreService(string projectId)
        {
            try
            {
                // Load credentials from embedded JSON file
                Assembly assembly = Assembly.GetExecutingAssembly();
                string resourceName = "RideMate.Services.ridemate-7c600-firebase-adminsdk-fbsvc-6384e1273d.json";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        string tempPath = Path.Combine(FileSystem.CacheDirectory, "firebase-credentials.json");
                        
                        using (FileStream fileStream = File.Create(tempPath))
                        {
                            stream.CopyTo(fileStream);
                        }

                        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", tempPath);
                        System.Diagnostics.Debug.WriteLine("✓ Firebase credentials loaded!");
                    }
                }

                // Connect to Firebase
                database = FirestoreDb.Create(projectId);
                System.Diagnostics.Debug.WriteLine($"✓ Connected to Firestore: {projectId}");
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine("❌ Error: " + error.Message);
                throw new Exception("Failed to initialize Firestore", error);
            }
        }

        // ========== DRIVER METHODS ==========

        // Add new driver
        public async Task<bool> AddDriver(Driver driver)
        {
            try
            {
                // Create unique ID
                driver.Id = "uid" + Guid.NewGuid().ToString().Substring(0, 6);

                // Get collection
                CollectionReference collection = database.Collection(COLLECTION_NAME);

                // Create driver data matching your structure
                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "role", "driver" },
                    { "name", driver.Name },
                    { "email", driver.Email },
                    { "phone", driver.Phone },
                    { "password", driver.Password },
                    { "licenseNumber", driver.LicenseNumber },
                    { "vehicleType", driver.VehicleType },
                    { "vehicleModel", driver.VehicleModel },
                    { "plateNumber", driver.PlateNumber },
                    { "createdAt", DateTime.UtcNow }
                };

                // Save to Firebase
                await collection.Document(driver.Id).SetAsync(data);

                System.Diagnostics.Debug.WriteLine($"✓ Driver saved: {driver.Id}");
                return true;
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error adding driver: {error.Message}");
                return false;
            }
        }

        // Find driver by phone and password
        public async Task<Driver> FindDriver(string phone, string password)
        {
            try
            {
                CollectionReference collection = database.Collection(COLLECTION_NAME);

                // Search for driver
                Query query = collection
                    .WhereEqualTo("role", "driver")
                    .WhereEqualTo("phone", phone)
                    .WhereEqualTo("password", password);

                QuerySnapshot results = await query.GetSnapshotAsync();

                if (results.Documents.Count > 0)
                {
                    DocumentSnapshot doc = results.Documents[0];

                    // Create Driver object from Firestore data
                    Driver driver = new Driver
                    {
                        Id = doc.Id,
                        Name = doc.ContainsField("name") ? doc.GetValue<string>("name") : "",
                        Email = doc.ContainsField("email") ? doc.GetValue<string>("email") : "",
                        Phone = doc.ContainsField("phone") ? doc.GetValue<string>("phone") : "",
                        Password = doc.ContainsField("password") ? doc.GetValue<string>("password") : "",
                        LicenseNumber = doc.ContainsField("licenseNumber") ? doc.GetValue<string>("licenseNumber") : "",
                        VehicleType = doc.ContainsField("vehicleType") ? doc.GetValue<string>("vehicleType") : "",
                        VehicleModel = doc.ContainsField("vehicleModel") ? doc.GetValue<string>("vehicleModel") : "",
                        PlateNumber = doc.ContainsField("plateNumber") ? doc.GetValue<string>("plateNumber") : ""
                    };

                    System.Diagnostics.Debug.WriteLine($"✓ Driver found: {driver.Name}");
                    return driver;
                }
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error finding driver: {error.Message}");
            }

            return null;
        }

        // Check if driver phone exists
        public async Task<bool> DriverPhoneExists(string phone)
        {
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

        // Update driver status
        public async Task<bool> UpdateDriverStatus(string driverId, bool isOnline)
        {
            try
            {
                DocumentReference doc = database.Collection(COLLECTION_NAME).Document(driverId);
                await doc.UpdateAsync("isOnline", isOnline);
                return true;
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error updating status: {error.Message}");
                return false;
            }
        }

        // Update driver location
        public async Task<bool> UpdateDriverLocation(string driverId, double latitude, double longitude)
        {
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
                System.Diagnostics.Debug.WriteLine($"❌ Error updating location: {error.Message}");
                return false;
            }
        }

        // Get all online drivers
        public async Task<List<Driver>> GetOnlineDrivers()
        {
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
                    Driver driver = new Driver
                    {
                        Id = doc.Id,
                        Name = doc.GetValue<string>("name"),
                        Phone = doc.GetValue<string>("phone"),
                        VehicleType = doc.ContainsField("vehicleType") ? doc.GetValue<string>("vehicleType") : "",
                        Latitude = doc.ContainsField("latitude") ? doc.GetValue<double>("latitude") : 0.0,
                        Longitude = doc.ContainsField("longitude") ? doc.GetValue<double>("longitude") : 0.0
                    };

                    drivers.Add(driver);
                }

                return drivers;
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error getting drivers: {error.Message}");
                return new List<Driver>();
            }
        }

        // ========== PASSENGER METHODS ==========

        // Add new passenger
        public async Task<bool> AddPassenger(Passenger passenger)
        {
            try
            {
                // Create unique ID
                passenger.Id = "uid" + Guid.NewGuid().ToString().Substring(0, 6);

                // Get collection
                CollectionReference collection = database.Collection(COLLECTION_NAME);

                // Create passenger data matching your structure
                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "role", "passenger" },
                    { "name", passenger.Name },
                    { "email", passenger.Email },
                    { "phone", passenger.Phone },
                    { "password", passenger.Password },
                    { "createdAt", DateTime.UtcNow }
                };

                // Save to Firebase
                await collection.Document(passenger.Id).SetAsync(data);

                System.Diagnostics.Debug.WriteLine($"✓ Passenger saved: {passenger.Id}");
                return true;
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error adding passenger: {error.Message}");
                return false;
            }
        }

        // Find passenger by phone and password
        public async Task<Passenger> FindPassenger(string phone, string password)
        {
            try
            {
                CollectionReference collection = database.Collection(COLLECTION_NAME);

                // Search for passenger
                Query query = collection
                    .WhereEqualTo("role", "passenger")
                    .WhereEqualTo("phone", phone)
                    .WhereEqualTo("password", password);

                QuerySnapshot results = await query.GetSnapshotAsync();

                if (results.Documents.Count > 0)
                {
                    DocumentSnapshot doc = results.Documents[0];

                    // Create Passenger object
                    Passenger passenger = new Passenger
                    {
                        Id = doc.Id,
                        Name = doc.ContainsField("name") ? doc.GetValue<string>("name") : "",
                        Email = doc.ContainsField("email") ? doc.GetValue<string>("email") : "",
                        Phone = doc.ContainsField("phone") ? doc.GetValue<string>("phone") : "",
                        Password = doc.ContainsField("password") ? doc.GetValue<string>("password") : ""
                    };

                    System.Diagnostics.Debug.WriteLine($"✓ Passenger found: {passenger.Name}");
                    return passenger;
                }
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error finding passenger: {error.Message}");
            }

            return null;
        }

        // Check if passenger phone exists
        public async Task<bool> PassengerPhoneExists(string phone)
        {
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

        // Update passenger location
        public async Task<bool> UpdatePassengerLocation(string passengerId, double latitude, double longitude)
        {
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
                System.Diagnostics.Debug.WriteLine($"❌ Error updating location: {error.Message}");
                return false;
            }
        }

        // Get waiting passengers
        public async Task<List<Passenger>> GetWaitingPassengers()
        {
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
                    Passenger passenger = new Passenger
                    {
                        Id = doc.Id,
                        Name = doc.GetValue<string>("name"),
                        Phone = doc.GetValue<string>("phone"),
                        Latitude = doc.ContainsField("latitude") ? doc.GetValue<double>("latitude") : 0.0,
                        Longitude = doc.ContainsField("longitude") ? doc.GetValue<double>("longitude") : 0.0
                    };

                    passengers.Add(passenger);
                }

                return passengers;
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error getting passengers: {error.Message}");
                return new List<Passenger>();
            }
        }

        // ========== PASSWORD RESET METHODS ==========

        // Reset driver password
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
                    await database.Collection(COLLECTION_NAME).Document(doc.Id).UpdateAsync("password", newPassword);
                    
                    System.Diagnostics.Debug.WriteLine($"✓ Driver password reset");
                    return true;
                }

                return false;
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error resetting password: {error.Message}");
                return false;
            }
        }

        // Reset passenger password
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
                    await database.Collection(COLLECTION_NAME).Document(doc.Id).UpdateAsync("password", newPassword);
                    
                    System.Diagnostics.Debug.WriteLine($"✓ Passenger password reset");
                    return true;
                }

                return false;
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error resetting password: {error.Message}");
                return false;
            }
        }

        // Verify account
        public async Task<bool> VerifyAccount(string userId, string userRole)
        {
            try
            {
                DocumentReference doc = database.Collection(COLLECTION_NAME).Document(userId);
                await doc.UpdateAsync("isVerified", true);
                return true;
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error verifying account: {error.Message}");
                return false;
            }
        }
    }
}
