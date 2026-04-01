using Client_For_Messenger.DTOs;
using Client_For_Messenger.Models;
using Client_For_Messenger.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Client_For_Messenger.ViewModels;

public partial class HomeViewModel : ObservableObject
{
	private readonly ChatService _chatService;
    public ObservableCollection<ChatModel> Chats { get; } = new();

    [ObservableProperty]
    private bool _IsRefreshing;
    [ObservableProperty]
    private bool _IsBusy;
    public HomeViewModel(ChatService chatService) => _chatService = chatService;
    [RelayCommand]
    public async Task LoadChatsAsync()
    {
        if (_IsBusy) return;

        try
        {
            _IsBusy = true;
            var dtos = await _chatService.GetChatsAsync();

            Chats.Clear();
            foreach (var dto in dtos)
            {
                // Превращаем DTO от сервера в красивую Model для UI
                Chats.Add(new ChatModel
                {
                    CompanionUsername = dto.CompanionUsername,
                    AvatarURL = dto.AvatarURL,
                    LastMessage = dto.LastMessage
                });
            }
        }
        catch (Exception ex)
        {
            // ВЫВЕДИТЕ ТЕКСТ ОШИБКИ, А НЕ СВОЙ ТЕКСТ
            await Shell.Current.DisplayAlert("DEBUG ERROR", ex.Message, "OK");
        }
        finally
        {
            _IsBusy = false;
            _IsRefreshing = false;
        }
    }

}
