using sportprofiles.Services;
using sportprofiles.Models.Contacts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace sportprofiles.ViewModels
{
    public class ContactViewModel : INotifyPropertyChanged
    {
       
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
            
            _conSvc = conSvc;
            Contact = new List<ContactsModel>();
            ContactRequest = new List<ContactsModel>();
            Task.Run(() => GetMyContactsAsync());
            Task.Run(() => GetMyContactRequestsAsync());           
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

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}