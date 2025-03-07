
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using sportprofiles.Models.Contacts;
using sportprofiles.Services;
using System.Windows.Input;

namespace sportprofiles.ViewModels
{
    public class SearchListViewModel : INotifyPropertyChanged
    {
        List<ContactsModel> _searchResult;
        public List<ContactsModel> SearchResults
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

        public ICommand PerformSearch => new Command<string>((string query) =>
        {
            SearchResults =  GetSearchResults(query);
        });

        public SearchListViewModel(sportprofiles.Services.IContacts conSvc)
        {
            _conSvc = conSvc;
        }

        List<ContactsModel> GetSearchResults(string query)
        {
            var result = (List<ContactsModel>) _conSvc.GetSearchResult();
            return result;
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

