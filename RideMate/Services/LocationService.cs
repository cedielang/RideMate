using Microsoft.Maui.Devices.Sensors;

namespace RideMate.Services
{
    public class LocationService
    {
        private CancellationTokenSource? _cancelTokenSource;
        private bool _isCheckingLocation;

        public async Task<bool> CheckAndRequestLocationPermission()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status == PermissionStatus.Granted)
                    return true;

                if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    // Prompt the user to turn on in settings
                    return false;
                }

                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                return status == PermissionStatus.Granted;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking location permission: {ex.Message}");
                return false;
            }
        }

        public async Task<Location?> GetCurrentLocation()
        {
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                Location? location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                return location;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unable to get location: {ex.Message}");
                return null;
            }
            finally
            {
                _isCheckingLocation = false;
            }
        }

        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }
    }
}
