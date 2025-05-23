using SmartCooking.Pages;

namespace SmartCooking
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage()); //  use your main page here

        }
    }
}