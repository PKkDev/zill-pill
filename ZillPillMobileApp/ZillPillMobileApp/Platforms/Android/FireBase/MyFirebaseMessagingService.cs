using Android.App;
using Android.Content;
using AndroidX.Core.App;

using Firebase.Messaging;
using Java.Util.Concurrent;
using AndroidApp = Android.App.Application;

namespace ZillPillMobileApp.Platforms.Android.FireBase
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage message)
        {
            string body;
            string title;
            if (!message.Data.ContainsKey("body"))
            {
                body = message.GetNotification().Body;
                title = message.GetNotification().Title;
            }
            else
            {
                body = message.Data["body"];
                title = message.Data["title"];
            }
            SendNotification(title, body, message.Data);
        }

        void SendNotification(string title, string messageBody, IDictionary<string, string> data)
        {
            //var intent = new Intent(this, typeof(MainActivity));
            //intent.AddFlags(ActivityFlags.ClearTop);
            //foreach (var key in data.Keys)
            //    intent.PutExtra(key, data[key]);

            //PendingIntent pendingIntent = PendingIntent.GetActivity(
            //    this,
            //    MainActivity.NOTIFICATION_ID,
            //    intent,
            //    PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this, CustomConstants.CHANNEL_FCM_ID)
                .SetContentTitle(title)
                .SetContentText(messageBody)
                .SetContentIntent(BuildIntentToShowMainActivity())
                .SetSmallIcon(Resource.Drawable.phone)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                .SetAutoCancel(true)
            //.SetContentIntent(pendingIntent);
            ;

            var notification = notificationBuilder.Build();
            NotificationManager manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);
            manager.Notify(CustomConstants.NOTIFICATION_FCM_ID, notification);

            //var notificationManager = NotificationManagerCompat.From(this);
            //notificationManager.Notify(CustomConstants.NOTIFICATION_FCM_ID, notificationBuilder.Build());
        }

        /// <summary>
        /// действие на переход в приложение
        /// </summary>
        /// <returns></returns>
        PendingIntent BuildIntentToShowMainActivity()
        {
            Intent notificationIntent = new Intent(this, typeof(MainActivity))
                .AddFlags(ActivityFlags.ClearTop)
                .SetAction(CustomConstants.ACTION_MAIN_ACTIVITY)
                .SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask)
                .PutExtra("notif", "openMain")
                .PutExtra("id", CustomConstants.NOTIFICATION_FCM_ID.ToString()); ;

            PendingIntent pendingIntent = PendingIntent.GetActivity(this, CustomConstants.NOTIFICATION_FCM_ID, notificationIntent, PendingIntentFlags.OneShot);
            return pendingIntent;
        }

        //void SendNotification(string messageTitle, string messageBody, IDictionary<string, string> data)
        //{
        //    var notificationBuilder = new NotificationCompat.Builder(this, "fcm_channel")
        //        .SetContentTitle(messageTitle)
        //        .SetContentText(messageBody)
        //        .SetContentIntent(BuildIntentToShowMainActivity())
        //        .SetSmallIcon(Resource.Drawable.pill)
        //        .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
        //        .SetAutoCancel(true);

        //    Notification notification = notificationBuilder.Build();
        //    NotificationManager manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);
        //    manager.Notify(100, notification);
        //}

        ///// <summary>
        ///// действие на переход в приложение
        ///// </summary>
        ///// <returns></returns>
        //PendingIntent BuildIntentToShowMainActivity()
        //{
        //    Intent notificationIntent = new Intent(this, typeof(MainActivity))
        //        .AddFlags(ActivityFlags.ClearTop)
        //        .SetAction("incoServiceAdminMobileApp.action.MAIN_ACTIVITY")
        //        .SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask)
        //        .PutExtra("notif", "openMain")
        //        .PutExtra("id", "100"); ;

        //    PendingIntent pendingIntent = PendingIntent.GetActivity(this, 100, notificationIntent, PendingIntentFlags.OneShot);
        //    return pendingIntent;
        //}
    }
}
