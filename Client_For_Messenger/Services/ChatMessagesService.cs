using Client_For_Messenger.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Client_For_Messenger.Services
{
    public class ChatMessagesService
    {
        private readonly ApiService _apiService;
        public ChatMessagesService(ApiService apiService) => _apiService = apiService;

        public async Task<List<ChatMessageDto>> GetChatsMessagesAsync(Guid chatId)
        {
            try
            {
                var response = await _apiService.RequestGet($"api/chats/{chatId}/messages");

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return await response.Content.ReadFromJsonAsync<List<ChatMessageDto>>(options) ?? new();
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
}
