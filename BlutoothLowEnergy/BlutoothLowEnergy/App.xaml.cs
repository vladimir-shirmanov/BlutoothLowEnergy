using BlutoothLowEnergy.Services;
using BlutoothLowEnergy.ViewModel;
using BlutoothLowEnergy.Views;
using Xamarin.Forms;

namespace BlutoothLowEnergy
{
    public partial class App : Application
    {
        private static ViewModelLocator locator;

        public static ViewModelLocator Locator => locator ?? (locator = new ViewModelLocator());

        public App()
        {
            var firstPage = new NavigationPage(new Page1());

            ((NavigationService)Locator.NavigationService).Initialize(firstPage);

            InitializeComponent();

            MainPage = firstPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
