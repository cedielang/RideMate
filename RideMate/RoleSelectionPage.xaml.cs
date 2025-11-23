namespace RideMate;

public partial class RoleSelectionPage : ContentPage
{
	public RoleSelectionPage()
	{
		InitializeComponent();
	}

	public async void OnDriverClicked(object sender, EventArgs e)
	{
		// Navigate to Login page with driver role
		await Navigation.PushAsync(new Login("Driver"));
	}

    public async void OnPassengerClicked(object sender, EventArgs e)
    {
        // Navigate to Login page with passenger role
        await Navigation.PushAsync(new Login("Passenger"));
    }
}