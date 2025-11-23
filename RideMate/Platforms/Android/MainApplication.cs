using Android.App;
using Android.Runtime;
using Plugin.Firebase;
using Plugin.Firebase.Core;
using Plugin.Firebase.Core.Platforms.Android;

namespace RideMate
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override void OnCreate()
        {
            base.OnCreate();


            try
            {
                if (MainActivity.Instance != null)
                {
                    CrossFirebase.Initialize(MainActivity.Instance);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Firebase init error: {ex.Message}");
            }
        }

        
    }
}
