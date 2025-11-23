using RideMate.Models;

namespace RideMate;

public partial class Signup : ContentPage
{
    // Variables
    string verifiedPhone;

    public Signup(string phone = "")
    {
        InitializeComponent();
        
        // Save the verified phone number
        verifiedPhone = phone;
        
        AccountTypeLabel.Text = "Passenger Account";
    }

    // When Sign Up button is clicked
    private async void OnSignUpClicked(object sender, EventArgs e)
    {
        // Get all inputs
        string name = FullNameEntry.Text;
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

       
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || 
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            await DisplayAlert("Error", "Please fill all fields", "OK");
            return;
        }

        // Check if passwords match
        if (password != confirmPassword)
        {
            await DisplayAlert("Error", "Passwords don't match", "OK");
            return;
        }

        // Check password length
        if (password.Length < 6)
        {
            await DisplayAlert("Error", "Password must be at least 6 characters", "OK");
            return;
        }

        try
        {
            // Disable button to prevent double-click
            SignUpButton.IsEnabled = false;

            // Create new passenger 
            Passenger newPassenger = new Passenger
            {
                Phone = verifiedPhone,
                Name = name,
                Email = email,
                Password = password,
                Address = ""
            };

            // Show success message
            await DisplayAlert("Success", "Account created successfully!", "OK");
            
            // Navigate directly to dashboard - use NavigationPage to clear stack
            Application.Current.MainPage = new NavigationPage(new PassengerDashboard(newPassenger));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to create account: {ex.Message}", "OK");
            SignUpButton.IsEnabled = true;
        }
    }
}
