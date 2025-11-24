using RideMate.Models;
using RideMate.Services;

namespace RideMate;

public partial class ActiveRidePage : ContentPage
{
    private Passenger _passenger;
    private Driver _driver;
    private string _destination;
    private double _fare;
    private bool _rideCompleted = false;
    private string _rideId;
    private CloudFirestoreService _firestoreService;

    public ActiveRidePage(Passenger passenger, Driver driver, string destination, double fare, string rideId = null)
    {
        InitializeComponent();
        
        _passenger = passenger;
        _driver = driver;
        _destination = destination;
        _fare = fare;
        _rideId = rideId;
        _firestoreService = new CloudFirestoreService();
        
        LoadRideInfo();
        LoadMap();
        SimulateRideProgress();
    }

    // Load ride information
    private void LoadRideInfo()
    {
        DriverNameLabel.Text = _driver.Name;
        VehicleInfoLabel.Text = $"{_driver.VehicleModel} - {_driver.PlateNumber}";
        DriverPhoneLabel.Text = $"Phone: {_driver.Phone}";
        FareLabel.Text = $"₱{_fare:F2}";
        EtaLabel.Text = "5 mins";
    }

    // Load map with route
    private async void LoadMap()
    {
        try
        {
            var htmlSource = new UrlWebViewSource
            {
                Url = $"file:///android_asset/map.html"
            };
            MapWebView.Source = htmlSource;
            
            await Task.Delay(1500);
            
            // Show route from driver to passenger to destination
            await ShowRideRoute();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading map: {ex.Message}");
        }
    }

    // Display the ride route on map
    private async Task ShowRideRoute()
    {
        try
        {
            // Driver location (example)
            double driverLat = 14.5995;
            double driverLng = 120.9842;
            
            // Passenger location (example)
            double passengerLat = 14.6042;
            double passengerLng = 120.9822;
            
            // Destination (example)
            double destLat = 14.6760;
            double destLng = 121.0437;
            
            // Add markers
            await MapWebView.EvaluateJavaScriptAsync(
                $"addMarker('driver', {driverLat}, {driverLng}, 'Driver: {_driver.Name}', 'driver')");
            
            await MapWebView.EvaluateJavaScriptAsync(
                $"addMarker('passenger', {passengerLat}, {passengerLng}, 'You', 'passenger')");
            
            await MapWebView.EvaluateJavaScriptAsync(
                $"addMarker('destination', {destLat}, {destLng}, '{_destination}', 'destination')");
            
            // Draw route from driver to passenger
            await MapWebView.EvaluateJavaScriptAsync(
                $"setRoute({driverLat}, {driverLng}, {passengerLat}, {passengerLng})");
            
            // Fit map to show all points
            await MapWebView.EvaluateJavaScriptAsync("fitBounds()");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error showing route: {ex.Message}");
        }
    }

    // Simulate ride progress (in real app, this would be real-time updates)
    private async void SimulateRideProgress()
    {
        // Wait 3 seconds
        await Task.Delay(3000);
        StatusLabel.Text = "Driver is arriving...";
        EtaLabel.Text = "2 mins";
        
        // Wait 5 seconds
        await Task.Delay(5000);
        StatusLabel.Text = "Driver has arrived!";
        EtaLabel.Text = "Arrived";
        
        // Wait 3 seconds
        await Task.Delay(3000);
        StatusLabel.Text = "Ride in progress to destination";
        
        // Update route to show passenger picked up
        await UpdateRouteToDestination();
        
        // Wait 8 seconds
        await Task.Delay(8000);
        StatusLabel.Text = "Arriving at destination...";
        EtaLabel.Text = "1 min";
        
        // Wait 5 seconds
        await Task.Delay(5000);
        StatusLabel.Text = "✓ You have arrived!";
        EtaLabel.Text = "Arrived";
        CompleteRideButton.IsVisible = true;
        _rideCompleted = true;
    }

    // Update map to show route to destination
    private async Task UpdateRouteToDestination()
    {
        try
        {
            await MapWebView.EvaluateJavaScriptAsync("clearRoute()");
            
            // Passenger location (now with driver)
            double currentLat = 14.6042;
            double currentLng = 120.9822;
            
            // Destination
            double destLat = 14.6760;
            double destLng = 121.0437;
            
            // Draw route to destination
            await MapWebView.EvaluateJavaScriptAsync(
                $"setRoute({currentLat}, {currentLng}, {destLat}, {destLng})");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error updating route: {ex.Message}");
        }
    }

    // Complete ride and go to payment
    private async void OnCompleteRideClicked(object sender, EventArgs e)
    {
        if (!_rideCompleted)
        {
            await DisplayAlert("Ride Not Complete", "Please wait until you arrive at your destination.", "OK");
            return;
        }
        
        // Update ride status to completed in Firestore
        if (!string.IsNullOrEmpty(_rideId))
        {
            await _firestoreService.UpdateRideStatus(_rideId, "Completed");
            await _firestoreService.UpdateRideFare(_rideId, _fare);
        }
        
        // Navigate to payment page
        await Navigation.PushAsync(new PaymentPage(_passenger, _driver, _fare, _rideId));
    }

    // Cancel ride
    private async void OnCancelRideClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Cancel Ride", 
            "Are you sure you want to cancel this ride?", 
            "Yes", "No");
        
        if (confirm)
        {
            await DisplayAlert("Ride Cancelled", "Your ride has been cancelled.", "OK");
            await Navigation.PopToRootAsync();
        }
    }
}
