namespace SmartCooking.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HomePage());
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Setting());
    }

    private async void OnAboutClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AboutPage());
    }
}