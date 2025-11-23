using RideMate.Models;

namespace RideMate;

public partial class DriverSignUp : ContentPage
{
    // Variables
    string verifiedPhone;

    public DriverSignUp(string phone = "")
    {
        InitializeComponent();
        
        verifiedPhone = phone;
    }

  
    private async void OnSignUpClicked(object sender, EventArgs e)
    {
     
        string name = FullNameEntry.Text;
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;
        string license = LicenseEntry.Text;
        string vehicleType = VehicleTypeEntry.Text;
        string vehicleModel = VehicleModelEntry.Text;
        string plateNumber = PlateNumberEntry.Text;

        
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || 
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) ||
            string.IsNullOrEmpty(license) || string.IsNullOrEmpty(vehicleType) ||
            string.IsNullOrEmpty(vehicleModel) || string.IsNullOrEmpty(plateNumber))
        {
            await DisplayAlert("Error", "Please fill all fields", "OK");
            return;
        }

        
        if (password != confirmPassword)
        {
            await DisplayAlert("Error", "Passwords don't match", "OK");
            return;
        }

        
        if (password.Length < 6)
        {
            await DisplayAlert("Error", "Password must be at least 6 characters", "OK");
            return;
        }

        try
        {
            // Disable button to prevent double-click
            SignUpButton.IsEnabled = false;

            // Create new driver (without database)
            Driver newDriver = new Driver
            {
                Phone = verifiedPhone,
                Name = name,
                Email = email,
                Password = password,
                LicenseNumber = license,
                VehicleType = vehicleType,
                VehicleModel = vehicleModel,
                PlateNumber = plateNumber,
                IsOnline = false
            };

            // Show success message
            await DisplayAlert("Success", "Driver account created successfully!", "OK");
            
            // Navigate directly to dashboard - use NavigationPage to clear stack
            Application.Current.MainPage = new NavigationPage(new DriverDashboard(newDriver));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to create account: {ex.Message}", "OK");
            SignUpButton.IsEnabled = true;
        }
    }
}
