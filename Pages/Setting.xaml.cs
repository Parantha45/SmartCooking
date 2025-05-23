namespace SmartCooking.Pages;

public partial class Setting : ContentPage
{
	public Setting()
	{
		InitializeComponent();
	}
    private async void OnPairButtonClicked(object sender, EventArgs e)
    {
#if ANDROID
    var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

    if (status != PermissionStatus.Granted)
    {
        await DisplayAlert("Permission Denied", "Location permission is required for Bluetooth", "OK");
        return;
    }

    var bluetoothService = BluetoothService.Instance;
    await bluetoothService.ListAndConnectToDeviceAsync(this);
#else
        await DisplayAlert("Unsupported", "Bluetooth pairing is only supported on Android", "OK");
#endif
    }

}