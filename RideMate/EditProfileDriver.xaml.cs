using Microsoft.Maui.Controls;
using RideMate.Models;
using RideMate.Services;
using System.Threading.Tasks;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace RideMate;

public partial class EditProfileDriver : ContentPage
{
    private Driver _driver;
    private readonly CloudFirestoreService _firestoreService;

    public EditProfileDriver(Driver driver)
    {
        InitializeComponent();

        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        _firestoreService = new CloudFirestoreService();

        // Use Data Binding to link UI fields to the Driver object
        this.BindingContext = _driver;
    }


    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // 1. Basic validation
        if (string.IsNullOrWhiteSpace(_driver.Name) || string.IsNullOrWhiteSpace(_driver.Email) ||
            string.IsNullOrWhiteSpace(_driver.VehicleType) || string.IsNullOrWhiteSpace(_driver.PlateNumber))
        {
            await DisplayAlert("Error", "All required fields (Name, Email, Vehicle Type, Plate Number) must be filled.", "OK");
            return;
        }

        // Disable button during save operation
        Button saveButton = (Button)sender;
        saveButton.IsEnabled = false;

        try
        {
            // 2. Prepare data for Firestore update
            var updates = new Dictionary<string, object>
            {
                { "Name", _driver.Name },
                { "Email", _driver.Email },
                { "VehicleType", _driver.VehicleType },
                { "VehicleModel", _driver.VehicleModel },
                { "PlateNumber", _driver.PlateNumber }
            };

            // 3. Perform the update call using the CloudFirestoreService.UpdateUserProfile method
            // The driver's ID (UID) is used to identify the document.
            bool success = await _firestoreService.UpdateUserProfile(_driver.Id, updates);

            if (success)
            {
                await DisplayAlert("Success", "Your profile and vehicle details have been updated.", "OK");
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
            Debug.WriteLine($"Error saving driver profile: {ex.Message}");
            await DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
        }
        finally
        {
            saveButton.IsEnabled = true;
        }
    }
}