#if ANDROID
using Android.Accessibilityservice.AccessibilityService;
#endif
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Client_For_Messenger.Services
{
    public record StoredToken(string AccessToken, DateTimeOffset ExpiredAt);
    public sealed class TokenStore
    {
        private const string _key = "auth_token_v1";
        private const string _userIdKey = "user_id_v1";

        public static string Key { get 
            {
                return _key;
            } }
        public static string UserId
        {
            get
            {
                return _userIdKey;
            }
        }
        public async Task SaveAsync(StoredToken token, Guid userId)
        {
            var json = JsonSerializer.Serialize(token);
            await SecureStorage.Default.SetAsync(_key, json);
            await SecureStorage.Default.SetAsync(_userIdKey, userId.ToString());
        }
        public async Task<StoredToken?> GetAsync()
        {
            var json = await SecureStorage.Default.GetAsync(_key);
            if (String.IsNullOrWhiteSpace(json))
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
        public Task ClearAsync()
        {
            SecureStorage.Default.Remove(_key);
            return Task.CompletedTask;
        }

        //public static bool IsValid(StoredToken token, TimeSpan? skew = null)
        //{
        //    var buffer = skew ?? TimeSpan.FromMinutes(1);
        //    File.AppendAllText(App.Path, $"ExpiredAt: {token.ExpiredAt}");
        //    File.AppendAllText(App.Path, $"Now: {DateTimeOffset.UtcNow}");
        //    return token.ExpiredAt > DateTimeOffset.UtcNow.Add(buffer);
        //}
    }
}
