
using sportprofiles.Services;
using sportprofiles.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace sportprofiles.ViewModels
{
    public class NewsViewModel : INotifyPropertyChanged
    {
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
            _commonSvc = commonSvc;
            News = new List<RecentNewsModel>();
            Task.Run(() => GetNewsAsync().Wait());
        }

        async Task GetNewsAsync()
        {
                List<RecentNewsModel> rn = await _commonSvc.GetRecentNews();
                News = rn;
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