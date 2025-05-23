namespace SmartCooking.Pages;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}
    // Navigate to SearchRecipe page when Cook Now button is clicked
    private async void OnCookNowButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SearchPage());
    }

    // Navigate to ModifyRecipe page when Modify Recipe button is clicked
    private async void OnModifyRecipeButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ModifyPage());
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

}