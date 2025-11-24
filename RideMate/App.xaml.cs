namespace RideMate
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Show loading page first
            return new Window(new NavigationPage(new LoadingPage()));
        }
    }
}