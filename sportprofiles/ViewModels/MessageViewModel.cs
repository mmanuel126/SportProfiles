using sportprofiles.Services;
using sportprofiles.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using sportprofiles.Views.Message;

namespace sportprofiles.ViewModels
{
    public class MessageViewModel : INotifyPropertyChanged
    {
        public ICommand RefreshCommand { get; set; }
        public Command <MessageInfoModel> DropCommand { get; set; }
        public Command<MessageInfoModel> OpenCommand { get; set; }
        
        bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;

            set
            {
                isRefreshing = value;

                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        List<MessageInfoModel> _Messages;
        public List<MessageInfoModel> Messages
        {
            get
            {
                return _Messages;
            }
            set
            {
                _Messages = value;
                OnPropertyChanged();
            }
        }

        private readonly IMessages _messagesSvc; 

        public MessageViewModel(IMessages messagesSvc)
        {
            _messagesSvc = messagesSvc;
            IsRefreshing = true;
            DropCommand = new  Command<MessageInfoModel>  (OnDropMessage);
            OpenCommand = new Command<MessageInfoModel>(OnOpenMessage);
            RefreshCommand = new Command (OnRefreshCommand);
            //_connectionsSvc = new ContactsModel();
            Messages = new List<MessageInfoModel>();
            Task.Run(() => GetMessagesAsync().Wait());
            IsRefreshing=false;
        }

        async void OnDropMessage(MessageInfoModel message)
        {
            await _messagesSvc.DeleteMessage(message.MessageID!, "");
            await DoRefresh();
        }

        async void OnOpenMessage(MessageInfoModel message)
        {
            Preferences.Set("MessageID", message.MessageID);
            Preferences.Set("SenderID", message.FromID);
            await  Application.Current.MainPage.Navigation.PushModalAsync(new MessageDetailsPage(new MessageDetailsViewModel(new sportprofiles.Services.Messages())));
        }

        async private void OnRefreshCommand() => await DoRefresh();

         async Task DoRefresh()
        {
            Messages.Clear();
            Messages = new List<MessageInfoModel>();
            IsRefreshing = true;
            await GetMessagesAsync();
            IsRefreshing = false;
        }

        async Task GetMessagesAsync()
        {
            List<MessageInfoModel> result = await _messagesSvc.GetMemberMessages("Inbox", "All");
            Messages = result;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
