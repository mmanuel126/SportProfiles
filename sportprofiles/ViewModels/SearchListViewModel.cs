using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using sportprofiles.Models.Contacts;
using System.Windows.Input;

namespace sportprofiles.ViewModels
{
    public class SearchListViewModel : INotifyPropertyChanged
    {
        Task<ObservableCollection<ContactsModel>> _searchResult;
        public Task<ObservableCollection<ContactsModel>> SearchResults
        {
            get
            {
                return _searchResult;
            }
            set
            {
                _searchResult = value;
                OnPropertyChanged();
            }
        }

        private readonly sportprofiles.Services.IContacts _conSvc;

        public ICommand PerformSearch => new Command<string>((string searchText) =>
        {

            SearchResults =  GetSearchResults(searchText);
        });

        public SearchListViewModel(sportprofiles.Services.IContacts conSvc)
        {
            _conSvc = conSvc;
        }

        async Task<ObservableCollection<ContactsModel>> GetSearchResults(string searchText)
        {
            var result = await  _conSvc.GetSearchResult(searchText);
            return result;
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

