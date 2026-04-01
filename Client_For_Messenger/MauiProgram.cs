#if ANDROID
using AndroidX.Browser.Trusted;
#endif

using Client_For_Messenger.Services;
using Client_For_Messenger.ViewModels;
using Microsoft.Extensions.Logging;

namespace Client_For_Messenger
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>();
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoSystemBorders", (handler, view) =>
            {
#if ANDROID
        handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif WINDOWS
        handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            });

            builder.Services.AddSingleton(_ =>
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                return new HttpClient(handler)
                {
                    BaseAddress = new Uri("https://localhost:7007/"), // Проверьте порт точно!
                    Timeout = TimeSpan.FromSeconds(10)
                };
            });
            builder.Services.AddSingleton<ApiService>();

            builder.Services.AddSingleton<TokenStore>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<ChatService>();

            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>(); 

            builder.Services.AddSingleton<AppShell>();
            return builder.Build();
        }
    }
}
