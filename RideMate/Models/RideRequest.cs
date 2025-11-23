namespace RideMate.Models
{
    public class RideRequest
    {
        public string Id { get; set; }
        public string PassengerId { get; set; }
        public string PassengerName { get; set; }
        public string PassengerPhone { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public double DestinationLatitude { get; set; }
        public double DestinationLongitude { get; set; }
        public string DestinationAddress { get; set; }
        public string Status { get; set; } // "Pending", "Accepted", "InProgress", "Completed", "Cancelled"
        public DateTime RequestTime { get; set; }
        public DateTime? AcceptedTime { get; set; }
        public DateTime? CompletedTime { get; set; }
    }

    public class LocationUpdate
    {
        public string UserId { get; set; }
        public string UserType { get; set; } // "Driver" or "Passenger"
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
