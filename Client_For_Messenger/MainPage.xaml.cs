using Client_For_Messenger.Models;

namespace Client_For_Messenger
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new LoginModel();
        }

        private void OnSendClicked(object? sender, EventArgs e)
        {
            
        }
    }
}
