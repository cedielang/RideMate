namespace RideMate;

public partial class ForgotPasswordPage : ContentPage
{
    // Variables
    string userRole;

    public ForgotPasswordPage(string role)
    {
        InitializeComponent();
        
        // Save user role (Driver or Passenger)
        userRole = role;
    }

    // When Reset Password button is clicked
    private async void OnResetPasswordClicked(object sender, EventArgs e)
    {
        // Get inputs
        string phone = PhoneEntry.Text;
        string newPassword = NewPasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        // Step 1: Check if all fields are filled
        if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
        {
            await DisplayAlert("Error", "Please fill all fields", "OK");
            return;
        }

        // Step 2: Format phone number
        string formattedPhone = FormatPhone(phone);
        
        if (formattedPhone == null)
        {
            await DisplayAlert("Error", "Invalid Philippine phone number", "OK");
            return;
        }

        // Step 3: Check if passwords match
        if (newPassword != confirmPassword)
        {
            await DisplayAlert("Error", "Passwords don't match", "OK");
            return;
        }

        // Step 4: Check password length
        if (newPassword.Length < 6)
        {
            await DisplayAlert("Error", "Password must be at least 6 characters", "OK");
            return;
        }

        // Show loading
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;
        ResetButton.IsEnabled = false;
        StatusLabel.Text = "Resetting password...";
        StatusLabel.TextColor = Colors.Blue;
        StatusLabel.IsVisible = true;

        // Simulate password reset (no database)
        await Task.Delay(1500);

        // Hide loading
        LoadingIndicator.IsVisible = false;
        LoadingIndicator.IsRunning = false;
        ResetButton.IsEnabled = true;
        StatusLabel.Text = "Password reset successful!";
        StatusLabel.TextColor = Colors.Green;
        
        await DisplayAlert("Success", "Your password has been reset successfully!", "OK");
        
        // Go back to login
        await Navigation.PopAsync();
    }

    // Format Philippine phone number
    string FormatPhone(string phone)
    {
        // Remove spaces and dashes
        phone = phone.Replace(" ", "").Replace("-", "");
        
        // Check different formats
        if (phone.StartsWith("09") && phone.Length == 11)
        {
            return "+63" + phone.Substring(1);
        }
        else if (phone.StartsWith("9") && phone.Length == 10)
        {
            return "+63" + phone;
        }
        else if (phone.StartsWith("63") && phone.Length == 12)
        {
            return "+" + phone;
        }
        else if (phone.StartsWith("+63") && phone.Length == 13)
        {
            return phone;
        }
        
        return null;
    }

    // When Back button is clicked
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
