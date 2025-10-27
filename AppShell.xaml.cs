using Plugin.Firebase.CloudMessaging;

namespace MAUIFirebaseNotification
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // InitializeFirebaseAsync();
            this.Loaded += AppShell_Loaded;

        }
        private async void AppShell_Loaded(object sender, EventArgs e)
        {
            try
            {
                await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
                var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
                Console.WriteLine($"FCM Token: {token}");

                await SendTokenToServerAsync(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Firebase init failed: {ex.Message}");
            }
        }


        private async Task SendTokenToServerAsync(string token)
        {
            try
            {
                // Example: send to your backend API (ASP.NET Core endpoint)

                var deviceId = Preferences.Get("device_id", string.Empty);
                if (string.IsNullOrEmpty(deviceId))
                {
                    deviceId = Guid.NewGuid().ToString();
                    Preferences.Set("device_id", deviceId);
                }

                var client = new HttpClient();
                var payload = new { Token = token, DeviceId = deviceId  };
                var content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(payload),
                    System.Text.Encoding.UTF8,
                    "application/json");
                await client.PostAsync("https://grani-dev-api-peter.azurewebsites.net/api/PushNotification/CreateFcmToken", content);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send token: {ex.Message}");
            }
        }


        private async void InitializeFirebaseAsync()
        {
            try
            {
                await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
                var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
                Console.WriteLine($"FCM Token: {token}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Firebase init failed: {ex.Message}");
            }
        }
    }
}
