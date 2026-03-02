using Client_For_Messenger.WinUI;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Client_For_Messenger.Services
{
    public class AuthService
    {
        private ApiService _apiService;

        public AuthService()
        {
            _apiService = new ApiService();
        }

        public async Task<string> LoginRequest(string Email, string Password)
        {
            var responce = await _apiService.RequestPost("/api/auth/login", new {
                Email = Email,
                Password = Password
            });
            if (!responce.IsSuccessStatusCode)
            {
                throw new Exception(); //ToDo сделать кастомные ошибки
            }
            var result = await responce.Content.ReadFromJsonAsync<ResponceFromServer>();

            return result.Token;
        }

        private class ResponceFromServer
        {
            public string Token { get; set; }
            public string Username { get; set; }
            public DateTime ExpiresAtUtc { get; set; }
        }
    }
}
