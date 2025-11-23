using RideMate.Services;

namespace RideMate;

public partial class OtpVerification : ContentPage
{
    // Variables
    string verificationId;
    string phoneNumber;
    OtpService otpService;
    string userRole;

    public OtpVerification(string verifyId, string phone, OtpService service, string role)
    {
        InitializeComponent();
        
        // Save all variables
        verificationId = verifyId;
        phoneNumber = phone;
        otpService = service;
        userRole = role;
        
        // Show phone number
        PhoneLabel.Text = $"Code sent to {phoneNumber}";
    }

    // When Verify button is clicked
    private async void OnVerifyClicked(object sender, EventArgs e)
    {
        // Get OTP code from text box
        string enteredOtp = OtpEntry.Text;
        
        if (string.IsNullOrEmpty(enteredOtp) || enteredOtp.Length != 6)
        {
            await DisplayAlert("Error", "Please enter 6-digit code", "OK");
            return;
        }

        try
        {
            // Show loading
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            VerifyButton.IsEnabled = false;
            StatusLabel.Text = "Verifying...";
            StatusLabel.TextColor = Colors.Blue;
            StatusLabel.IsVisible = true;

            // Wait 1 second (simulate verification)
            await Task.Delay(1000);

            // Verify OTP
            bool isCorrect = otpService.VerifyOTP(verificationId, enteredOtp);

            // Hide loading
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            VerifyButton.IsEnabled = true;

            if (isCorrect)
            {
                // OTP is correct!
                StatusLabel.Text = "Verification successful!";
                StatusLabel.TextColor = Colors.Green;
                
                await DisplayAlert("Success", "Phone verified!", "OK");

                // Go to signup page based on role
                if (userRole == "Driver")
                {
                    await Navigation.PushAsync(new DriverSignUp(phoneNumber));
                }
                else
                {
                    await Navigation.PushAsync(new Signup(phoneNumber));
                }
            }
            else
            {
                // OTP is wrong
                StatusLabel.Text = "Invalid code";
                StatusLabel.TextColor = Colors.Red;
                
                await DisplayAlert("Error", "Wrong OTP code", "OK");
            }
        }
        catch (Exception ex)
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            VerifyButton.IsEnabled = true;
            StatusLabel.Text = "Verification failed";
            StatusLabel.TextColor = Colors.Red;
            
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    // When Resend button is clicked
    private async void OnResendClicked(object sender, EventArgs e)
    {
        try
        {
            // Show loading
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            ResendButton.IsEnabled = false;
            StatusLabel.Text = "Resending...";
            StatusLabel.TextColor = Colors.Blue;
            StatusLabel.IsVisible = true;

            // Wait 2 seconds
            await Task.Delay(2000);

            // Send new OTP
            string newVerificationId = otpService.SendOTP(phoneNumber);
            verificationId = newVerificationId;

            // Hide loading
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            ResendButton.IsEnabled = true;
            StatusLabel.Text = "Code resent!";
            StatusLabel.TextColor = Colors.Green;

            // Show new OTP for testing
            string testOtp = otpService.GetOTP(verificationId);
            await DisplayAlert("New Test OTP", $"Your new OTP is: {testOtp}", "OK");
        }
        catch (Exception ex)
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            ResendButton.IsEnabled = true;
            StatusLabel.Text = "Failed to resend";
            StatusLabel.TextColor = Colors.Red;
            
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
