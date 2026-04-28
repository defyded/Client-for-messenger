using Client_For_Messenger.DTOs;
using Client_For_Messenger.Models;
using Client_For_Messenger.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Xml;

namespace Client_For_Messenger.ViewModels;

public partial class HomeViewModel : ObservableObject
{
	private readonly ChatService _chatService;
    private readonly UserService _userService;
    public ObservableCollection<ChatModel> Chats { get; } = new();
    public ObservableCollection<UserModel> Users { get; } = new();

    [ObservableProperty]
    private UserModel? _SelectedUser;

    [ObservableProperty]
    private bool _IsRefreshing;

    [ObservableProperty]
    private bool _IsSearching;

    [ObservableProperty]
    private bool _IsBusy;

    public HomeViewModel(ChatService chatService, UserService userService) 
    { 
        _chatService = chatService;
        _userService = userService;
    }
    [RelayCommand]
    public async Task LoadChatsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var dtos = await _chatService.GetChatsAsync();

            Chats.Clear();
            foreach (var dto in dtos)
            {
                Chats.Add(new ChatModel
                {
                    Id = dto.ChatId,
                    CompanionUsername = dto.CompanionUsername,
                    AvatarURL = dto.AvatarURL,
                    LastMessage = dto.LastMessage
                });
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("DEBUG ERROR", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing= false;
        }
    }
    [RelayCommand]
    public async Task SearchUsers(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            IsSearching = false;
            Users.Clear();
            return;
        }

        try
        {
            IsSearching = true;
            var response = await _userService.SearchUsers(query);

            Users.Clear();
            if (response?.Users != null)
            {
                foreach (var dto in response.Users)
                {
                    Users.Add(new UserModel
                    {
                        UserId = dto.userId,
                        AvatarUrl = dto.avatarUrl,
                        Username = dto.username,
                    });
                            
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("DEBUG ERROR", ex.Message, "OK");
        }
    }
    public async Task<ChatDto> ChatCreate(Guid CompanionId)
    {
        return await _chatService.CreateChatAsync(CompanionId);
    }
}
