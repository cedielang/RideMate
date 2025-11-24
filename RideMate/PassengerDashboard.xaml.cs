using RideMate.Models;

namespace RideMate;

public partial class PassengerDashboard : ContentPage
{
    // Variable to store passenger info
    Passenger? currentPassenger;

    public PassengerDashboard(Passenger? passenger = null)
    {
        InitializeComponent();
        
        // Save passenger info
        currentPassenger = passenger;
    }

    
    // When Book Ride button is clicked
    private async void OnBookRideClicked(object sender, EventArgs e)
    {
        // Navigate to map dashboard
        await Navigation.PushAsync(new PassengerMapDashboard(currentPassenger));
    }

    // When My Trips button is clicked
    private async void OnMyTripsClicked(object sender, EventArgs e)
    {
        await DisplayAlert("My Trips", "This feature will be added soon", "OK");
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
            Application.Current.MainPage = new NavigationPage(new RoleSelectionPage());
        }
    }
}
