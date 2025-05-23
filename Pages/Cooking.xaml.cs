using Microsoft.Maui.Controls;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartCooking.Pages
{
    public partial class Cooking : ContentPage
    {
        private TimeSpan _timeRemaining;
        private bool _isPaused;
        private bool _isStarted;

        // Constructor accepting recipe duration
        public Cooking(int durationHours, int durationMinutes)
        {
            InitializeComponent();
            _isPaused = true;
            _isStarted = false;
            _timeRemaining = new TimeSpan(durationHours, durationMinutes, 0); // Set timer to recipe's duration
            UpdateTimerLabel();
        }

        // This method is called when the page appears
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Automatically start the timer when the page is displayed
            StartTimer();
        }

        // Method to start the timer
        private void StartTimer()
        {
            if (_isPaused)
            {
                _isPaused = false;
                _isStarted = true;
                pauseStartButton.Text = "Pause";
                Device.StartTimer(TimeSpan.FromSeconds(1), OnTimerTick); // Start the timer
            }
        }

        // Event handler for Pause/Start button
        private async void OnPauseStartButtonClicked(object sender, EventArgs e)
        {
            if (_isPaused)
            {
                // Start the timer
                StartTimer();

#if ANDROID
                try
                {
                    string command = "S*";
                    BluetoothService.Instance.SendCommandToDevice(command);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Bluetooth Error", $"Failed to send resume command: {ex.Message}", "OK");
                }
#endif
            }
            else
            {
                // Pause the timer
                _isPaused = true;
                pauseStartButton.Text = "Start";
#if ANDROID
                try
                {
                    string command = "P_";
                    BluetoothService.Instance.SendCommandToDevice(command);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Bluetooth Error", $"Failed to send resume command: {ex.Message}", "OK");
                }
#endif
            }
        }

        // Timer ticking logic (updates every second)
        private bool OnTimerTick()
        {
            if (_isPaused) return true;

            // Decrease the remaining time by 1 second
            _timeRemaining = _timeRemaining.Subtract(TimeSpan.FromSeconds(1));
            if (_timeRemaining <= TimeSpan.Zero)
            {
                _timeRemaining = TimeSpan.Zero;
#if ANDROID
                try
                {
                    string command = "F#";
                    BluetoothService.Instance.SendCommandToDevice(command);
                }
                catch (Exception ex)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await DisplayAlert("Bluetooth Error", $"Failed to send finish command: {ex.Message}", "OK");
                    });
                }
#endif
                // Timer is done, navigate to Final page
                Device.BeginInvokeOnMainThread(() => Navigation.PushAsync(new Final()));
                return false; // Stop the timer once time is up
            }

            UpdateTimerLabel();
            return true; // Continue the timer
        }

        // Update the timer label
        private void UpdateTimerLabel()
        {
            // Format the timer as hh:mm:ss and display it
            timerLabel.Text = _timeRemaining.ToString(@"hh\:mm\:ss");
        }

        // Event handler for Stop button (navigates to ShowDetails page)
        private async void OnStopButtonClicked(object sender, EventArgs e)
        {
#if ANDROID
            try
            {
                string command = "P_"; // Signal to Arduino to stop cooking
                BluetoothService.Instance.SendCommandToDevice(command);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Bluetooth Error", $"Failed to send stop command: {ex.Message}", "OK");
            }
#endif

            // Navigate to final screen
            await Navigation.PushAsync(new Final());
        }

    }
}

