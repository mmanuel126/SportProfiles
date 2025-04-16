using System.ComponentModel;
using System.Runtime.CompilerServices;
using sportprofiles.Models;
using sportprofiles.Services;

namespace sportprofiles.ViewModels
{
    public class MessageDetailsViewModel : INotifyPropertyChanged
    {
        MessageDetails _messageDetail;

        public MessageDetails MessageDetails
        {
            get { return _messageDetail; }
            set { _messageDetail = value; OnPropertyChanged(); }
        }

        private string messageText;
        public string MessageText
        {
            get { return messageText; }
            set
            {
                messageText = value;
                OnPropertyChanged("MessageText");
            }
        }

        private bool isLoading = false;
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                this.isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        private readonly IMessages _messagesSvc;
        public MessageDetailsViewModel(IMessages messagesSvc)
        {
            _messagesSvc = messagesSvc;
            Task.Run(() => GetMessageDetails());
        }
       
        public async Task SendMessage(MessageDetails msgData)
        {
            IsLoading = true;
            string memberID = Preferences.Default.Get("UserID", "");
            await _messagesSvc.SendMessage(memberID, msgData.SenderID!, msgData.Subject!, msgData.Body!);
            IsLoading = false;
        }
       
        public async Task GetMessageDetails()
        {
            IsLoading = true;
            string messageID = Preferences.Get("MessageID", "");
            string senderID = Preferences.Get("SenderID", "");
            var pcInfoLst = await _messagesSvc.GetMessageInfoByID(messageID, "Inbox");
            if (pcInfoLst != null && pcInfoLst.Count != 0)
            {
                MessageDetails = pcInfoLst[0];
                MessageDetails.SenderID = senderID;
            }
            IsLoading = false;
        }

        public async void LogException(string msg, string stackTrace, string jwt)
        {
            await _messagesSvc.LogException(msg, stackTrace, jwt);
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChangedEventHandler?handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
