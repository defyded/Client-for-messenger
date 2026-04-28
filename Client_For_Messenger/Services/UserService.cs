using Client_For_Messenger.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Client_For_Messenger.Services
{
    public class UserService
    {
        private readonly ApiService _apiService;
        public UserService(ApiService apiService) => _apiService = apiService;

        public async Task<SearchUsersResponce> SearchUsers(string query)
        {
            try
            {
                var response = await _apiService.RequestGet($"api/users/search?query={Uri.EscapeDataString(query)}");

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var usersList = await response.Content.ReadFromJsonAsync<List<UserDto>>(options);

                    if (usersList == null)
                    {
                        throw new UserException("NOT_LOADED", "users are not loaded");
                    }
                    return new SearchUsersResponce(usersList);
                }

                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Сервер ответил: {response.StatusCode}. {errorBody}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка связи: {ex.Message}");
            }
        }
    }
    public sealed class UserException : Exception
    {
        public string Code { get; }

        public UserException(string code, string message) : base(message)
            => Code = code;
    }
}
