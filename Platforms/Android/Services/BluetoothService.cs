#if ANDROID
using Android;
using Android.Bluetooth;
using Android.Content.PM;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using System.Linq;
using System.Threading.Tasks;
using Java.Util;
using System.IO;
using System.Threading;


public class BluetoothService
{
    // Singleton instance
    private static BluetoothService _instance;
    public static BluetoothService Instance => _instance ??= new BluetoothService();

    // Private constructor for singleton
    private BluetoothService() { }

    // Instance fields (was static)
    private BluetoothSocket _socket;
    private Stream _outputStream;
    private Stream _inputStream;
    private StreamReader _reader;
    private CancellationTokenSource _cancellationTokenSource;

    // Property to check connection status
    public bool IsConnected => _socket != null && _socket.IsConnected;

    // Event triggered when data is received from the Bluetooth device
    public event Action<string> OnDataReceived;


    // Connect and save the socket globally
    public async Task ListAndConnectToDeviceAsync(Page page)
    {
        var adapter = BluetoothAdapter.DefaultAdapter;
        if (adapter == null || !adapter.IsEnabled)
        {
            Toast.MakeText(Android.App.Application.Context, "Bluetooth is not available or enabled", ToastLength.Short).Show();
            return;
        }

        //  Runtime permissions for Android 12+
        if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.S)
        {
            var connectPermission = ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.BluetoothConnect);
            var scanPermission = ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.BluetoothScan);

            if (connectPermission != (int)Permission.Granted || scanPermission != (int)Permission.Granted)
            {
                var activity = Platform.CurrentActivity;
                ActivityCompat.RequestPermissions(activity, new string[] {
                    Manifest.Permission.BluetoothConnect,
                    Manifest.Permission.BluetoothScan
                }, 1001);

                Toast.MakeText(Android.App.Application.Context, "Bluetooth permissions required", ToastLength.Long).Show();
                return;
            }
        }

        var pairedDevices = adapter.BondedDevices;
        if (pairedDevices.Count > 0)
        {
            var deviceNames = pairedDevices.Select(d => d.Name).ToArray();
            string selectedDevice = await page.DisplayActionSheet("Select Device", "Cancel", null, deviceNames);

            if (!string.IsNullOrEmpty(selectedDevice) && selectedDevice != "Cancel")
            {
                var device = pairedDevices.FirstOrDefault(d => d.Name == selectedDevice);
                if (device != null)
                    ConnectToDevice(device);
            }
        }
        else
        {
            Toast.MakeText(Android.App.Application.Context, "No paired Bluetooth devices found", ToastLength.Short).Show();
        }
    }

    private void ConnectToDevice(BluetoothDevice device)
    {
        try
        {
            var uuid = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB"); // HC-05 SPP UUID
            _socket = device.CreateRfcommSocketToServiceRecord(uuid);
            BluetoothAdapter.DefaultAdapter.CancelDiscovery();
            _socket.Connect();
            _outputStream = _socket.OutputStream;
            _inputStream = _socket.InputStream;
            _reader = new StreamReader(_inputStream);

            StartListening(); // Start reading data from Arduino

            Toast.MakeText(Android.App.Application.Context, $"Connected to {device.Name}", ToastLength.Short).Show();
        }
        catch (Exception ex)
        {
            Toast.MakeText(Android.App.Application.Context, $"Connection failed: {ex.Message}", ToastLength.Long).Show();
        }
    }

    private void StartListening()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        Task.Run(async () =>
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    string line = await _reader.ReadLineAsync();
                    if (!string.IsNullOrEmpty(line))
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            OnDataReceived?.Invoke(line); // Trigger the event with received data
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Toast.MakeText(Android.App.Application.Context, $"Read error: {ex.Message}", ToastLength.Long).Show();
                });
            }
        }, token);
    }

    public void StopListening()
    {
        _cancellationTokenSource?.Cancel();
        _reader?.Dispose();
        _inputStream?.Dispose();
    }

    //  Send data using existing socket
    public async Task SendCommandToDevice(string command)
    {
        if (!IsConnected)
        {
            Toast.MakeText(Android.App.Application.Context, "Bluetooth device not connected", ToastLength.Short).Show();
            return;
        }

        try
        {
            var lines = command.Split('\n');
            foreach (var line in lines)
            {
                byte[] lineBytes = System.Text.Encoding.ASCII.GetBytes(line + "\n");
                _outputStream.Write(lineBytes, 0, lineBytes.Length);
                await Task.Delay(100); // Let Arduino process each line
            }
        }
        catch (Exception ex)
        {
            Toast.MakeText(Android.App.Application.Context, $"Error sending: {ex.Message}", ToastLength.Long).Show();
        }
    }

}
#endif
