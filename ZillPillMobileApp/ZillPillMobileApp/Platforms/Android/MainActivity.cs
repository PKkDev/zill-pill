using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android;

using Plugin.Fingerprint;

using Android.Gms.Common;

using Firebase.Messaging;
using Firebase.Iid;

namespace ZillPillMobileApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    internal static readonly string CHANNEL_ID = "my_notification_channel";

    internal static readonly int NOTIFICATION_ID = 100;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        #region FCM

        bool avail = IsPlayServicesAvailable();
        CreateNotificationChannel();
        var token = FirebaseInstanceId.Instance.Token;

        #endregion FCM

        #region Fingerprint

        CrossFingerprint.SetCurrentActivityResolver(() => Platform.CurrentActivity);

        #endregion Fingerprint
    }

    protected override void OnResume()
    {
        #region FCM

        bool avail = IsPlayServicesAvailable();

        #endregion FCM

        base.OnResume();
    }

    /// <summary>
    /// тест на подключение к google сервиса
    /// </summary>
    /// <returns></returns>
    public bool IsPlayServicesAvailable()
    {
        var resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
        if (resultCode != ConnectionResult.Success)
            return false;
        return true;
    }

    /// <summary>
    /// создание канала для google сервиса
    /// </summary>
    private void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.O)
        {
            // Notification channels are new in API 26 (and not a part of the
            // support library). There is no need to create a notification 
            // channel on older versions of Android.
            return;
        }

        var channel = new NotificationChannel(CHANNEL_ID, "FCM Notifications", NotificationImportance.Default)
        {
            Description = "Firebase Cloud Messages appear in this channel"
        };

        var notificationManager = (NotificationManager)GetSystemService(NotificationService);
        notificationManager.CreateNotificationChannel(channel);
    }

    private void SetSubscribers()
    {
        FirebaseMessaging.Instance.SubscribeToTopic("system");
        FirebaseMessaging.Instance.SubscribeToTopic("sheduller");
    }

    private void CheckPermission()
    {
        if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != (int)Permission.Granted)
        {
            ActivityCompat.RequestPermissions(
                this,
                new String[] {
                        Manifest.Permission.Camera,
                        Manifest.Permission.ReadExternalStorage,
                        Manifest.Permission.ReadContacts,
                        Manifest.Permission.WriteContacts,
                },
                20);
        }
    }
}
