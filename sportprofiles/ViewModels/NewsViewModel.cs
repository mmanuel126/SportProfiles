
using sportprofiles.Services;
using sportprofiles.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace sportprofiles.ViewModels
{
    public class NewsViewModel : INotifyPropertyChanged
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

        List<RecentNewsModel> _News;
        public List<RecentNewsModel> News
        {
            get
            {
                return _News;
            }
            set
            {
                _News = value;
                OnPropertyChanged();
            }
        }

        private readonly ICommons _commonSvc;

        public NewsViewModel(ICommons commonSvc)
        {
            RefreshCommand = new Command(OnRefreshCommandExecuted);
            _commonSvc = commonSvc;
            News = new List<RecentNewsModel>();
            Task.Run(() => GetNewsAsync().Wait());
        }

        private void OnRefreshCommandExecuted() => Task.Run(() => GetNewsAsync());

        async Task GetNewsAsync()
        {
            try
            {
                IsRefreshing = true;
                List<RecentNewsModel> rn = await _commonSvc.GetRecentNews();
                News = rn;
                IsRefreshing = false;
            }
            catch (Exception ex)
            {
                IsRefreshing = false;
                if (ex.GetType() == typeof(HttpRequestException))
                {
                    await App.Current.MainPage.DisplayAlert("Network Error...", "Error accessing network or services. Check internet connection and then try again.", "Ok");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(" General Error...", "A general error occured while you were using the application. The error has been logged and recorded for a specialist to look at. Try again in a bit later.", "Ok");
                }
            }
        }

          public async void LogException(string msg, string stackTrace, string jwt)
        {
            await _commonSvc.LogException(msg, stackTrace, jwt);
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