using Android.App;
using Android.Util;

using Firebase.Iid;

namespace ZillPillMobileApp.Platforms.Android.FireBase
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug("MyFirebaseIIDService", "Refreshed token: " + refreshedToken);
            SendRegistrationToServer(refreshedToken);
        }

        void SendRegistrationToServer(string token)
        {
            // Add custom implementation, as needed.
        }
    }
}
