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
	public HomePage(HomeViewModel homeViewModel)
	{
		InitializeComponent();
		_homeViewModel = homeViewModel;
		BindingContext = _homeViewModel;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
		await _homeViewModel.LoadChatsAsync();
    }
    private async void OnChatSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is ChatModel selectedChat)
        {
            ((CollectionView)sender).SelectedItem = null;

            // todo сделать переход на страницу чата await Shell.Current.GoToAsync($"chat?id={selectedChat.}");
        }
    }
}
