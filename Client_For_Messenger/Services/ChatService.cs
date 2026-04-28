using Client_For_Messenger.DTOs;
using Client_For_Messenger.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Client_For_Messenger.Services
{
    public partial class ChatService
    {
        private readonly ApiService _apiService;
        public ChatService(ApiService apiService) => _apiService = apiService;

        public async Task<List<ChatDto>> GetChatsAsync()
        {
            try
            {
                var response = await _apiService.RequestGet("api/chats");

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return await response.Content.ReadFromJsonAsync<List<ChatDto>>(options) ?? new();
                }

                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Сервер ответил: {response.StatusCode}. {errorBody}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка связи: {ex.Message}");
            }
        }
        public async Task<ChatDto> CreateChatAsync(Guid companionId)
        {
            var dto = new { CompanionId = companionId };
            var response = await _apiService.RequestPost("api/chats", dto); 

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return await response.Content.ReadFromJsonAsync<ChatDto>(options);
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new ChatException("CAN_NOT_CREATE_CHAT", "can not create chat");
        }
    }
    public sealed class ChatException : Exception
    {
        public string Code { get; }

        public ChatException(string code, string message) : base(message)
            => Code = code;
    }
}
