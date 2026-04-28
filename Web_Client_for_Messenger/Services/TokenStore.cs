#if ANDROID
using Android.Accessibilityservice.AccessibilityService;
#endif
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Client_For_Messenger.Services
{
    public record StoredToken(string AccessToken, DateTimeOffset ExpiredAt);

    public sealed class TokenStore
    {
        private readonly IJSRuntime _js; // Инструмент для работы с браузером
        private const string _key = "auth_token_v1";
        private const string _userIdKey = "user_id_v1";

        public TokenStore(IJSRuntime js)
        {
            _js = js;
        }

        public static string Key => _key;
        public static string UserId => _userIdKey;

        public async Task SaveAsync(StoredToken token, Guid userId)
        {
            var json = JsonSerializer.Serialize(token);
            await _js.InvokeVoidAsync("localStorage.setItem", _key, json);
            await _js.InvokeVoidAsync("localStorage.setItem", _userIdKey, userId.ToString());
        }

        public async Task<StoredToken?> GetAsync()
        {
            var json = await _js.InvokeAsync<string>("localStorage.getItem", _key);
            if (string.IsNullOrWhiteSpace(json))
                return null;
            try
            {
                return JsonSerializer.Deserialize<StoredToken>(json);
            }
            catch
            {
                return null;
            }
        }

        public async Task ClearAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", _key);
            await _js.InvokeVoidAsync("localStorage.removeItem", _userIdKey);
        }

        //public static bool IsValid(StoredToken token, TimeSpan? skew = null)
        //{
        //    var buffer = skew ?? TimeSpan.FromMinutes(1);
        //    // Заменяем File.AppendAllText (которого нет в вебе) на консоль браузера
        //    Console.WriteLine($"Token check - ExpiredAt: {token.ExpiredAt}, Now: {DateTimeOffset.UtcNow}");
        //    return token.ExpiredAt > DateTimeOffset.UtcNow.Add(buffer);
        //}
    }
}
