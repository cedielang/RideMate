using RideMate.Services;
using System.Linq;

namespace RideMate;

public partial class SendOtpPage : ContentPage
{
    // Variables
    string verificationId = "";
    OtpService otpService;
    string userRole;

    public SendOtpPage(string role = "User")
    {
        InitializeComponent();
        
        // Create OTP service
        otpService = new OtpService();
        
        // Save user role
        userRole = role;
    }

    // When Send OTP button is clicked
    private async void OnSendOtpClicked(object sender, EventArgs e)
    {
        // Get phone number from text box
        string phone = PhoneEntry.Text;
        
        if (string.IsNullOrEmpty(phone))
        {
            await DisplayAlert("Error", "Please enter phone number", "OK");
            return;
        }

        // Format phone number
        string formattedPhone = FormatPhone(phone);
        
        if (formattedPhone == null)
        {
            await DisplayAlert("Error", "Invalid Philippine phone number\nFormat: 09123456789", "OK");
            return;
        }

        try
        {
            // Show loading
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            SendOtpButton.IsEnabled = false;
            StatusLabel.Text = "Sending OTP...";
            StatusLabel.TextColor = Colors.Blue;
            StatusLabel.IsVisible = true;

            // Wait 2 seconds (simulate sending SMS)
            await Task.Delay(2000);

            // Send OTP
            verificationId = otpService.SendOTP(formattedPhone);

            // Hide loading
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            SendOtpButton.IsEnabled = true;
            StatusLabel.Text = "OTP sent successfully!";
            StatusLabel.TextColor = Colors.Green;

            // Get OTP for testing
            string testOtp = otpService.GetOTP(verificationId);
            await DisplayAlert("Test OTP", $"Your OTP is: {testOtp}", "OK");

            // Go to verification page
            await Navigation.PushAsync(new OtpVerification(verificationId, formattedPhone, otpService, userRole));
        }
        catch (Exception ex)
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            SendOtpButton.IsEnabled = true;
            StatusLabel.Text = "Failed to send OTP";
            StatusLabel.TextColor = Colors.Red;
            
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    // Format Philippine phone number
    string FormatPhone(string phone)
    {
        // Remove all non-digit characters
        string digitsOnly = "";
        foreach (char c in phone)
        {
            if (char.IsDigit(c))
            {
                digitsOnly += c;
            }
        }

        // Check different formats
        if (digitsOnly.StartsWith("63") && digitsOnly.Length == 12)
        {
            return "+" + digitsOnly;
        }
        else if (digitsOnly.StartsWith("09") && digitsOnly.Length == 11)
        {
            return "+63" + digitsOnly.Substring(1);
        }
        else if (digitsOnly.StartsWith("9") && digitsOnly.Length == 10)
        {
            return "+63" + digitsOnly;
        }

        return null; // Invalid
    }
}
