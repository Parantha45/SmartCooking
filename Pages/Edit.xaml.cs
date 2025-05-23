namespace SmartCooking.Pages;

public partial class Edit : ContentPage
{
	public Edit()
	{
		InitializeComponent();
	}
    private async void OnGoHomeButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HomePage());
    }
}