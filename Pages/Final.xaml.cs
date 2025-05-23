namespace SmartCooking.Pages;

public partial class Final : ContentPage
{
	public Final()
	{
		InitializeComponent();
	}
    private async void OngoHomeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HomePage());
    }
}