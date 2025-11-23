using RideMate.Models;

namespace RideMate;

public partial class Login : ContentPage
{
    // Variables to store user role
    string userRole;

    public Login(string role)
    {
        InitializeComponent();
        
        // Save the role (Driver or Passenger)
        userRole = role;
        
        // Show role in label
        AccountTypeLabel.Text = $"Login as {role}";
    }

    public Login()
    {
    }

    // When Login button is clicked
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // Get phone and password from text boxes
        string phone = PhoneNumberEntry.Text;
        string password = PasswordEntry.Text;

        // Check if empty
        if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Error", "Please enter phone and password", "OK");
            return;
        }

        // Format phone number
        string formattedPhone = FormatPhone(phone);
        
        if (formattedPhone == null)
        {
            await DisplayAlert("Error", "Invalid Philippine phone number", "OK");
            return;
        }

        // For now, just create a demo user and login (no database)
        if (userRole == "Driver")
        {
            Driver demoDriver = new Driver
            {
                Phone = formattedPhone,
                Name = "Demo Driver",
                Email = "driver@demo.com",
                Password = password,
                LicenseNumber = "DEMO123",
                VehicleType = "Sedan",
                VehicleModel = "Toyota",
                PlateNumber = "ABC123",
                IsOnline = false
            };
            
            await DisplayAlert("Success", $"Welcome {demoDriver.Name}!", "OK");
            await Navigation.PushAsync(new DriverDashboard(demoDriver));
        }
        else // Passenger
        {
            Passenger demoPassenger = new Passenger
            {
                Phone = formattedPhone,
                Name = "Demo Passenger",
                Email = "passenger@demo.com",
                Password = password,
                Address = ""
            };
            
            await DisplayAlert("Success", $"Welcome {demoPassenger.Name}!", "OK");
            await Navigation.PushAsync(new PassengerDashboard(demoPassenger));
        }
    }

    // Format Philippine phone number
    string FormatPhone(string phone)
    {
        // Remove spaces and dashes
        phone = phone.Replace(" ", "").Replace("-", "");
        
        // Check different formats
        if (phone.StartsWith("09") && phone.Length == 11)
        {
            // 09123456789 -> +639123456789
            return "+63" + phone.Substring(1);
        }
        else if (phone.StartsWith("9") && phone.Length == 10)
        {
            // 9123456789 -> +639123456789
            return "+63" + phone;
        }
        else if (phone.StartsWith("63") && phone.Length == 12)
        {
            // 639123456789 -> +639123456789
            return "+" + phone;
        }
        else if (phone.StartsWith("+63") && phone.Length == 13)
        {
            // Already formatted
            return phone;
        }
        
        return null; // Invalid format
    }

    // When Sign Up button is clicked
    private async void OnSignupClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SendOtpPage(userRole));
    }

    // When Forgot Password is clicked
    private async void OnForgotPasswordClicked(object sender, EventArgs e)
    {
        // Navigate to Forgot Password page
        await Navigation.PushAsync(new ForgotPasswordPage(userRole));
    }

    // When Back button is clicked
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
