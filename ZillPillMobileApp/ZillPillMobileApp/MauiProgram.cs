using CommunityToolkit.Maui;
using Microsoft.Maui.LifecycleEvents;

namespace ZillPillMobileApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            //.RegisterFirebaseServices()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Nunito-Regular.ttf", "NunitoRegular");
                fonts.AddFont("Nunito-SemiBold.ttf", "NunitoSemiBold");
            });

        // builder.Services.AddSingleton(typeof(IFingerprint), CrossFingerprint.Current);

        return builder.Build();
    }

    //    private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
    //    {
    //        builder.ConfigureLifecycleEvents(events =>
    //        {
    //#if IOS
    //            events.AddiOS(iOS => iOS.FinishedLaunching((app, launchOptions) => {
    //                CrossFirebase.Initialize(app, launchOptions, CreateCrossFirebaseSettings());
    //                return false;
    //            }));
    //#else
    //            events.AddAndroid(android => android.OnCreate((activity, state) =>
    //                CrossFirebase.Initialize(activity, state, CreateCrossFirebaseSettings())));
    //#endif
    //        });

    //        builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);
    //        return builder;
    //    }
}
