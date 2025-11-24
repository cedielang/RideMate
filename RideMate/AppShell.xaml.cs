namespace RideMate
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            // Register routes for navigation
            Routing.RegisterRoute("loading", typeof(LoadingPage));
            Routing.RegisterRoute("sendotp", typeof(SendOtpPage));
            Routing.RegisterRoute("otpverification", typeof(OtpVerification));
            Routing.RegisterRoute("roleselection", typeof(RoleSelectionPage));
            Routing.RegisterRoute("login", typeof(Login));
            Routing.RegisterRoute("signup", typeof(Signup));
            Routing.RegisterRoute("driversignup", typeof(DriverSignUp));
            Routing.RegisterRoute("passengerdashboard", typeof(PassengerDashboard));
            Routing.RegisterRoute("driverdashboard", typeof(DriverDashboard));
            Routing.RegisterRoute("activeride", typeof(ActiveRidePage));
            Routing.RegisterRoute("payment", typeof(PaymentPage));
            Routing.RegisterRoute("rating", typeof(RatingPage));
            Routing.RegisterRoute(nameof(Signup), typeof(Signup));
            Routing.RegisterRoute(nameof(Login), typeof(Login));
        }
    }
}
