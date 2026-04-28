using Client_For_Messenger.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Client_For_Messenger
{
    public partial class App : Application
    {
        public static string Path = "C:\\Users\\akou0\\source\\repos\\Client_For_Messenger\\Client_For_Messenger\\log.txt";
        private readonly TokenStore _tokenStore;
        private readonly ILogger<App> _logger;
        public App(TokenStore tokenStore, ILogger<App> logger, AppShell shell)
        {
            InitializeComponent();
            _tokenStore = tokenStore;
            _logger = logger;

            MainPage = shell;

            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("register", typeof(RegisterPage));
            Routing.RegisterRoute("home", typeof(HomePage));
            Routing.RegisterRoute("chat", typeof(ChatPage));

        }

        protected override Window CreateWindow(IActivationState? activationState)
        {

            var window = base.CreateWindow(activationState);

            window.Dispatcher.Dispatch(async () =>
            {
                await InitializeAsync();
            });

            return window;
        }
        private async Task InitializeAsync()
        {
            //await Task.Delay(100);
            try
            {
                var token = await _tokenStore.GetAsync();
                if (token is not null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        if (Shell.Current != null)
                            Shell.Current.GoToAsync("//home"); 
                    });
                }
                else
                {
                    _logger.LogError(token is not null ? "token exsists no valid" : "token doesnt exsist");
                    File.AppendAllText(Path, token is not null ? "token exsists no valid" : "token doesnt exsist");
                    if (token is not null)
                        await _tokenStore.ClearAsync();

                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        if (Shell.Current != null)
                            Shell.Current.GoToAsync("//login");
                    });

                }
            }
            catch(Exception ex)
            {
                //var errorMessage = $"Ошибка инициализации: {ex.Message} \n {ex.StackTrace}";
                //_logger.LogError(errorMessage);
                //File.AppendAllText(Path, errorMessage);
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    if (Shell.Current != null)
                        Shell.Current.GoToAsync("//login");
                });
            }
        }
    }
}