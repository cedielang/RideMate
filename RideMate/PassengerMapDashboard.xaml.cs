using Microsoft.Maui.Controls.Shapes;
using RideMate.Models;
using RideMate.Services;

namespace RideMate;

public partial class PassengerMapDashboard : ContentPage
{
    private Passenger _passenger;
    private LocationService _locationService;
    private RideService _rideService;
    private string? _currentLocation;
    private string? _destination;

    public PassengerMapDashboard(Passenger passenger)
    {
        InitializeComponent();
        _passenger = passenger;
        _locationService = new LocationService();
        _rideService = new RideService();
        
        LoadMap();
        GetCurrentLocation();
    }

    // Load map from HTML file
    private async void LoadMap()
    {
        try
        {
            // Load map.html from Raw folder
            var htmlSource = new UrlWebViewSource
            {
                Url = $"file:///android_asset/map.html"
            };
            MapWebView.Source = htmlSource;
            
            // Wait for map to load
            await Task.Delay(1000);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading map: {ex.Message}");
        }
    }

    // Get passenger's current location
    private async void GetCurrentLocation()
    {
        try
        {
            bool hasPermission = await _locationService.CheckAndRequestLocationPermission();
            
            if (hasPermission)
            {
                var location = await _locationService.GetCurrentLocation();
                
                if (location != null)
                {
                    _currentLocation = $"{location.Latitude:F4}, {location.Longitude:F4}";
                }
                else
                {
                    _currentLocation = "14.5995, 120.9842"; // Manila default
                }
            }
            else
            {
                _currentLocation = "14.5995, 120.9842";
            }
        }
        catch (Exception ex)
        {
            _currentLocation = "14.5995, 120.9842";
            System.Diagnostics.Debug.WriteLine($"Location error: {ex.Message}");
        }
    }

    // When passenger clicks "Find Drivers" button
    private async void OnFindDriversClicked(object sender, EventArgs e)
    {
        // Get destination
        _destination = DestinationEntry.Text;
        
        // Validate
        if (string.IsNullOrWhiteSpace(_destination))
        {
            await DisplayAlert("Error", "Please enter your destination", "OK");
            return;
        }
        
        // Find drivers going to same destination
        var availableRides = _rideService.GetRidesByDestination(_destination);
        
        if (availableRides == null || !availableRides.Any())
        {
            await DisplayAlert("No Rides Found", 
                $"No drivers are currently going to '{_destination}'.\n\nPlease try again later or search for a different destination.", 
                "OK");
            return;
        }
        
        // Show available drivers
        ShowAvailableDrivers(availableRides);
        
        // Update map
        UpdateMapWithDestination();
    }

