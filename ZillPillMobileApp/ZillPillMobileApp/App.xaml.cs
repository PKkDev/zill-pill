using ZillPillMobileApp.Infrastructure.Services;

namespace ZillPillMobileApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        DependencyService.Register<UserDataService>();
        DependencyService.Register<MedicalProductDataService>();
        DependencyService.Register<CountriesDataService>();

        string lang = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU");
        string lang2 = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

        MainPage = new AppShell();
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnResume()
    {
        base.OnResume();
    }

    protected override void OnSleep()
    {
        base.OnSleep();
    }
}
