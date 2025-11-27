using System;
using System.Collections.Generic;
using System.Linq; // Essential for Select, Sum, First, ToList
using System.Threading.Tasks;
using Plugin.CloudFirestore; // Contains IFirestore, IQuery, ICollectionReference, etc.
using RideMate.Models;
using System.Diagnostics;
using Math = System.Math;

namespace RideMate.Services
{
    public class CloudFirestoreService
    {
        // Connection instance (IFirestore is the correct type)
        private readonly IFirestore _db;

        // Collection constants
        private const string USERS_COLLECTION = "database_information"; // Using user's defined name
        private const string RIDES_COLLECTION = "rides";
        private const string PAYMENTS_COLLECTION = "payments";
        private const string RATINGS_COLLECTION = "ratings";

        public CloudFirestoreService()
        {
            try
            {
                _db = CrossCloudFirestore.Current.Instance;
                Debug.WriteLine("✓ Firestore Client initialized");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error initializing Firestore Client: {ex.Message}");
                throw new InvalidOperationException("Failed to initialize Firestore client. Check native Firebase setup and installed SDKs.", ex);
            }
        }

        #region ---------- Generic Helpers & Converters ----------

        private ICollectionReference Collection(string name) => _db.Collection(name);
        private IDocumentReference Document(string collectionName, string id) => _db.Collection(collectionName).Document(id);

