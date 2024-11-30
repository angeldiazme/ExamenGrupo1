using Microsoft.Extensions.Logging;
using Microsoft.Maui.Maps.Handlers;
using Plugin.Maui.Audio;
using Syncfusion.Maui.Core.Hosting;

namespace Examen_Grupo2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddHandler(typeof(Map), typeof(MapHandler));
                })
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton(AudioManager.Current);
            builder.Services.AddTransient<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
