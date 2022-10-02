using Firebase.Messaging;
using ZillPillMobileApp.MVVM.ViewModel;

namespace ZillPillMobileApp.MVVM.View.AboutPages;

public partial class UserPage : ContentPage
{
    private UserPageViewModel _viewModel;

    public UserPage()
    {
        InitializeComponent();

        BindingContext = _viewModel = new UserPageViewModel();

        SystemNotifSwitch.IsToggled = Preferences.Default.Get("SystemNotifSwitch", true);
        ShedullerNotifSwitch.IsToggled = Preferences.Default.Get("ShedullerNotifSwitch", true);
    }

    private void SystemNotifSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        if (SystemNotifSwitch.IsToggled)
            FirebaseMessaging.Instance.SubscribeToTopic($"system");
        else
            FirebaseMessaging.Instance.UnsubscribeFromTopic($"system");

        Preferences.Default.Set("SystemNotifSwitch", SystemNotifSwitch.IsToggled);
    }

    private async void ShedullerNotifSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var phone = SecureStorage.GetAsync("phone");
        var phoneRes = phone.Result;

        if (SystemNotifSwitch.IsToggled)
            FirebaseMessaging.Instance.SubscribeToTopic($"sheduller_{phoneRes}");
        else
            FirebaseMessaging.Instance.UnsubscribeFromTopic($"sheduller_{phoneRes}");

        Preferences.Default.Set("ShedullerNotifSwitch", ShedullerNotifSwitch.IsToggled);
    }
}