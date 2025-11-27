using RideMate.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace RideMate.Services
{
    /// <summary>
    /// RideService acts as a wrapper layer, converting business logic calls 
    /// into secure asynchronous calls to the CloudFirestoreService.
    /// It ensures all persistence and status updates go to Firebase.
    /// </summary>
    public class RideService
    {
        // Dependency on the core, secure data layer
        private readonly CloudFirestoreService _firestoreService;

        public RideService()
        {
            _firestoreService = new CloudFirestoreService();
        }

        // =======================================================
        // DRIVER AVAILABILITY AND STATUS (Resolves UpdateDriverStatus errors)
        // =======================================================

        /// <summary>
        /// REQUIRED FIX: Updates the driver's online status in the Firestore 'users' collection.
        /// </summary>
        public async Task<bool> UpdateDriverStatus(string driverId, bool isOnline)
        {
            // Delegates the call to the secure CloudFirestoreService
            return await _firestoreService.UpdateDriverStatus(driverId, isOnline);
        }

        /// <summary>
        /// Updates driver's location in the Firestore 'users' collection.
        /// </summary>
        public async Task<bool> UpdateDriverLocation(string driverId, double latitude, double longitude)
        {
            return await _firestoreService.UpdateDriverLocation(driverId, latitude, longitude);
        }

        // =======================================================
        // RIDE OFFERING (Creation and Retrieval)
        // =======================================================

        /// <summary>
        /// Driver creates a ride offer (saves a document to the 'rides' collection).
        /// </summary>
        public async Task<string?> CreateDriverRideOffer(RideRequest rideOffer)
        {
            // NOTE: The CreateRideRequest method handles the actual creation in Firestore.
            // Assuming the passed-in rideOffer has all the necessary coordinates and status.
            return await _firestoreService.CreateRideRequest(rideOffer);
        }

        /// <summary>
        /// Get available rides for passengers (must search Firestore).
        /// NOTE: This replaces the old local list filtering.
        /// </summary>
        /// <param name="destination">The destination keyword to search for.</param>
        public async Task<List<RideRequest>> GetRidesByDestinationAsync(string destination)
        {
            // This is complex filtering that should ideally be done on the server (Cloud Function).
            // Since we are limited to the client, we fetch all active rides and filter locally.

            // NOTE: This is inefficient but necessary if Firebase Query limitations persist.
            var activeRides = await _firestoreService.GetDriverActiveRides(null!); // Fetching all active rides

            // Filter the results in C# memory
            return activeRides
                .Where(r => r.Status == "Available" &&
                           r.DestinationAddress.Contains(destination, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // --- Other original RideService methods would be converted similarly ---

        // Get available drivers near destination (using Firestore query)
        public async Task<List<Driver>> GetAvailableDrivers(double destLat, double destLon, double radiusKm = 10)
        {
            // Now calls the secure method
            return await _firestoreService.GetOnlineDrivers();
        }

        // Get active ride for passenger
        public async Task<RideRequest?> GetActiveRideForPassenger(string passengerId)
        {
            var rides = await _firestoreService.GetPassengerActiveRides(passengerId);
            // Return the most recent or active ride
            return rides.FirstOrDefault();
        }

        // Get active ride for driver
        public async Task<RideRequest?> GetActiveRideForDriver(string driverId)
        {
            var rides = await _firestoreService.GetDriverActiveRides(driverId);
            return rides.FirstOrDefault();
        }

        // --- Insecure local list methods removed: CreateRideRequest (Passenger), AcceptRideRequest, etc. ---
        // These methods rely on direct Firestore calls via CloudFirestoreService now.

        // Final version of UpdateDriverAvailability (now just calls the status method):
        public async Task UpdateDriverAvailability(Driver driver, bool isOnline)
        {
            await UpdateDriverStatus(driver.Id, isOnline);
            if (isOnline)
            {
                await UpdateDriverLocation(driver.Id, driver.Latitude, driver.Longitude);
            }
        }
    }
}