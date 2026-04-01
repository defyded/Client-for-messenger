using Client_For_Messenger.DTOs;
using Client_For_Messenger.Models;
using Client_For_Messenger.Services;

namespace Client_For_Messenger;

public partial class RegisterPage : ContentPage
{
    private readonly AuthService _authService;
    private readonly TokenStore _tokenStore;
    public RegisterPage(AuthService authService, TokenStore tokenStore)
	{
		InitializeComponent();
        _authService = authService;
        _tokenStore = tokenStore;
        BindingContext = new RegisterModel();
    }

    private async void OnSendClicked(object? sender, EventArgs e)
    {
        StatusLabel.Text = "";
        var email = EmailTb.Text?.Trim() ?? "";
        var username = UsernameTb.Text?.Trim() ?? "";
        var pass = PasswordTb.Text ?? "";
        var passConfirm = PasswordConfirmTb.Text ?? "";
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(passConfirm))
        {
            StatusLabel.Text = "Введите все данные";
            return;
        }
        if (pass != passConfirm)
        {
            StatusLabel.Text = "пароли разные";
            return;
        }
        ((Button)sender).IsEnabled = false;
        try
        {
            var result = await _authService.RegisterAsync(new RegisterRequest(email, username, pass));
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

    private async void OpenLoginPage(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//login");
    }
}