    // Display list of available drivers
    private void ShowAvailableDrivers(List<RideRequest> rides)
    {
        // Clear previous list
        DriversListContainer.Children.Clear();
        
        // Add header
        DriversListContainer.Children.Add(new Label
        {
            Text = $"✓ Found {rides.Count} driver(s) going to: {_destination}",
            FontSize = 14,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#4CAF50"),
            Margin = new Thickness(0, 0, 0, 10)
        });
        
        // Add each driver
        foreach (var ride in rides)
        {
            var driverCard = new Border
            {
                Stroke = Color.FromArgb("#E0E0E0"),
                Background = Color.FromArgb("#F5F5F5"),
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = 10
                },
                Padding = 15,
                Margin = new Thickness(0, 5)
            };
            
            var cardContent = new VerticalStackLayout { Spacing = 8 };
            
            // Driver name
            cardContent.Children.Add(new Label
            {
                Text = $"🚗 Driver: {ride.DriverName}",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black
            });
            
            // Destination
            cardContent.Children.Add(new Label
            {
                Text = $"📍 Going to: {ride.DestinationAddress}",
                FontSize = 14,
                TextColor = Color.FromArgb("#666")
            });
            
            // Request button
            var requestButton = new Button
            {
                Text = "Request Ride",
                BackgroundColor = Color.FromArgb("#4CAF50"),
                TextColor = Colors.White,
                CornerRadius = 8,
                Margin = new Thickness(0, 5, 0, 0)
            };
            
            // Handle button click
            requestButton.Clicked += async (s, args) => await OnRequestRideClicked(ride);
            
            cardContent.Children.Add(requestButton);
            driverCard.Content = cardContent;
            DriversListContainer.Children.Add(driverCard);
        }
    }

    // Update map to show destination
    private async void UpdateMapWithDestination()
    {
        try
        {
            // Parse current location
            var coords = _currentLocation.Split(',');
            double passengerLat = double.Parse(coords[0].Trim());
            double passengerLng = double.Parse(coords[1].Trim());
            
            // Add passenger marker
            await MapWebView.EvaluateJavaScriptAsync(
                $"addMarker('passenger', {passengerLat}, {passengerLng}, 'You', 'passenger')");
            
            // For demo, use destination coordinate
            double destLat = 14.6760;
            double destLng = 121.0437;
            
            // Add destination marker
            await MapWebView.EvaluateJavaScriptAsync(
                $"addMarker('destination', {destLat}, {destLng}, '{_destination}', 'destination')");
            
            // Center map to show both
            await MapWebView.EvaluateJavaScriptAsync("fitBounds()");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error updating map: {ex.Message}");
        }
    }

    // When passenger requests a ride
    private async Task OnRequestRideClicked(RideRequest ride)
    {
        bool confirm = await DisplayAlert("Confirm Ride Request", 
            $"Request ride from {ride.DriverName}?\n\nDestination: {ride.DestinationAddress}", 
            "Yes", "No");
        
        if (!confirm) return;
        
        try
        {
            // Parse current location
            var coords = _currentLocation.Split(',');
            double passengerLat = double.Parse(coords[0].Trim());
            double passengerLng = double.Parse(coords[1].Trim());
            
            // Destination coordinates (example)
            double destLat = 14.6760;
            double destLng = 121.0437;
            
            // Create ride request
            var request = new RideRequest
            {
                PassengerId = _passenger.Phone,
                PassengerName = _passenger.Name,
                PassengerPhone = _passenger.Phone,
                DriverId = ride.DriverId,
                DriverName = ride.DriverName,
                PickupLatitude = passengerLat,
                PickupLongitude = passengerLng,
                DestinationLatitude = destLat,
                DestinationLongitude = destLng,
                DestinationAddress = _destination,
                Status = "Requested",
                RequestTime = DateTime.Now
            };
            
            // Save to Firestore
            var firestoreService = new CloudFirestoreService();
            string rideId = await firestoreService.CreateRideRequest(request);
            
            // Show waiting message
            await DisplayAlert("Request Sent!", 
                $"Your ride request has been sent to {ride.DriverName}.\n\nWaiting for driver to accept...", 
                "OK");
            
            // Update map to show connection
            UpdateMapWithRideRequest(ride);
            
            // Simulate driver accepting (in real app, this would be real-time notification)
            await Task.Delay(3000);
            
            // Update ride status to accepted
            if (!string.IsNullOrEmpty(rideId))
            {
                await firestoreService.AssignDriverToRide(rideId, ride.DriverId, ride.DriverName);
            }
            
            bool accepted = await DisplayAlert("Driver Accepted!", 
                $"{ride.DriverName} has accepted your ride request!\n\nYour driver is on the way.", 
                "View Ride", "Cancel");
            
            if (accepted)
            {
                // Create driver object from ride info
                var driver = new Driver
                {
                    Name = ride.DriverName,
                    Phone = ride.DriverId,
                    VehicleModel = "Toyota Vios",
                    PlateNumber = "ABC 1234"
                };
                
                // Calculate fare (example)
                double fare = 150.00;
                
                // Navigate to active ride page
                await Navigation.PushAsync(new ActiveRidePage(_passenger, driver, _destination, fare, rideId));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to request ride: {ex.Message}", "OK");
        }
    }

    // Update map to show ride request
    private async void UpdateMapWithRideRequest(RideRequest ride)
    {
        try
        {
            // Clear existing markers
            await MapWebView.EvaluateJavaScriptAsync("clearMarkers()");
            await MapWebView.EvaluateJavaScriptAsync("clearRoute()");
            
            // Parse current location
            var coords = _currentLocation.Split(',');
            double passengerLat = double.Parse(coords[0].Trim());
            double passengerLng = double.Parse(coords[1].Trim());
            
            // Add driver marker
            await MapWebView.EvaluateJavaScriptAsync(
                $"addMarker('driver', {ride.PickupLatitude}, {ride.PickupLongitude}, 'Driver: {ride.DriverName}', 'driver')");
            
            // Add passenger marker
            await MapWebView.EvaluateJavaScriptAsync(
                $"addMarker('passenger', {passengerLat}, {passengerLng}, 'You', 'passenger')");
            
            // Add destination marker
            double destLat = 14.6760;
            double destLng = 121.0437;
            await MapWebView.EvaluateJavaScriptAsync(
                $"addMarker('destination', {destLat}, {destLng}, '{_destination}', 'destination')");
            
            // Draw route from driver to passenger
            await MapWebView.EvaluateJavaScriptAsync(
                $"setRoute({ride.PickupLatitude}, {ride.PickupLongitude}, {passengerLat}, {passengerLng})");
            
            // Fit map to show all points
            await MapWebView.EvaluateJavaScriptAsync("fitBounds()");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error updating map: {ex.Message}");
        }
    }

    // Back button
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
