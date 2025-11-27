using RideMate.Models;
using RideMate.Services;
using System.Threading.Tasks;
using System;
using Microsoft.Maui.Dispatching; // Required for IDispatcherTimer
using Plugin.CloudFirestore; // Required for CrossCloudFirestore
using System.Linq; // Required for LINQ operations

namespace RideMate;

public partial class DriverDashboard : ContentPage
{
    // Variables
    Driver? currentDriver;
    private readonly LocationService _locationService;
    private readonly RideService _rideService;
    private readonly FareCalculatorService _fareCalculator;
    private bool _isRouteActive = false;

    // REAL-TIME POLLING INTEGRATION FIELDS
    private RideMonitor _rideMonitor; // Handles the data polling logic
    private IDispatcherTimer? _pollingTimer; // The timer object for background syncing

    public DriverDashboard(Driver? driver = null)
    {
        InitializeComponent();

        // Save driver info
        currentDriver = driver;
        _locationService = new LocationService();
        _rideService = new RideService();
        _fareCalculator = new FareCalculatorService();

        // INITIALIZE THE REAL-TIME MONITOR SERVICE
        _rideMonitor = new RideMonitor(CrossCloudFirestore.Current.Instance);

        // Set initial online status
        if (currentDriver != null)
        {
            OnlineSwitch.IsToggled = currentDriver.IsOnline;

            // If already online (e.g., app resumed), start monitoring immediately
            if (currentDriver.IsOnline)
            {
                StartMonitoring();
            }
        }

        // Load map
        LoadMap();
    }

    // --- POLLING LOGIC ---

    private void StartMonitoring()
    {
        if (currentDriver == null) return;

        // Ensure only one timer is running
        StopMonitoring();

        // 1. Create a timer that fires every 5 seconds (5000 milliseconds)
        _pollingTimer = Dispatcher.CreateTimer();
        _pollingTimer.Interval = TimeSpan.FromSeconds(5);

        // 2. Define the action to execute when the timer ticks
        _pollingTimer.Tick += PollingTimer_Tick;

        // 3. Start the timer
        _pollingTimer.Start();
        System.Diagnostics.Debug.WriteLine("✓ Dashboard: Polling Timer STARTED (5s interval).");
    }

    private void StopMonitoring()
    {
        if (_pollingTimer != null && _pollingTimer.IsRunning)
        {
            _pollingTimer.Stop();
            _pollingTimer = null;
            System.Diagnostics.Debug.WriteLine("✓ Dashboard: Polling Timer STOPPED.");
        }
    }

    // 4. The method that executes every 5 seconds to check Firebase for new requests
    private async void PollingTimer_Tick(object? sender, EventArgs e)
    {
        if (_rideMonitor == null) return;

        // Call the service to get the current list of requested rides
        var newRequests = await _rideMonitor.GetIncomingRequestsAsync();

        if (newRequests.Count > 0)
        {
            System.Diagnostics.Debug.WriteLine($"[TICK] Found {newRequests.Count} active requests.");

            // Simplified Alert Logic: Show an alert for the newest request found
            var newestRequest = newRequests.First();

            // NOTE: In a production app, you would check a local list to prevent spamming the alert
            // For now, we display the alert to confirm the real-time communication is working.
            await DisplayAlert("🚨 NEW RIDE REQUEST (Polling)",
                                 $"Passenger: {newestRequest.PassengerName} has requested a ride!",
                                 "Accept");
        }
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
    private async void OnStatusToggled(object sender, ToggledEventArgs e)
    {
        // Check if online or offline
        bool isOnline = e.Value;

        if (isOnline)
        {
            StatusLabel.Text = "Online";
            StatusLabel.TextColor = Colors.Green;

            // START POLLING
            StartMonitoring();

            // Update status in Firestore
            if (currentDriver != null)
            {
                currentDriver.IsOnline = true;
                // Assuming _rideService has a method that wraps CloudFirestoreService.UpdateDriverStatus
                await _rideService.UpdateDriverStatus(currentDriver.Id, true);
            }
        }
        else
        {
            StatusLabel.Text = "Offline";
            StatusLabel.TextColor = Colors.Red;

            // STOP POLLING
            StopMonitoring();

            // Update status in Firestore
            if (currentDriver != null)
            {
                currentDriver.IsOnline = false;
                await _rideService.UpdateDriverStatus(currentDriver.Id, false);
            }
        }
    }

    // When Start Route button is clicked
    private void OnStartRouteButtonClicked(object sender, EventArgs e)
    {
        if (!OnlineSwitch.IsToggled)
        {
            // Changed to await DisplayAlert because it's a synchronous method call in an async context (best practice)
            // But since this method is not async, we use the synchronous approach here (not ideal for MAUI, but matching original code)
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
                // Using currentDriver.Id (Firebase UID) as DriverId
                DriverId = currentDriver.Id,
                DriverName = currentDriver.Name,
                PickupLatitude = currentDriver.Latitude,
                PickupLongitude = currentDriver.Longitude,
                DestinationAddress = destination,
                Status = "Available",
                // RequestTime will be set by FieldValue.ServerTimestamp in the service
            };

            // NOTE: _rideService.CreateDriverRideOffer must be updated to use CloudFirestoreService.CreateRideRequest
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
            // 1. Ensure monitoring stops immediately
            StopMonitoring();

            // 2. Set driver status offline in Firestore
            if (currentDriver != null)
            {
                await _rideService.UpdateDriverStatus(currentDriver.Id, false);
            }

            // 3. Clear navigation stack and go to login page
            Application.Current.MainPage = new NavigationPage(new RoleSelectionPage());
        }
    }
}