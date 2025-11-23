using RideMate.Models;
using RideMate.Services;

namespace RideMate;

public partial class DriverDashboard : ContentPage
{
    // Variables
    Driver? currentDriver;
    private LocationService _locationService;
    private RideService _rideService;
    private FareCalculatorService _fareCalculator;
    private bool _isRouteActive = false;

    public DriverDashboard(Driver? driver = null)
    {
        InitializeComponent();
        
        // Save driver info
        currentDriver = driver;
        _locationService = new LocationService();
        _rideService = new RideService();
        _fareCalculator = new FareCalculatorService();
        
        // Set initial online status
        if (currentDriver != null)
        {
            OnlineSwitch.IsToggled = currentDriver.IsOnline;
        }
        
        // Load map
        LoadMap();
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
            await Task.Delay(1500);
            
            // Get and show driver location
            await ShowDriverLocation();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading map: {ex.Message}");
        }
    }

    // Show driver's current location on map
    private async Task ShowDriverLocation()
    {
        try
        {
            bool hasPermission = await _locationService.CheckAndRequestLocationPermission();
            
            if (hasPermission)
            {
                var location = await _locationService.GetCurrentLocation();
                
                if (location != null && currentDriver != null)
                {
                    // Save driver location
                    currentDriver.Latitude = location.Latitude;
                    currentDriver.Longitude = location.Longitude;
                    
                    // Show driver pin on map
                    await MapWebView.EvaluateJavaScriptAsync(
                        $"addMarker('driver', {location.Latitude}, {location.Longitude}, 'You are here', 'driver')");
                    
                    // Center map on driver
                    await MapWebView.EvaluateJavaScriptAsync(
                        $"centerMap({location.Latitude}, {location.Longitude}, 15)");
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error showing driver location: {ex.Message}");
        }
    }

    // When online/offline switch is toggled
    private void OnStatusToggled(object sender, ToggledEventArgs e)
    {
        // Check if online or offline
        bool isOnline = e.Value;
        
        if (isOnline)
        {
            StatusLabel.Text = "Online";
            StatusLabel.TextColor = Colors.Green;
        }
        else
        {
            StatusLabel.Text = "Offline";
            StatusLabel.TextColor = Colors.Red;
        }

        // Update driver status
        if (currentDriver != null)
        {
            currentDriver.IsOnline = isOnline;
        }
    }

    // When Start Route button is clicked
    private void OnStartRouteButtonClicked(object sender, EventArgs e)
    {
        if (!OnlineSwitch.IsToggled)
        {
            DisplayAlert("Go Online", "Please go online first to start a route", "OK");
            return;
        }
        
        // Show route input panel
        RouteInputPanel.IsVisible = true;
        StartRouteButton.IsVisible = false;
    }

    // When Set Route button is clicked
    private async void OnSetRouteClicked(object sender, EventArgs e)
    {
        string destination = DestinationEntry.Text;
        
        if (string.IsNullOrWhiteSpace(destination))
        {
            await DisplayAlert("Error", "Please enter a destination", "OK");
            return;
        }
        
        if (currentDriver == null)
        {
            await DisplayAlert("Error", "Driver information not available", "OK");
            return;
        }
        
        try
        {
            // For demo, use fixed destination coordinates
            // In real app, use geocoding service to convert address to coordinates
            double destLat = 14.6760; // Quezon City
            double destLng = 121.0437;
            
            // Calculate distance and fare
            double distance = _fareCalculator.CalculateDistance(
                currentDriver.Latitude, currentDriver.Longitude, 
                destLat, destLng);
            
            double estimatedTime = _fareCalculator.EstimateTime(distance);
            
            var fareBreakdown = _fareCalculator.CalculateFareWithBreakdown(distance, estimatedTime);
            
            // Add destination marker
            await MapWebView.EvaluateJavaScriptAsync(
                $"addMarker('destination', {destLat}, {destLng}, '{destination}', 'destination')");
            
            // Draw route with polyline
            await MapWebView.EvaluateJavaScriptAsync(
                $"setRoute({currentDriver.Latitude}, {currentDriver.Longitude}, {destLat}, {destLng})");
            
            // Fit map to show both points
            await MapWebView.EvaluateJavaScriptAsync("fitBounds()");
            
            // Hide input panel
            RouteInputPanel.IsVisible = false;
            
            // Update button to show fare
            StartRouteButton.Text = $"💰 Fare: ₱{fareBreakdown.Total:F2}";
            StartRouteButton.BackgroundColor = Color.FromArgb("#4CAF50");
            StartRouteButton.IsVisible = true;
            _isRouteActive = true;
            
            // Create ride offer with fare
            var rideOffer = new RideRequest
            {
                Id = Guid.NewGuid().ToString(),
                DriverId = currentDriver.Phone,
                DriverName = currentDriver.Name,
                PickupLatitude = currentDriver.Latitude,
                PickupLongitude = currentDriver.Longitude,
                DestinationAddress = destination,
                Status = "Available",
                RequestTime = DateTime.Now
            };
            
            _rideService.CreateDriverRideOffer(rideOffer);
            
            // Show fare breakdown
            await DisplayAlert("Route Started", 
                $"Route to {destination} is active!\n\n" +
                $"📏 Distance: {fareBreakdown.DistanceInKm:F2} km\n" +
                $"⏱️ Est. Time: {fareBreakdown.TimeInMinutes:F0} min\n\n" +
                $"💰 Estimated Fare: ₱{fareBreakdown.Total:F2}\n\n" +
                $"Passengers can now see your ride!", 
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to set route: {ex.Message}", "OK");
        }
    }

    // When Cancel button is clicked
    private void OnCancelRouteClicked(object sender, EventArgs e)
    {
        RouteInputPanel.IsVisible = false;
        StartRouteButton.IsVisible = true;
    }

    // When Vehicle Info button is clicked
    private async void OnVehicleInfoClicked(object sender, EventArgs e)
    {
        if (currentDriver != null)
        {
            string info = $"Type: {currentDriver.VehicleType}\n" +
                         $"Model: {currentDriver.VehicleModel}\n" +
                         $"Plate: {currentDriver.PlateNumber}";
            await DisplayAlert("Vehicle Information", info, "OK");
        }
    }

    // When Edit Profile button is clicked
    private async void OnEditProfileClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Edit Profile", "This feature will be added soon", "OK");
    }

    // When Logout button is clicked
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
        if (confirm)
        {
            // Clear navigation stack and go to login page
            Application.Current.MainPage = new NavigationPage(new Login());
        }
    }
}
