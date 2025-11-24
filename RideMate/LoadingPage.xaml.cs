namespace RideMate;

public partial class LoadingPage : ContentPage
{
    public LoadingPage()
    {
        InitializeComponent();
        
        // Start initialization process
        InitializeApp();
    }

    private async void InitializeApp()
    {
        try
        {
            // Add fade-in animation for logo
            await Task.WhenAll(
                this.FadeTo(1, 500),
                Task.Delay(500)
            );
            
            // Initialize services (minimum 2 seconds to show the logo)
            await Task.Delay(4000);
            
            // You can add initialization tasks here:
            // - Initialize Firebase
            // - Load cached data
            // - Check for updates
            // - Verify permissions
            
            System.Diagnostics.Debug.WriteLine("✓ App initialized successfully");
            
            // Fade out before navigating
            await this.FadeTo(0, 300);
            
            // Navigate to main app
            Application.Current.MainPage = new AppShell();
        }
        catch (Exception ex)
        {
            // If initialization fails, log error and still navigate
            System.Diagnostics.Debug.WriteLine($"❌ Initialization error: {ex.Message}");
            
            // Still navigate to app even if there's an error
            Application.Current.MainPage = new AppShell();
        }
    }
}
