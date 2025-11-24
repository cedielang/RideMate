using System.Net.Http;
using System.Text.Json;

namespace RideMate.Services
{
    // Map Service - handles map and routing with Maptiler API
    public class MapService
    {
        private readonly HttpClient httpClient;
        private readonly string apiKey = "eyJvcmciOiI1YjNjZTM1OTc4NTExMTAwMDFjZjYyNDgiLCJpZCI6Ijc5MDUxZmRlYWQyMTQ1Nzc4ZWVlZGJlMGU3NDA2MGFjIiwiaCI6Im11cm11cjY0In0=";


        public MapService()
        {
            httpClient = new HttpClient();
        }

        // Get route between two points
        public async Task<RouteInfo> GetRoute(double startLat, double startLon, double endLat, double endLon)
        {
            try
            {
                // Build Maptiler Directions API URL
                string url = $"https://api.maptiler.com/directions/{startLon},{startLat};{endLon},{endLat}.json?key={apiKey}";

                // Make request
                HttpResponseMessage response = await httpClient.GetAsync(url);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Parse response
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("routes", out JsonElement routes) && routes.GetArrayLength() > 0)
                {
                    JsonElement firstRoute = routes[0];

                    // Get distance and duration
                    double distance = firstRoute.GetProperty("distance").GetDouble() / 1000; // Convert to km
                    double duration = firstRoute.GetProperty("duration").GetDouble() / 60; // Convert to minutes

                    // Get route coordinates
                    List<Location> routePoints = new List<Location>();

                    if (firstRoute.TryGetProperty("geometry", out JsonElement geometry))
                    {
                        if (geometry.TryGetProperty("coordinates", out JsonElement coordinates))
                        {
                            foreach (JsonElement coord in coordinates.EnumerateArray())
                            {
                                double lon = coord[0].GetDouble();
                                double lat = coord[1].GetDouble();
                                routePoints.Add(new Location { Latitude = lat, Longitude = lon });
                            }
                        }
                    }

                    return new RouteInfo
                    {
                        DistanceKm = distance,
                        DurationMinutes = duration,
                        RoutePoints = routePoints
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error getting route: " + ex.Message);
            }

            return null;
        }

        // Calculate distance between two points (in kilometers)
        public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // Haversine formula
            double R = 6371; // Earth radius in km

            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                      Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                      Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c;

            return distance;
        }

        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }

    // Route information class
    public class RouteInfo
    {
        public double DistanceKm { get; set; }
        public double DurationMinutes { get; set; }
        public List<Location> RoutePoints { get; set; } = new List<Location>();
    }

    // Location class
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public static implicit operator Location?(Microsoft.Maui.Devices.Sensors.Location? v)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Microsoft.Maui.Devices.Sensors.Location(Location v)
        {
            throw new NotImplementedException();
        }
    }
}
