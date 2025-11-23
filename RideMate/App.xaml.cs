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
            // Go directly to main app
            return new Window(new AppShell());
        }
    }
}