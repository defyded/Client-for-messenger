using Client_For_Messenger.ViewModels;

namespace Client_For_Messenger;

public partial class ChatPage : ContentPage
{
    private readonly ChatMessagesViewModel _chatMessagesViewModel;
    public ChatPage(ChatMessagesViewModel chatMessagesViewModel)
    {
        InitializeComponent();
        _chatMessagesViewModel = chatMessagesViewModel;
        BindingContext = _chatMessagesViewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _chatMessagesViewModel.LoadChatMessagesAsync();
    }
}