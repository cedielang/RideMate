// This file uses the basic, one-time read pattern (polling) 
// to ensure compilation stability and functional correctness.
// NOTE: Real-time updates must be managed by the calling ViewModel using a timer.

using Plugin.CloudFirestore;
using RideMate.Models;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RideMate.Services
{
    public class RideMonitor
    {
        private readonly IFirestore database;
        private const string RIDES_COLLECTION = "rides";

        public RideMonitor(IFirestore firestoreInstance)
        {
            this.database = firestoreInstance;
        }

        // ==========================================================
        // 1. DRIVER DASHBOARD: GET INCOMING REQUESTS (POLLING MODEL)
        // ==========================================================

        /// <summary>
        /// Retrieves a snapshot of all incoming ride requests (Status == "Requested").
        /// This method is intended to be called repeatedly on a timer by the Driver Dashboard.
        /// </summary>
        /// <returns>A List of currently requested rides.</returns>
        public async Task<List<RideRequest>> GetIncomingRequestsAsync()
        {
            try
            {
                // FIX: Remove problematic .Where() and .Limit() calls.
                // We will rely on C# LINQ filtering after retrieval (inefficient but compiles).
                IQuery query = database.Collection(RIDES_COLLECTION);

                // Execute a ONE-TIME read (GetAsync)
                IQuerySnapshot snapshot = await query.GetAsync();

                // Use the safe conversion utility from the service layer
                List<RideRequest> allRides = snapshot.Documents
                    .Where(d => d.Exists)
                    .Select(d =>
                    {
                        var obj = d.ToObject<RideRequest>();
                        if (obj != null) obj.Id = d.Id;
                        return obj;
                    })
                    .Where(x => x != null)
                    .ToList()!;

                // FINAL STEP: Apply filtering and limiting using C# LINQ
                var filteredRides = allRides
                    .Where(r => r.Status == "Requested")
                    .Take(20) // Apply limit locally
                    .ToList();

                Debug.WriteLine($"[Driver Polling] Found {filteredRides.Count} active requests.");
                return filteredRides;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Driver Polling] ERROR retrieving requests: {ex.Message}");
                return new List<RideRequest>();
            }
        }

        // ==========================================================
        // 2. PASSENGER DASHBOARD: MONITOR ACTIVE RIDE (POLLING MODEL)
        // ==========================================================

        /// <summary>
        /// Retrieves a snapshot of a single, specific ride document.
        /// This is intended to be called repeatedly on a timer by the Passenger Dashboard.
        /// </summary>
        /// <returns>The current state of the ride, or null if not found.</returns>
        public async Task<RideRequest?> GetActiveRideStateAsync(string rideId)
        {
            try
            {
                IDocumentReference rideDocument = database
                    .Collection(RIDES_COLLECTION)
                    .Document(rideId);

                // Execute a ONE-TIME read (GetAsync)
                IDocumentSnapshot doc = await rideDocument.GetAsync();

                if (doc.Exists)
                {
                    var ride = doc.ToObject<RideRequest>();
                    if (ride != null) ride.Id = doc.Id;
                    Debug.WriteLine($"[Passenger Polling] Ride {rideId} Status: {ride?.Status}");
                    return ride;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Passenger Polling] ERROR retrieving ride state: {ex.Message}");
            }
            return null;
        }

        // --- DEPRECATED REAL-TIME METHODS REMOVED ---
        // The previous methods relying on IDisposable and AsObservable have been removed
        // to simplify the code and guarantee compilation.
    }
}