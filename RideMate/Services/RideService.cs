using RideMate.Models;

namespace RideMate.Services
{
    public class RideService
    {
        private static List<RideRequest> _rideRequests = new List<RideRequest>();
        private static List<Driver> _availableDrivers = new List<Driver>();
        private static List<RideRequest> _driverRideOffers = new List<RideRequest>(); // Drivers offering rides

        // Driver creates a ride offer (Point A to Point B)
        public void CreateDriverRideOffer(RideRequest rideOffer)
        {
            _driverRideOffers.Add(rideOffer);
        }

        // Get rides by destination (for passengers to find)
        public List<RideRequest> GetRidesByDestination(string destination)
        {
            return _driverRideOffers
                .Where(r => r.Status == "Available" && 
                           r.DestinationAddress.Contains(destination, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Passenger creates a ride request
        public RideRequest CreateRideRequest(Passenger passenger, double pickupLat, double pickupLon, double destLat, double destLon, string destAddress)
        {
            var request = new RideRequest
            {
                Id = Guid.NewGuid().ToString(),
                PassengerId = passenger.Phone,
                PassengerName = passenger.Name,
                PassengerPhone = passenger.Phone,
                PickupLatitude = pickupLat,
                PickupLongitude = pickupLon,
                DestinationLatitude = destLat,
                DestinationLongitude = destLon,
                DestinationAddress = destAddress,
                Status = "Pending",
                RequestTime = DateTime.Now
            };

            _rideRequests.Add(request);
            return request;
        }

        // Get available drivers near destination
        public List<Driver> GetAvailableDrivers(double destLat, double destLon, double radiusKm = 10)
        {
            return _availableDrivers.Where(d => d.IsOnline).ToList();
        }

        // Driver accepts ride request
        public bool AcceptRideRequest(string requestId, Driver driver)
        {
            var request = _rideRequests.FirstOrDefault(r => r.Id == requestId);
            if (request != null && request.Status == "Pending")
            {
                request.DriverId = driver.Phone;
                request.DriverName = driver.Name;
                request.Status = "Accepted";
                request.AcceptedTime = DateTime.Now;
                return true;
            }
            return false;
        }

        // Driver declines ride request
        public bool DeclineRideRequest(string requestId)
        {
            var request = _rideRequests.FirstOrDefault(r => r.Id == requestId);
            if (request != null && request.Status == "Pending")
            {
                request.Status = "Cancelled";
                return true;
            }
            return false;
        }

        // Complete ride
        public bool CompleteRide(string requestId)
        {
            var request = _rideRequests.FirstOrDefault(r => r.Id == requestId);
            if (request != null && request.Status == "Accepted")
            {
                request.Status = "Completed";
                request.CompletedTime = DateTime.Now;
                return true;
            }
            return false;
        }

        // Get ride request by ID
        public RideRequest GetRideRequest(string requestId)
        {
            return _rideRequests.FirstOrDefault(r => r.Id == requestId);
        }

        // Get pending requests for driver
        public List<RideRequest> GetPendingRequests()
        {
            return _rideRequests.Where(r => r.Status == "Pending").ToList();
        }

        // Get active ride for passenger
        public RideRequest GetActiveRideForPassenger(string passengerId)
        {
            return _rideRequests.FirstOrDefault(r => r.PassengerId == passengerId && 
                (r.Status == "Pending" || r.Status == "Accepted"));
        }

        // Get active ride for driver
        public RideRequest GetActiveRideForDriver(string driverId)
        {
            return _rideRequests.FirstOrDefault(r => r.DriverId == driverId && r.Status == "Accepted");
        }

        // Update driver availability
        public void UpdateDriverAvailability(Driver driver, bool isOnline)
        {
            var existingDriver = _availableDrivers.FirstOrDefault(d => d.Phone == driver.Phone);
            if (existingDriver != null)
            {
                existingDriver.IsOnline = isOnline;
                existingDriver.Latitude = driver.Latitude;
                existingDriver.Longitude = driver.Longitude;
            }
            else
            {
                _availableDrivers.Add(driver);
            }
        }
    }
}
