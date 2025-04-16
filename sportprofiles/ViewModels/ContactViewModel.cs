using sportprofiles.Models.Contacts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace sportprofiles.ViewModels
{
    public class ContactViewModel : INotifyPropertyChanged
    {
        public ICommand RefreshCommand { get; set; }
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

        public Command<ContactsModel> DropCommand { get; set; }
        public Command<ContactsModel> AcceptCommand { get; set; }
        public Command<ContactsModel> RejectCommand { get; set; }

        List<ContactsModel> _Contact;
        List<ContactsModel> _ContactRequests;

        public List<ContactsModel> Contact
        {
            get
            {
                return _Contact;
            }
            set
            {
                _Contact = value;
                OnPropertyChanged();
            }
        }

        public List<ContactsModel> ContactRequest
        {
            get
            {
                return _ContactRequests;
            }
            set
            {
                _ContactRequests = value;
                OnPropertyChanged();
            }
        }

        private readonly sportprofiles.Services.IContacts _conSvc;

        public ContactViewModel(sportprofiles.Services.IContacts conSvc)
        {
            IsRefreshing = true;
            RefreshCommand = new Command(OnRefreshCommandExecuted);
            IsRefreshing = false;
            DropCommand = new Command<ContactsModel>(OnDropContact);
            AcceptCommand = new Command<ContactsModel>(OnAcceptContact);
            RejectCommand = new Command<ContactsModel>(OnRejectContact);
            _conSvc = conSvc;
            Contact = new List<ContactsModel>();
            ContactRequest = new List<ContactsModel>();
            Task.Run(() => GetMyContactsAsync());
            Task.Run(() => GetMyContactRequestsAsync());           
        }

        private void OnRefreshCommandExecuted() => Task.Run(() => DoRefreshCon());

        async void OnDropContact(ContactsModel contacts)
        {
            IsRefreshing = true;

            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            string memberID = Preferences.Get("UserID","");
            await _conSvc.DeleteContact(memberID, contacts.ContactID!, jwtToken!);
            IsRefreshing = false;
            await DoRefreshCon();
        }

        async void OnAcceptContact(ContactsModel contacts)
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            string memberID = Preferences.Get("UserID","");
            await _conSvc.AcceptRequest(memberID, contacts.ContactID!, jwtToken!);
            await DoRefreshCon();
        }

        async void OnRejectContact(ContactsModel contacts)
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            string memberID = Preferences.Get("UserID","");
             await _conSvc.RejectRequest(memberID, contacts.ContactID!, jwtToken!);
            await DoRefreshCon();
        }

        async Task DoRefreshCon()
        {
            Contact.Clear(); ContactRequest.Clear();
            Contact = new List<ContactsModel>(); ContactRequest= new List<ContactsModel>();
            IsRefreshing = true; 
            await this.GetMyContactsAsync(); 
            await this.GetMyContactRequestsAsync();
            IsRefreshing = false;
        }
        
        public async Task GetMyContactsAsync()
        {
            List<ContactsModel> rn = await _conSvc.GetMyContacts();
            Contact = rn;

        }

        public async Task GetMyContactRequestsAsync()
        {
            List<ContactsModel> rn = await _conSvc.GetContactRequests();
            ContactRequest = rn;
        }

        public async void LogException(string msg, string stackTrace, string jwt)
        {
            await _conSvc.LogException(msg, stackTrace, jwt);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}