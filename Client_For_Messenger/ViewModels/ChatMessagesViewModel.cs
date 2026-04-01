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
    public partial class ChatMessagesViewModel : ObservableObject
    {
        private readonly ChatMessagesService _chatMessagesService ;
        private readonly Guid _chatId;
        public ObservableCollection<ChatMessagesModel> ChatMessages { get; } = new();

        [ObservableProperty]
        private bool _IsRefreshing;
        [ObservableProperty]
        private bool _IsBusy;
        public ChatMessagesViewModel(ChatMessagesService chatMessagesService, Guid chatId)
        {
            _chatMessagesService = chatMessagesService;
            _chatId = chatId;
        }
        
        [RelayCommand]
        public async Task LoadChatMessagesAsync()
        {
            if (_IsBusy) return;

            try
            {
                _IsBusy = true;
                var dtos = await _chatMessagesService.GetChatsMessagesAsync(_chatId);


                ChatMessages.Clear();
                var userId = SecureStorage.GetAsync(TokenStore.UserId).Result;
                foreach (var dto in dtos)
                {
                    ChatMessages.Add(new ChatMessagesModel
                    {
                        Message = dto.Content,
                        SendAt = dto.CreatedAt.ToString("HH:mm"),
                        FlowDirection = dto.SenderId.ToString() == userId ? "End" : "Start" 
                    });
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("DEBUG ERROR", ex.Message, "OK");
            }
            finally
            {
                _IsBusy = false;
                _IsRefreshing = false;
            }
        }

    }
}
