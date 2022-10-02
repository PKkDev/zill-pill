using ZillPillMobileApp.MVVM.Model;

namespace ZillPillMobileApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        MessagingCenter.Subscribe<ErrorMessage>(this, "Error", (message) =>
        {
            DisplayAlert("Error", message.Message, "ok");
        });
    }

    private async void LogOut_Clicked(object sender, EventArgs e)
    {
        SecureStorage.RemoveAll();
        await Current.GoToAsync("//LogInPage");
    }
}
