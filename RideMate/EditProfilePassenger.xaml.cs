using Microsoft.Maui.Controls;
using RideMate.Models;
using RideMate.Services;
using System.Threading.Tasks;
using System.Diagnostics;
using System;

namespace RideMate;

public partial class EditProfilePassenger : ContentPage
{
    private Passenger _passenger;
    private readonly CloudFirestoreService _firestoreService;
  

    public EditProfilePassenger(Passenger passenger)
    {
        InitializeComponent();

        _passenger = passenger ?? throw new ArgumentNullException(nameof(passenger));
        _firestoreService = new CloudFirestoreService();
        

       
        this.BindingContext = _passenger;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // 1. Basic validation
        if (string.IsNullOrWhiteSpace(_passenger.Name) || string.IsNullOrWhiteSpace(_passenger.Email))
        {
            
            await DisplayAlert("Error", "Name and Email fields cannot be empty.", "OK");
            return;
        }

        // Disable button during save operation
        SaveButton.IsEnabled = false;

        try
        {
            // Prepare data for Firestore update
            var updates = new Dictionary<string, object>
            {
                { "Name", _passenger.Name },
                { "Email", _passenger.Email }
            };

            // 3. Perform the update call (This calls the secure CloudFirestoreService)
            // Assumes a general UpdateProfile method exists in your service layer
            bool success = await _firestoreService.UpdateUserProfile(_passenger.Id, updates);

            if (success)
            {
               
                await DisplayAlert("Success", "Your profile has been updated.", "OK");
                // Navigate back to the main dashboard
                await Navigation.PopAsync();
            }
            else
            {
               
                await DisplayAlert("Error", "Saving failed. Please try again.", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error saving passenger profile: {ex.Message}");
        
            await DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
        }
        finally
        {
            SaveButton.IsEnabled = true;
        }
    }
}