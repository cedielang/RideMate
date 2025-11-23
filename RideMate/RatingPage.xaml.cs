using RideMate.Models;

namespace RideMate;

public partial class RatingPage : ContentPage
{
    private Passenger _passenger;
    private Driver _driver;
    private double _fare;
    private string _paymentMethod;
    private int _selectedRating = 0;

    public RatingPage(Passenger passenger, Driver driver, double fare, string paymentMethod)
    {
        InitializeComponent();
        
        _passenger = passenger;
        _driver = driver;
        _fare = fare;
        _paymentMethod = paymentMethod;
        
        DriverNameLabel.Text = _driver.Name;
        FareLabel.Text = $"₱{_fare:F2}";
        PaymentMethodLabel.Text = $"Paid via {_paymentMethod}";
    }

    // Star rating clicked
    private void OnStar1Clicked(object sender, EventArgs e)
    {
        SetRating(1);
    }

    private void OnStar2Clicked(object sender, EventArgs e)
    {
        SetRating(2);
    }

    private void OnStar3Clicked(object sender, EventArgs e)
    {
        SetRating(3);
    }

    private void OnStar4Clicked(object sender, EventArgs e)
    {
        SetRating(4);
    }

    private void OnStar5Clicked(object sender, EventArgs e)
    {
        SetRating(5);
    }

    // Set rating and update stars
    private void SetRating(int rating)
    {
        _selectedRating = rating;
        
        // Update star display
        Star1.Text = rating >= 1 ? "⭐" : "☆";
        Star2.Text = rating >= 2 ? "⭐" : "☆";
        Star3.Text = rating >= 3 ? "⭐" : "☆";
        Star4.Text = rating >= 4 ? "⭐" : "☆";
        Star5.Text = rating >= 5 ? "⭐" : "☆";
        
        // Update rating text
        switch (rating)
        {
            case 1:
                RatingTextLabel.Text = "Poor";
                RatingTextLabel.TextColor = Color.FromArgb("#F44336");
                break;
            case 2:
                RatingTextLabel.Text = "Fair";
                RatingTextLabel.TextColor = Color.FromArgb("#FF9800");
                break;
            case 3:
                RatingTextLabel.Text = "Good";
                RatingTextLabel.TextColor = Color.FromArgb("#FFC107");
                break;
            case 4:
                RatingTextLabel.Text = "Very Good";
                RatingTextLabel.TextColor = Color.FromArgb("#8BC34A");
                break;
            case 5:
                RatingTextLabel.Text = "Excellent";
                RatingTextLabel.TextColor = Color.FromArgb("#4CAF50");
                break;
        }
    }

    // Quick feedback buttons
    private void OnFriendlyClicked(object sender, EventArgs e)
    {
        ToggleFeedbackButton(FriendlyButton);
    }

    private void OnSafeClicked(object sender, EventArgs e)
    {
        ToggleFeedbackButton(SafeButton);
    }

    private void OnCleanClicked(object sender, EventArgs e)
    {
        ToggleFeedbackButton(CleanButton);
    }

    private void OnPunctualClicked(object sender, EventArgs e)
    {
        ToggleFeedbackButton(PunctualButton);
    }

    // Toggle feedback button selection
    private void ToggleFeedbackButton(Button button)
    {
        if (button.BackgroundColor == Color.FromArgb("#E3F2FD"))
        {
            // Deselect
            button.BackgroundColor = Colors.White;
            button.TextColor = Color.FromArgb("#666");
        }
        else
        {
            // Select
            button.BackgroundColor = Color.FromArgb("#E3F2FD");
            button.TextColor = Color.FromArgb("#1976D2");
        }
    }

    // Submit rating
    private async void OnSubmitRatingClicked(object sender, EventArgs e)
    {
        if (_selectedRating == 0)
        {
            await DisplayAlert("Rating Required", "Please select a star rating for your driver", "OK");
            return;
        }

        SubmitButton.IsEnabled = false;
        SubmitButton.Text = "Submitting...";

        try
        {
            // Get comment
            string comment = CommentEditor.Text ?? "";
            
            // Get selected feedback tags
            List<string> feedbackTags = new List<string>();
            if (FriendlyButton.BackgroundColor == Color.FromArgb("#E3F2FD"))
                feedbackTags.Add("Friendly");
            if (SafeButton.BackgroundColor == Color.FromArgb("#E3F2FD"))
                feedbackTags.Add("Safe Driver");
            if (CleanButton.BackgroundColor == Color.FromArgb("#E3F2FD"))
                feedbackTags.Add("Clean Car");
            if (PunctualButton.BackgroundColor == Color.FromArgb("#E3F2FD"))
                feedbackTags.Add("Punctual");
            
            // In real app, save rating to database
            await Task.Delay(1000);
            
            // Show success message
            await DisplayAlert("✓ Thank You!", 
                $"Your {_selectedRating}-star rating has been submitted.\n\nThank you for riding with {_driver.Name}!", 
                "OK");
            
            // Return to passenger dashboard
            await Navigation.PopToRootAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to submit rating: {ex.Message}", "OK");
            SubmitButton.IsEnabled = true;
            SubmitButton.Text = "Submit Rating";
        }
    }

    // Skip rating
    private async void OnSkipClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Skip Rating", 
            "Are you sure you want to skip rating your driver?", 
            "Yes, Skip", "No");
        
        if (confirm)
        {
            await Navigation.PopToRootAsync();
        }
    }
}
