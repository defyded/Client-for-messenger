using Client_For_Messenger.DTOs;
using Client_For_Messenger.Models;
using Client_For_Messenger.Services;
using Client_For_Messenger.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Client_For_Messenger;

public partial class HomePage : ContentPage 
{
	private readonly HomeViewModel _homeViewModel;
    private CancellationTokenSource? _searchCts;
    public HomePage(HomeViewModel homeViewModel)
	{
		InitializeComponent();
		_homeViewModel = homeViewModel;
		BindingContext = _homeViewModel;
	}
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _searchCts?.Cancel();
        //_searchCts?.Dispose();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
		await _homeViewModel.LoadChatsAsync();
    }
    private async void SearchHandler(object sender, TextChangedEventArgs e)
    {
        var query = e.NewTextValue?.Trim();

        _searchCts?.Cancel();
        _searchCts = new CancellationTokenSource();
        var token = _searchCts.Token;

        if (string.IsNullOrWhiteSpace(query))
        {
            _homeViewModel.IsSearching = false;
            _homeViewModel.Users.Clear();
            return;
        }

        try
        {
            await Task.Delay(500, token);

            await _homeViewModel.SearchUsersCommand.ExecuteAsync(query);
        }
        catch (OperationCanceledException)
        {
            
        }
    }
    private async void OnChatCreated(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is UserModel selectedUser)
        {
            try
            {
                var res = await _homeViewModel.ChatCreate(selectedUser.UserId);
                await Shell.Current.GoToAsync($"chat?id={res.ChatId}&name={res.CompanionUsername}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("DEBUG ERROR", ex.Message, "OK");
            }
        }
    }
    private async void OnChatSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is ChatModel selectedChat)
        {
            ((CollectionView)sender).SelectedItem = null;

            await Shell.Current.GoToAsync($"chat?id={selectedChat.Id}&name={selectedChat.CompanionUsername}");
        }
    }
    private async void Logout(object sender, EventArgs e)
    {
        SecureStorage.Default.RemoveAll();
        await Shell.Current.GoToAsync("//login");
    }
}
    