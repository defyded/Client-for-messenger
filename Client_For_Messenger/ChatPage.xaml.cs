using Client_For_Messenger.ViewModels;

namespace Client_For_Messenger;

public partial class ChatPage : ContentPage
{
    private readonly ChatMessagesViewModel _viewModel;

    public ChatPage(ChatMessagesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;

        _viewModel.ChatMessages.CollectionChanged += (s, e) =>
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                Dispatcher.Dispatch(async () =>
                {
                    await Task.Delay(100); 
                    var lastItem = _viewModel.ChatMessages.LastOrDefault();
                    if (lastItem != null)
                    {
                        MessageList.ScrollTo(lastItem, position: ScrollToPosition.End, animate: true);
                    }
                });
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (Guid.TryParse(_viewModel.ChatId, out var guidId))
        {
            await _viewModel.LoadChatMessagesAsync();

            await _viewModel.ConnectHubAsync(guidId);
        }
    }

    protected async void Send()
    {
        await _viewModel.SendMessageAsync();
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();

        if (Guid.TryParse(_viewModel.ChatId, out var guidId))
        {
            await _viewModel.DisconnectHubAsync(guidId);
        }
    }
}