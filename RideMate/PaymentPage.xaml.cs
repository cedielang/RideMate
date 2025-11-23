using RideMate.Models;

namespace RideMate;

public partial class PaymentPage : ContentPage
{
    private Passenger _passenger;
    private Driver _driver;
    private double _fare;
    private string _selectedPaymentMethod = "";

    public PaymentPage(Passenger passenger, Driver driver, double fare)
    {
        InitializeComponent();
        
        _passenger = passenger;
        _driver = driver;
        _fare = fare;
        
        FareLabel.Text = $"₱{_fare:F2}";
    }

    // GCash selected
    private void OnGCashSelected(object sender, EventArgs e)
    {
        _selectedPaymentMethod = "GCash";
        UpdatePaymentSelection();
    }

    // Maya selected
    private void OnMayaSelected(object sender, EventArgs e)
    {
        _selectedPaymentMethod = "Maya";
        UpdatePaymentSelection();
    }

    // Cash selected
    private void OnCashSelected(object sender, EventArgs e)
    {
        _selectedPaymentMethod = "Cash";
        UpdatePaymentSelection();
    }

    // Update UI to show selected payment method
    private void UpdatePaymentSelection()
    {
        // Reset all
        GCashCheck.Text = "○";
        MayaCheck.Text = "○";
        CashCheck.Text = "○";
        
        GCashBorder.BackgroundColor = Colors.White;
        MayaBorder.BackgroundColor = Colors.White;
        CashBorder.BackgroundColor = Colors.White;
        
        // Highlight selected
        switch (_selectedPaymentMethod)
        {
            case "GCash":
                GCashCheck.Text = "●";
                GCashBorder.BackgroundColor = Color.FromArgb("#E3F2FD");
                PayButton.Text = "Pay ₱" + _fare.ToString("F2") + " with GCash";
                break;
            case "Maya":
                MayaCheck.Text = "●";
                MayaBorder.BackgroundColor = Color.FromArgb("#E8F5E9");
                PayButton.Text = "Pay ₱" + _fare.ToString("F2") + " with Maya";
                break;
            case "Cash":
                CashCheck.Text = "●";
                CashBorder.BackgroundColor = Color.FromArgb("#F1F8E9");
                PayButton.Text = "Confirm Cash Payment";
                break;
        }
    }

    // Process payment
    private async void OnPayClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_selectedPaymentMethod))
        {
            await DisplayAlert("Select Payment", "Please select a payment method", "OK");
            return;
        }

        PayButton.IsEnabled = false;
        PayButton.Text = "Processing...";

        try
        {
            if (_selectedPaymentMethod == "GCash")
            {
                await ProcessGCashPayment();
            }
            else if (_selectedPaymentMethod == "Maya")
            {
                await ProcessMayaPayment();
            }
            else if (_selectedPaymentMethod == "Cash")
            {
                await ProcessCashPayment();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Payment Error", $"Failed to process payment: {ex.Message}", "OK");
            PayButton.IsEnabled = true;
            UpdatePaymentSelection();
        }
    }

    // Process GCash payment
    private async Task ProcessGCashPayment()
    {
        // Show loading
        await DisplayAlert("Opening GCash", "You will be redirected to GCash to complete payment...", "OK");
        
        // Simulate payment processing
        await Task.Delay(2000);
        
        // In real app, this would:
        // 1. Open GCash app or web payment
        // 2. Process payment via GCash API
        // 3. Verify payment status
        // 4. Update database
        
        // For demo, simulate successful payment
        bool paymentSuccess = await SimulateOnlinePayment("GCash");
        
        if (paymentSuccess)
        {
            await DisplayAlert("✓ Payment Successful", 
                $"Your payment of ₱{_fare:F2} via GCash has been processed successfully!", 
                "OK");
            
            // Go to rating page
            await Navigation.PushAsync(new RatingPage(_passenger, _driver, _fare, "GCash"));
        }
        else
        {
            await DisplayAlert("Payment Failed", "GCash payment was cancelled or failed. Please try again.", "OK");
            PayButton.IsEnabled = true;
            UpdatePaymentSelection();
        }
    }

    // Process Maya payment
    private async Task ProcessMayaPayment()
    {
        // Show loading
        await DisplayAlert("Opening Maya", "You will be redirected to Maya to complete payment...", "OK");
        
        // Simulate payment processing
        await Task.Delay(2000);
        
        // In real app, this would:
        // 1. Open Maya app or web payment
        // 2. Process payment via Maya API
        // 3. Verify payment status
        // 4. Update database
        
        // For demo, simulate successful payment
        bool paymentSuccess = await SimulateOnlinePayment("Maya");
        
        if (paymentSuccess)
        {
            await DisplayAlert("✓ Payment Successful", 
                $"Your payment of ₱{_fare:F2} via Maya has been processed successfully!", 
                "OK");
            
            // Go to rating page
            await Navigation.PushAsync(new RatingPage(_passenger, _driver, _fare, "Maya"));
        }
        else
        {
            await DisplayAlert("Payment Failed", "Maya payment was cancelled or failed. Please try again.", "OK");
            PayButton.IsEnabled = true;
            UpdatePaymentSelection();
        }
    }

    // Process cash payment
    private async Task ProcessCashPayment()
    {
        bool confirm = await DisplayAlert("Confirm Cash Payment", 
            $"Please pay ₱{_fare:F2} in cash to your driver.\n\nHave you paid the driver?", 
            "Yes, Paid", "Cancel");
        
        if (confirm)
        {
            await DisplayAlert("✓ Payment Confirmed", 
                "Thank you! Your cash payment has been recorded.", 
                "OK");
            
            // Go to rating page
            await Navigation.PushAsync(new RatingPage(_passenger, _driver, _fare, "Cash"));
        }
        else
        {
            PayButton.IsEnabled = true;
            UpdatePaymentSelection();
        }
    }

    // Simulate online payment (GCash/Maya)
    private async Task<bool> SimulateOnlinePayment(string method)
    {
        // In real app, this would integrate with actual payment gateway
        // For demo, we'll simulate a payment screen
        
        await Task.Delay(1500);
        
        // Simulate payment confirmation (90% success rate for demo)
        Random random = new Random();
        bool success = random.Next(100) < 90;
        
        return success;
    }
}
