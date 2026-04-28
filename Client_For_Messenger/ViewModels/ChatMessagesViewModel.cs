using Client_For_Messenger.DTOs;
using Client_For_Messenger.Models;
using Client_For_Messenger.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Client_For_Messenger.ViewModels
{
    [QueryProperty(nameof(ChatId), "id")]
    [QueryProperty(nameof(CompanionName), "name")]
    public partial class ChatMessagesViewModel : ObservableObject
    {
        private readonly ChatMessagesService _chatMessagesService ;
        [ObservableProperty]
        private string _chatId; 

        [ObservableProperty]
        private string _companionName;

        
        [ObservableProperty] 
        private string _lastSeenStatus = "в сети";
        [ObservableProperty] 
        private string _newMessageText;
        [ObservableProperty] 
        private bool _isRefreshing;
        [ObservableProperty] 
        private bool _isBusy;

        private string _cachedUserId;
        public ObservableCollection<ChatMessagesModel> ChatMessages { get; } = new();

        public ChatMessagesViewModel(ChatMessagesService chatMessagesService)
        {
            _chatMessagesService = chatMessagesService;
            _chatMessagesService.OnMessageReceived += OnNewMessageReceived;
        }

        [RelayCommand]
        public async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(NewMessageText))
                return;

            if (string.IsNullOrEmpty(ChatId))
                return;

            try
            {
                var textToSend = NewMessageText;
                NewMessageText = string.Empty;

                await _chatMessagesService.SendMessageAsync(Guid.Parse(ChatId), textToSend);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("DEBUG ERROR", ex.Message, "OK");
            }
        }

        [RelayCommand]
        public async Task LoadChatMessagesAsync()
        {
            if (_isBusy) return;
            if (string.IsNullOrWhiteSpace(ChatId) || ChatId == Guid.Empty.ToString())
                return;
            try
            {
                _isBusy = true;
                var guidId = Guid.Parse(ChatId);

                var dtos = await _chatMessagesService.GetChatsMessagesAsync(guidId);


                ChatMessages.Clear();
                _cachedUserId =  await SecureStorage.GetAsync(TokenStore.UserId);
                foreach (var dto in dtos)
                {
                    ChatMessages.Add(new ChatMessagesModel
                    {
                        Message = dto.Content,
                        SendAt = dto.CreatedAt.ToString("HH:mm"),
                        FlowDirection = string.Equals(dto.SenderId.ToString(), _cachedUserId, StringComparison.OrdinalIgnoreCase) ? "End" : "Start"
                    });
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("DEBUG ERROR", ex.Message, "OK");
            }
            finally
            {
                _isBusy = false;
                _isRefreshing = false;
            }
        }

        private async void OnNewMessageReceived(ChatMessageDto dto)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {

                bool isMine = string.Equals(dto.SenderId.ToString(), _cachedUserId, StringComparison.OrdinalIgnoreCase);

                if (ChatMessages.Any(m => m.Message == dto.Content && m.SendAt == dto.CreatedAt.ToString("HH:mm")))
                    return;

                ChatMessages.Add(new ChatMessagesModel
                {
                    Message = dto.Content,
                    SendAt = dto.CreatedAt.ToString("HH:mm"),
                    FlowDirection = isMine ? "End" : "Start"
                });
            });
        }


        public async Task ConnectHubAsync(Guid chatId)
        {
            try { await _chatMessagesService.ConnectAsync(chatId); }
            catch (Exception ex) { Shell.Current.DisplayAlert("DEBUG ERROR", ex.Message, "OK"); }
        }

        public async Task DisconnectHubAsync(Guid chatId)
        {
            try { await _chatMessagesService.DisconnectAsync(chatId); }
            catch (Exception ex) { Shell.Current.DisplayAlert("DEBUG ERROR", ex.Message, "OK"); }
        }

    }
}
