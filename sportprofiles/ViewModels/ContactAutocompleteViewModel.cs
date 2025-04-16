using System.Collections.ObjectModel;
using System.ComponentModel;
using sportprofiles.Models.Contacts;

namespace sportprofiles.ViewModels
{
    public class ContactAutocompleteViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ContactsModel> _Contacts;
        public ObservableCollection<ContactsModel> Contacts
        {
            get
            {
                return _Contacts;
            }
            set
            {
                _Contacts = value;
                RaisePropertyChanged("");
            }
        }

        private string messageText;
        public string MessageText
        {
            get { return messageText; }
            set
            {
                messageText = value;
                RaisePropertyChanged("MessageText");
            }
        }

        private string subjectText;
        public string SubjectText
        {
            get { return subjectText; }
            set
            {
                subjectText = value;
                RaisePropertyChanged("SubjectText");
            }
        }

        private bool isLoading = false;
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                this.isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }

        public ContactAutocompleteViewModel()
        {
            GetContactsAsync();
        }

        public async Task SendMessage(string contactID, string msg, string subject)
        {
            IsLoading = true;
            sportprofiles.Services.Messages msgSvc = new sportprofiles.Services.Messages();
            string memberID = "0";
            if (Preferences.Get("UserID", "") != null)
            {
                memberID = Preferences.Get("UserID", "").ToString();
            }
            await msgSvc.SendMessage(memberID, contactID, subject, msg);
            IsLoading = false;
        }

        async Task GetContactsAsync()
        {
            sportprofiles.Services.Contacts svc = new();
            ObservableCollection<ContactsModel> result = await svc.GetMyContactsList();
            this.Contacts = result;
        }

        public async void LogException(string msg, string stackTrace, string jwt)
        {
            sportprofiles.Services.Contacts svc = new();
            await svc.LogException(msg, stackTrace, jwt);
        }        

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void RaisePropertyChanged(String name)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
