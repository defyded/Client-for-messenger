using Client_For_Messenger.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Client_For_Messenger.Services
{
    public class AuthService
    {
        private readonly ApiService _apiService; 
        private readonly TokenStore _tokenStore;

        public AuthService(ApiService apiService, TokenStore tokenStore)
        {
            _apiService = apiService;
            _tokenStore = tokenStore;
        }

        public async Task<ApiResult<LoginResponse>> LoginAsync(LoginRequest req, CancellationToken ct = default) => await PerformAction(Login, req, ct);
        public async Task<ApiResult<LoginResponse>> RegisterAsync(RegisterRequest req, CancellationToken ct = default) => await PerformAction(Register, req, ct);
        private async Task<ApiResult<LoginResponse>> Login(LoginRequest req, CancellationToken ct = default)
        {
            using var resp = await _apiService.RequestPost("api/auth/login", req);
            if (!resp.IsSuccessStatusCode)
            {
                var serverText = await resp.Content.ReadAsStringAsync(ct);
                var msg = string.IsNullOrWhiteSpace(serverText)
                    ? $"HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}"
                    : serverText;

                return new ApiResult<LoginResponse>(false, null, msg, resp.StatusCode);
            }
            var data = await resp.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: ct);
            if (data is null)
            {
                await _tokenStore.SaveAsync(new StoredToken(data.AccessToken, data.ExpiresAtUtc), data.id);
                return new ApiResult<LoginResponse>(false, null, "Пустой ответ сервера", resp.StatusCode);
            }

            return new ApiResult<LoginResponse>(true, data, null, resp.StatusCode);
        }
        private async Task<ApiResult<LoginResponse>> Register(RegisterRequest req, CancellationToken ct = default)
        {
            using var resp = await _apiService.RequestPost("api/auth/register", req);
            if (!resp.IsSuccessStatusCode)
            {
                var serverText = await resp.Content.ReadAsStringAsync(ct);
                var msg = string.IsNullOrWhiteSpace(serverText)
                    ? $"HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}"
                    : serverText;

                return new ApiResult<LoginResponse>(false, null, msg, resp.StatusCode);
            }
            var data = await resp.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: ct);
            if (data is null)
                return new ApiResult<LoginResponse>(false, null, "Пустой ответ сервера", resp.StatusCode);

            return new ApiResult<LoginResponse>(true, data, null, resp.StatusCode);
        }
        private async Task<ApiResult<LoginResponse>> PerformAction<T>(Func<T, CancellationToken, Task<ApiResult<LoginResponse>>>  Action, T req, CancellationToken ct)
        {
            try
            {
                return await Action.Invoke(req, ct);
            }
            catch (TaskCanceledException)
            {
                return new ApiResult<LoginResponse>(false, null, "Таймаут/отмена запроса", null);
            }
            catch (HttpRequestException ex)
            {
                return new ApiResult<LoginResponse>(false, null, $"Сетевая ошибка: {ex.Message}", null);
            }
            catch (Exception ex)
            {
                return new ApiResult<LoginResponse>(false, null, $"Неожиданная ошибка: {ex.Message}", null);
            }
        }
        //public async Task<string> LoginRequest(string Email, string Password)
        //{
        //    var responce = await _apiService.RequestPost("/api/auth/login", new {
        //        Email = Email,
        //        Password = Password
        //    });
        //    if (!responce.IsSuccessStatusCode)
        //    {
        //        throw new LoginException("LOGIN_UNSUCCESFUL","login is unsuccesful"); //ToDo сделать кастомные ошибки
        //    }
        //    var result = await responce.Content.ReadFromJsonAsync<ResponceFromServer>();

        //    return result.Token;
        //}
        public sealed class LoginException : Exception
        {
            public string Code { get; }

            public LoginException(string code, string message) : base(message)
                => Code = code;
        }
        private class ResponceFromServer
        {
            public string Token { get; set; }
            public string Username { get; set; }
            public DateTime ExpiresAtUtc { get; set; }
            public Guid UserId{ get; set; }
        }
    }
}
