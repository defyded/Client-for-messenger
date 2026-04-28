using Client_For_Messenger.DTOs;
using Microsoft.AspNetCore.SignalR.Client;
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
        private HubConnection _hubConnection;
        public event Action<ChatMessageDto> OnMessageReceived;
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
        public async Task ConnectAsync(Guid chatId)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7007/chathub") // URL вашего хаба
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<ChatMessageDto>("ReceiveMessage", (msg) => OnMessageReceived?.Invoke(msg));

            await _hubConnection.StartAsync();
            await _hubConnection.InvokeAsync("JoinChat", chatId); // Входим в группу
        }

        public async Task SendMessageAsync(Guid chatId, string text)
        {
            var content = new { Content = text };
            await _apiService.RequestPost($"api/chats/{chatId}/messages", content);
        }

        public async Task DisconnectAsync(Guid chatId)
        {
            if (_hubConnection == null) return;
            await _hubConnection.InvokeAsync("LeaveChat", chatId);
            await _hubConnection.StopAsync();
        }
    }
}
