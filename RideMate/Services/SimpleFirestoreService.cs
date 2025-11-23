using RideMate.Models;
using Newtonsoft.Json;
using System.Text;

namespace RideMate.Services
{
    // Simple Firestore Service using REST API
    // This is easier for beginners - no complex authentication needed!
    public class SimpleFirestoreService
    {
        // Your Firebase project details
        string projectId = "ridemate-7c600";
        string apiKey = "YOUR_FIREBASE_WEB_API_KEY"; // Get this from Firebase Console
        HttpClient httpClient;

        public SimpleFirestoreService()
        {
            httpClient = new HttpClient();
        }

        // ========== DRIVER METHODS ==========

        // Add new driver
        public async Task<bool> AddDriver(Driver driver)
        {
            try
            {
                // Create unique ID
                driver.Id = Guid.NewGuid().ToString();

                // Convert driver to JSON
                string json = JsonConvert.SerializeObject(driver);

                // Build URL
                string url = $"https://firestore.googleapis.com/v1/projects/{projectId}/databases/(default)/documents/drivers?documentId={driver.Id}&key={apiKey}";

                // Create request
                StringContent content = new StringContent(ConvertToFirestoreFormat(driver), Encoding.UTF8, "application/json");

                // Send to Firebase
                HttpResponseMessage response = await httpClient.PostAsync(url, content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine("Error adding driver: " + error.Message);
                return false;
            }
        }

        // Find driver by phone and password
        public async Task<Driver> FindDriver(string phone, string password)
        {
            try
            {
                // Build query URL
                string url = $"https://firestore.googleapis.com/v1/projects/{projectId}/databases/(default)/documents/drivers?key={apiKey}";

                // Get all drivers
                HttpResponseMessage response = await httpClient.GetAsync(url);
                string responseJson = await response.Content.ReadAsStringAsync();

                // Parse response
                dynamic result = JsonConvert.DeserializeObject(responseJson);

                if (result?.documents != null)
                {
                    foreach (var doc in result.documents)
                    {
                        Driver driver = ParseDriverFromFirestore(doc);
                        
                        if (driver.Phone == phone && driver.Password == password)
                        {
                            return driver;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine("Error finding driver: " + error.Message);
            }

            return null;
        }

        // Helper: Convert Driver to Firestore format
        string ConvertToFirestoreFormat(Driver driver)
        {
            var firestoreDoc = new
            {
                fields = new
                {
                    Id = new { stringValue = driver.Id },
                    Name = new { stringValue = driver.Name },
                    Phone = new { stringValue = driver.Phone },
                    Email = new { stringValue = driver.Email },
                    Password = new { stringValue = driver.Password },
                    LicenseNumber = new { stringValue = driver.LicenseNumber },
                    VehicleType = new { stringValue = driver.VehicleType },
                    VehicleModel = new { stringValue = driver.VehicleModel },
                    PlateNumber = new { stringValue = driver.PlateNumber },
                    IsOnline = new { booleanValue = driver.IsOnline },
                    IsVerified = new { booleanValue = driver.IsVerified },
                    Latitude = new { doubleValue = driver.Latitude },
                    Longitude = new { doubleValue = driver.Longitude }
                }
            };

            return JsonConvert.SerializeObject(firestoreDoc);
        }

        // Helper: Parse Driver from Firestore format
        Driver ParseDriverFromFirestore(dynamic doc)
        {
            var fields = doc.fields;
            
            return new Driver
            {
                Id = fields.Id?.stringValue?.ToString() ?? "",
                Name = fields.Name?.stringValue?.ToString() ?? "",
                Phone = fields.Phone?.stringValue?.ToString() ?? "",
                Email = fields.Email?.stringValue?.ToString() ?? "",
                Password = fields.Password?.stringValue?.ToString() ?? "",
                LicenseNumber = fields.LicenseNumber?.stringValue?.ToString() ?? "",
                VehicleType = fields.VehicleType?.stringValue?.ToString() ?? "",
                VehicleModel = fields.VehicleModel?.stringValue?.ToString() ?? "",
                PlateNumber = fields.PlateNumber?.stringValue?.ToString() ?? "",
                IsOnline = fields.IsOnline?.booleanValue ?? false,
                IsVerified = fields.IsVerified?.booleanValue ?? false,
                Latitude = fields.Latitude?.doubleValue ?? 0.0,
                Longitude = fields.Longitude?.doubleValue ?? 0.0
            };
        }

        // Similar methods for Passenger...
        // (Add more methods as needed)
    }
}
