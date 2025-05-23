using Microsoft.Maui.Controls;
using SmartCooking.Models;
using System;
using System.Threading.Tasks;

#if ANDROID
using SmartCooking.Helpers;
using Microsoft.Maui.Dispatching;
#endif

namespace SmartCooking.Pages
{
    public partial class ShowProcess : ContentPage
    {
        private RecipeDB _recipe;
        private double _hours;
        private double _minutes;

        private bool spiceDone = false;
        private bool liquidDone = false;

        public ShowProcess()
        {
            InitializeComponent();
            CookButton.IsEnabled = false;
        }

        public ShowProcess(RecipeDB recipe, double hours, double minutes) : this()
        {
            _recipe = recipe;
            _hours = hours;
            _minutes = minutes;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
#if ANDROID
            BluetoothService.Instance.OnDataReceived += OnDataReceived;
#endif
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
#if ANDROID
            BluetoothService.Instance.OnDataReceived -= OnDataReceived;
#endif
        }

#if ANDROID
        private async void OndispenseClicked(object sender, EventArgs e)
        {
            bool confirmed = await DisplayAlert(
                "Start Dispensing",
                "Are you sure you want to begin the dispensing process?",
                "Yes", "No");

            if (!confirmed)
                return;

            try
            {
                // Reset states
                spiceDone = false;
                liquidDone = false;

                // Start animation tasks immediately
                _ = AnimateSpiceBarUntilComplete();
                _ = AnimateLiquidBarUntilComplete();

                // Send command to Arduino
                string command = "START_DISPENSE";
                BluetoothService.Instance.SendCommandToDevice(command);

                await DisplayAlert("Command Sent", "Dispensing signal sent to device.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Bluetooth Error", $"Failed to send command: {ex.Message}", "OK");
            }
        }

        private async void OnDataReceived(string data)
        {
            data = data.Trim();

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                switch (data)
                {
                    case "S":
                        spiceDone = true;
                        spiceDispenseProgressBar.Progress = 1;
                        break;
                    case "L":
                        liquidDone = true;
                        liquidDispenseProgressBar.Progress = 1;
                        break;
                }

                if (spiceDone && liquidDone)
                {
                    await DisplayAlert("Done", "Dispensing process completed.", "OK");
                    CookButton.IsEnabled = true;
                }
            });
        }
#endif

        private async Task AnimateSpiceBarUntilComplete()
        {
            spiceDispenseProgressBar.Progress = 0;
            while (!spiceDone && spiceDispenseProgressBar.Progress < 1)
            {
                spiceDispenseProgressBar.Progress += 0.01;
                await Task.Delay(100);
            }

            if (!spiceDone)
                spiceDispenseProgressBar.Progress = 1;
        }

        private async Task AnimateLiquidBarUntilComplete()
        {
            liquidDispenseProgressBar.Progress = 0;
            while (!liquidDone && liquidDispenseProgressBar.Progress < 1)
            {
                liquidDispenseProgressBar.Progress += 0.01;
                await Task.Delay(100);
            }

            if (!liquidDone)
                liquidDispenseProgressBar.Progress = 1;
        }


        private async void OnCookClicked(object sender, EventArgs e)
        {
            if (spiceDispenseProgressBar.Progress >= 1 &&
                liquidDispenseProgressBar.Progress >= 1 )
            {
#if ANDROID
                try {
                    string command = "C";
                    BluetoothService.Instance.SendCommandToDevice(command);
                    await DisplayAlert("Command Sent", "Dispensing signal sent to device", "OK");
                    
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Bluetooth Error", $"Failed to send command: {ex.Message}", "OK");
                }
#endif
                await Navigation.PushAsync(new Cooking(
                        _recipe.DurationHours,
                        _recipe.DurationMinutes));

            }
            else
            {
                await DisplayAlert("Please wait", "Dispensing in progress…", "OK");
            }
        }
    }
}