        /// <summary>
        /// Retrieves and maps a single document snapshot to a C# object.
        /// </summary>
        private T? GetDocObject<T>(IDocumentSnapshot doc) where T : class, new()
        {
            if (doc == null || !doc.Exists) return null;
            try
            {
                // Uses the reliable ToObject<T>() method provided by the plugin
                var result = doc.ToObject<T>();
                if (result != null) (result as dynamic).Id = doc.Id;
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Conversion failed for doc {doc.Id}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Retrieves and maps all documents from a query snapshot to a List of C# objects.
        /// </summary>
        private List<T> GetQueryObjects<T>(IQuerySnapshot snapshot) where T : class, new()
        {
            if (snapshot == null) return new List<T>();

            // Accessing .Documents first to enable standard Linq extension methods
            return snapshot.Documents
                .Where(d => d.Exists)
                // Using .ToObject<T>() for correct conversion
                .Select(d =>
                {
                    var obj = d.ToObject<T>();
                    if (obj != null) (obj as dynamic).Id = d.Id;
                    return obj;
                })
                .Where(x => x != null)
                .ToList()!;
        }

        /// <summary>
        /// Helper for field lookups that gracefully handles potential conversion errors.
        /// </summary>
        private static T TryGet<T>(IDocumentSnapshot doc, string field, T defaultValue)
        {
            try { return doc.Get<T>(field); }
            catch { return defaultValue; }
        }

        #endregion

        #region ---------- User Profiles & Authentication Wrappers ----------

        public async Task<bool> AddDriverProfile(Driver driver, string firebaseUid)
        {
            try
            {
                var docRef = Document(USERS_COLLECTION, firebaseUid);
                var data = new Dictionary<string, object>
                {
                    { "Role", "driver" }, { "Name", driver.Name ?? string.Empty },
                    { "Email", driver.Email ?? string.Empty }, { "Phone", driver.Phone ?? string.Empty },
                    { "LicenseNumber", driver.LicenseNumber ?? string.Empty },
                    { "VehicleType", driver.VehicleType ?? string.Empty },
                    { "VehicleModel", driver.VehicleModel ?? string.Empty },
                    { "PlateNumber", driver.PlateNumber ?? string.Empty },
                    { "IsBlocked", false }, { "IsOnline", false },
                    { "CreatedAt", FieldValue.ServerTimestamp }
                };

                await docRef.SetAsync(data);
                Debug.WriteLine($"✓ Driver profile saved: {firebaseUid}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error adding driver profile: {ex.Message}");
                return false;
            }
        }

        public async Task<Driver?> FindDriverProfileByUid(string firebaseUid)
        {
            try
            {
                var snap = await Document(USERS_COLLECTION, firebaseUid).GetAsync();
                return GetDocObject<Driver>(snap);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error finding driver profile: {ex.Message}");
                return null;
            }
        }

        // DEPRECATED Wrapper methods for older code (FindDriver/AddDriver)
        public Task<Driver?> FindDriver(string phone, string password) => QuerySingleUserByPhoneAsync<Driver>("driver", phone);
        public Task<bool> AddDriver(Driver driver) => AddDriverProfile(driver, Guid.NewGuid().ToString());

        public async Task<bool> AddPassengerProfile(Passenger passenger, string firebaseUid)
        {
            try
            {
                var docRef = Document(USERS_COLLECTION, firebaseUid);
                var data = new Dictionary<string, object>
                {
                    { "Role", "passenger" }, { "Name", passenger.Name ?? string.Empty },
                    { "Email", passenger.Email ?? string.Empty }, { "Phone", passenger.Phone ?? string.Empty },
                    { "IsBlocked", false },
                    { "CreatedAt", FieldValue.ServerTimestamp }
                };
                await docRef.SetAsync(data);
                Debug.WriteLine($"✓ Passenger profile saved: {firebaseUid}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error adding passenger profile: {ex.Message}");
                return false;
            }
        }

        public async Task<Passenger?> FindPassengerProfileByUid(string firebaseUid)
        {
            try
            {
                var snap = await Document(USERS_COLLECTION, firebaseUid).GetAsync();
                return GetDocObject<Passenger>(snap);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error finding passenger profile: {ex.Message}");
                return null;
            }
        }

        // DEPRECATED Wrapper methods for older code (FindPassenger/AddPassenger)
        public Task<Passenger?> FindPassenger(string phone, string password) => QuerySingleUserByPhoneAsync<Passenger>("passenger", phone);
        public Task<bool> AddPassenger(Passenger passenger) => AddPassengerProfile(passenger, Guid.NewGuid().ToString());


        /// <summary>
        /// Retrieves ALL user documents and filters using C# LINQ (Workaround for query method failure).
        /// WARNING: Inefficient on large datasets.
        /// </summary>
        private async Task<T?> QuerySingleUserByPhoneAsync<T>(string role, string phone) where T : class, new()
        {
            try
            {
                // STEP 1: Get ALL documents in the USERS collection (no server-side filter)
                var snap = await Collection(USERS_COLLECTION).GetAsync();

                // STEP 2: Convert to objects and filter using C# LINQ
                var allUsers = GetQueryObjects<T>(snap);

                return allUsers.Where(u =>
                {
                    // This is the core filtering logic done in C# memory
                    if (u is Driver driver)
                        return driver.Role == role && driver.Phone == phone;
                    if (u is Passenger passenger)
                        return passenger.Role == role && passenger.Phone == phone;
                    return false;
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error querying user by phone (LINQ Fallback): {ex.Message}");
                return null;
            }
        }

        public async Task<bool> PhoneExists(string phone)
        {
            try
            {
                // STEP 1: Get ALL documents (inefficient, but compiles)
                var snap = await Collection(USERS_COLLECTION).GetAsync();

                // STEP 2: Filter in C# memory
                var allUsers = GetQueryObjects<Driver>(snap); // Use driver as a base type since it has phone

                return allUsers.Any(u => u.Phone == phone);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error checking phone existence: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> PassengerPhoneExists(string phone) => await PhoneExists(phone);

        // Placeholder for password reset methods (should be replaced by Firebase Auth API calls)
        public Task<bool> ResetDriverPassword(string phone, string newPassword) => Task.FromResult(false);
        public Task<bool> ResetPassengerPassword(string phone, string newPassword) => Task.FromResult(false);

        #endregion

        #region ---------- Location & Status ----------

        public async Task<bool> UpdateDriverStatus(string driverId, bool isOnline)
        {
            try
            {
                await Document(USERS_COLLECTION, driverId).UpdateAsync("IsOnline", isOnline);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error updating status: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateDriverLocation(string driverId, double latitude, double longitude)
        {
            try
            {
                var updates = new Dictionary<string, object>
                {
                    { "Latitude", latitude },
                    { "Longitude", longitude }
                };
                await Document(USERS_COLLECTION, driverId).UpdateAsync(updates);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error updating location: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Driver>> GetOnlineDrivers()
        {
            try
            {
                // Fetch all and filter in memory (Linq Fallback)
                var snap = await Collection(USERS_COLLECTION).GetAsync();
                var allDrivers = GetQueryObjects<Driver>(snap);

                return allDrivers.Where(d => d.Role == "driver" && d.IsOnline).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error getting drivers: {ex.Message}");
                return new List<Driver>();
            }
        }

        public async Task<List<Passenger>> GetWaitingPassengers()
        {
            try
            {
                // Fetch all and filter in memory (Linq Fallback)
                var snap = await Collection(USERS_COLLECTION).GetAsync();
                var allPassengers = GetQueryObjects<Passenger>(snap);

                return allPassengers.Where(p => p.Role == "passenger" && p.IsWaitingForRide).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error getting passengers: {ex.Message}");
                return new List<Passenger>();
            }
        }

        public async Task<bool> UpdatePassengerLocation(string passengerId, double latitude, double longitude)
        {
            try
            {
                var updates = new Dictionary<string, object>
                {
                    { "Latitude", latitude },
                    { "Longitude", longitude }
                };
                await Document(USERS_COLLECTION, passengerId).UpdateAsync(updates);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error updating passenger location: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region ---------- Account Actions (Verification) ----------

        public async Task<bool> VerifyAccount(string userId, string userRole = null)
        {
            try
            {
                await Document(USERS_COLLECTION, userId).UpdateAsync("IsVerified", true);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error verifying account: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateUserProfile(string userId, Dictionary<string, object> updates)
        {
            try
            {
                
                // The document path is: USERS_COLLECTION/{userId}
                IDocumentReference doc = _db.Collection(USERS_COLLECTION).Document(userId);

                // UpdateAsync sends the dictionary of changes to Firebase
                await doc.UpdateAsync(updates);

                System.Diagnostics.Debug.WriteLine($"✓ Profile updated for user: {userId}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error updating profile: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region ---------- Rides ----------

        public async Task<string?> CreateRideRequest(RideRequest ride)
        {
            try
            {
                var collection = Collection(RIDES_COLLECTION);
                var data = new Dictionary<string, object>
                {
                    { "PassengerId", ride.PassengerId }, { "PassengerName", ride.PassengerName ?? string.Empty },
                    { "PassengerPhone", ride.PassengerPhone ?? string.Empty },
                    { "DriverId", ride.DriverId ?? string.Empty }, { "DriverName", ride.DriverName ?? string.Empty },
                    { "PickupLatitude", ride.PickupLatitude }, { "PickupLongitude", ride.PickupLongitude },
                    { "DestinationLatitude", ride.DestinationLatitude }, { "DestinationLongitude", ride.DestinationLongitude },
                    { "DestinationAddress", ride.DestinationAddress ?? string.Empty },
                    { "Status", ride.Status ?? "Requested" }, { "Fare", 0.0 },
                    { "RequestTime", FieldValue.ServerTimestamp },
                    { "AcceptTime", null }, { "StartTime", null }, { "CompleteTime", null }
                };

                var docRef = await collection.AddAsync(data);
                Debug.WriteLine($"✓ Ride created: {docRef.Id}");
                return docRef.Id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error creating ride: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateRideStatus(string rideId, string status)
        {
            try
            {
                var doc = Document(RIDES_COLLECTION, rideId);
                var updates = new Dictionary<string, object> { { "Status", status } };

                if (status == "Accepted") updates.Add("AcceptTime", FieldValue.ServerTimestamp);
                else if (status == "InProgress") updates.Add("StartTime", FieldValue.ServerTimestamp);
                else if (status == "Completed") updates.Add("CompleteTime", FieldValue.ServerTimestamp);

                await doc.UpdateAsync(updates);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error updating ride status: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AssignDriverToRide(string rideId, string driverId, string driverName)
        {
            try
            {
                var doc = Document(RIDES_COLLECTION, rideId);
                var updates = new Dictionary<string, object>
                {
                    { "DriverId", driverId },
                    { "DriverName", driverName },
                    { "Status", "Accepted" },
                    { "AcceptTime", FieldValue.ServerTimestamp }
                };
                await doc.UpdateAsync(updates);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error assigning driver: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateRideFare(string rideId, double fare)
        {
            try
            {
                await Document(RIDES_COLLECTION, rideId).UpdateAsync("Fare", fare);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error updating fare: {ex.Message}");
                return false;
            }
        }

        public async Task<List<RideRequest>> GetPassengerActiveRides(string passengerId)
        {
            try
            {
                // Fetch all and filter in memory (Linq Fallback)
                var snap = await Collection(RIDES_COLLECTION).GetAsync();
                var allRides = GetQueryObjects<RideRequest>(snap);

                return allRides.Where(r =>
                    r.PassengerId == passengerId &&
                    (r.Status == "Requested" || r.Status == "Accepted" || r.Status == "InProgress")
                ).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error getting passenger rides: {ex.Message}");
                return new List<RideRequest>();
            }
        }

        public async Task<List<RideRequest>> GetDriverActiveRides(string driverId)
        {
            try
            {
                // Fetch all and filter in memory (Linq Fallback)
                var snap = await Collection(RIDES_COLLECTION).GetAsync();
                var allRides = GetQueryObjects<RideRequest>(snap);

                return allRides.Where(r =>
                    r.DriverId == driverId &&
                    (r.Status == "Accepted" || r.Status == "InProgress")
                ).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error getting driver rides: {ex.Message}");
                return new List<RideRequest>();
            }
        }

        #endregion

        #region ---------- Payments ----------

        public async Task<string?> SavePayment(string rideId, string passengerId, string driverId, double amount, string method)
        {
            try
            {
                var collection = Collection(PAYMENTS_COLLECTION);
                var data = new Dictionary<string, object>
                {
                    { "RideId", rideId }, { "PassengerId", passengerId }, { "DriverId", driverId },
                    { "Amount", amount }, { "Method", method ?? string.Empty },
                    { "Status", "Completed" }, { "Timestamp", FieldValue.ServerTimestamp }
                };

                var docRef = await collection.AddAsync(data);
                Debug.WriteLine($"✓ Payment saved: {docRef.Id} - ₱{amount:F2} via {method}");
                return docRef.Id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error saving payment: {ex.Message}");
                return null;
            }
        }

        public async Task<Dictionary<string, object>?> GetPaymentByRideId(string rideId)
        {
            try
            {
                // Fetch all and filter in memory (Linq Fallback)
                var snap = await Collection(PAYMENTS_COLLECTION).GetAsync();
                var allPayments = snap.Documents.Where(d => d.Exists).ToList();

                var doc = allPayments.FirstOrDefault(d => TryGet(d, "RideId", string.Empty) == rideId);

                if (doc == null) return null;

                var result = new Dictionary<string, object> { { "Id", doc.Id } };

                // Safer field retrieval using TryGet helper
                result.Add("Amount", TryGet(doc, "Amount", 0.0));
                result.Add("Method", TryGet(doc, "Method", string.Empty));
                result.Add("Status", TryGet(doc, "Status", string.Empty));

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error getting payment: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region ---------- Ratings ----------

        public async Task<string?> SaveRating(string rideId, string passengerId, string driverId, int stars, List<string> feedbackTags, string comment)
        {
            try
            {
                var collection = Collection(RATINGS_COLLECTION);
                var data = new Dictionary<string, object>
                {
                    { "RideId", rideId }, { "PassengerId", passengerId }, { "DriverId", driverId },
                    { "Stars", stars }, { "FeedbackTags", feedbackTags ?? new List<string>() },
                    { "Comment", comment ?? string.Empty }, { "Timestamp", FieldValue.ServerTimestamp }
                };

                var docRef = await collection.AddAsync(data);
                await UpdateDriverAverageRating(driverId);
                Debug.WriteLine($"✓ Rating saved: {stars} stars");
                return docRef.Id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error saving rating: {ex.Message}");
                return null;
            }
        }

        private async Task UpdateDriverAverageRating(string driverId)
        {
            try
            {
                // Fetch all and filter in memory (Linq Fallback)
                var snap = await Collection(RATINGS_COLLECTION).GetAsync();
                var allRatings = snap.Documents.Where(d => TryGet(d, "DriverId", string.Empty) == driverId).ToList();

                if (allRatings.Count == 0) return;

                // Safely calculate star sum using TryGet
                var starSum = allRatings
                    .Select(d => TryGet(d, "Stars", 0))
                    .Sum();

                var count = allRatings.Count;
                var average = (double)starSum / Math.Max(1, count);

                await Document(USERS_COLLECTION, driverId).UpdateAsync(new Dictionary<string, object>
                {
                    { "AverageRating", average },
                    { "TotalRatings", count }
                });
                Debug.WriteLine($"✓ Driver rating updated: {average:F2} ({count} ratings)");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error updating average rating: {ex.Message}");
            }
        }

        public async Task<List<Dictionary<string, object>>> GetDriverRatings(string driverId)
        {
            try
            {
                // Fetch all and filter in memory (Linq Fallback)
                var snap = await Collection(RATINGS_COLLECTION).GetAsync();
                var allRatings = snap.Documents
                    .Where(d => TryGet(d, "DriverId", string.Empty) == driverId)
                    .ToList();

                // Sort by Timestamp (in C#)
                var sortedRatings = allRatings
                    .OrderByDescending(d => TryGet(d, "Timestamp", DateTime.MinValue))
                    .Take(50);

                return sortedRatings
                    .Select(d => new Dictionary<string, object>
                    {
                        { "Id", d.Id },
                        { "Stars", TryGet(d, "Stars", 0) },
                        { "Comment", TryGet(d, "Comment", string.Empty) },
                        { "FeedbackTags", TryGet(d, "FeedbackTags", new List<string>()) },
                        { "Timestamp", TryGet(d, "Timestamp", DateTime.MinValue) }
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error getting ratings: {ex.Message}");
                return new List<Dictionary<string, object>>();
            }
        }

        #endregion
    }
}