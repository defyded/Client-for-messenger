#if ANDROID
using Android.Net.Wifi.Hotspot2;
#endif
using Client_For_Messenger.DTOs;
using Client_For_Messenger.Models;
using Client_For_Messenger.Services;

namespace Client_For_Messenger
{
    public partial class LoginPage : ContentPage
    {
        private readonly AuthService _authService;
        private readonly TokenStore _tokenStore;
        public LoginPage(AuthService authService, TokenStore tokenStore)
        {
            InitializeComponent();
            _authService = authService;
            _tokenStore = tokenStore;
            BindingContext = new LoginModel();
        }

        private async void OnSendClicked(object? sender, EventArgs e)
        {
            StatusLabel.Text = "";
            var email = LoginTb.Text?.Trim() ?? "";
            var pass = PasswordTb.Text ?? "";
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
            {
                StatusLabel.Text = "Введите email и пароль";
                return;
            }
            ((Button)sender).IsEnabled = false;
            try
            {
                var result = await _authService.LoginAsync(new LoginRequest(email, pass));
                if (!result.IsSuccess || result.Data is null)
                {
                    StatusLabel.Text = result.ErrorMessage ?? "Ошибка входа";
                    return;
                }

                await _tokenStore.SaveAsync(new StoredToken(result.Data.AccessToken, result.Data.ExpiresAtUtc), result.Data.id);
                await Shell.Current.GoToAsync("//home");
            }
            finally
            {
                ((Button)sender).IsEnabled = true;
            }
        }
        private async void OpenRegisterPage(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//register");
        }
    }
}
