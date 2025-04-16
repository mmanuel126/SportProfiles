using sportprofiles.Services;
using sportprofiles.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace sportprofiles.ViewModels
{
    public class SettingsPrivacyViewModel : INotifyPropertyChanged
    {
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

        PrivacySettingsModel _PrivacySettingsInfo;
        public PrivacySettingsModel PrivacySettingsInfo
        {
            get
            {
                return _PrivacySettingsInfo;
            }
            set
            {
                _PrivacySettingsInfo = value;
                OnPropertyChanged();
            }
        }

        List<ProfilePrivacyTypesModel> _ProfilePrivacyTypes;
        public List<ProfilePrivacyTypesModel> ProfilePrivacyTypes
        {
            get
            {
                return _ProfilePrivacyTypes;
            }
            set
            {
                _ProfilePrivacyTypes = value;
                OnPropertyChanged();
            }
        }

        private readonly ISettings _settingsSvc;

        public SettingsPrivacyViewModel(ISettings settingsSvc)
        {
            try
            {
                _settingsSvc = settingsSvc;
                GetProfilePrivacyTypes();
                Task.Run(() => GetPrivacySettingsInfo().Wait());
            }
            catch (Exception ex)
            {
                IsRefreshing = false;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (ex.GetType() == typeof(HttpRequestException))
                    {
                        await App.Current.MainPage.DisplayAlert("Network Error...", "Error accessing network or services. Check internet connection and then try again.", "Ok");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(" General Error...", "A general error occured while you were using the application. The error has been logged and recorded for a specialist to look at. Try again in a bit later.", "Ok");
                    }
                });
            }
        }

        public async Task GetPrivacySettingsInfo()
        {
            try
            {
                string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
                string memberID = "0";
                if (!String.IsNullOrEmpty(Preferences.Get("UserID", "")))
                {
                    memberID = Preferences.Get("UserID", "");
                }
                PrivacySettingsInfo = await _settingsSvc.GetProfileSettings(memberID, jwtToken!);
            }
            catch (Exception ex)
            {
                IsRefreshing = false;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (ex.GetType() == typeof(HttpRequestException))
                    {
                        await App.Current.MainPage.DisplayAlert("Network Error...", "Error accessing network or services. Check internet connection and then try again.", "Ok");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(" General Error...", "A general error occured while you were using the application. The error has been logged and recorded for a specialist to look at. Try again in a bit later.", "Ok");
                    }
                });
            }
        }

        public void GetProfilePrivacyTypes()
        {
            List<ProfilePrivacyTypesModel> lst = new List<ProfilePrivacyTypesModel>();
            var question = new ProfilePrivacyTypesModel { Id = 0, Desc = "Select..." }; lst.Add(question);
            question = new ProfilePrivacyTypesModel { Id = 1, Desc = "Public" }; lst.Add(question);
            question = new ProfilePrivacyTypesModel { Id = 2, Desc = "Private" }; lst.Add(question);
            question = new ProfilePrivacyTypesModel { Id = 3, Desc = "Only Contacts" }; lst.Add(question);

            ProfilePrivacyTypes = lst;
        }

        public async void SaveProfileSettings(PrivacySettingsModel body)
        {
            try
            {
                string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
                string memberID = "0";
                if (!String.IsNullOrEmpty(Preferences.Get("UserID", "")))
                {
                    memberID = Preferences.Get("UserID", "");
                }
                await _settingsSvc.SaveProfileSettings(memberID, body, jwtToken!);
            }
            catch (Exception ex)
            {
                IsRefreshing = false;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (ex.GetType() == typeof(HttpRequestException))
                    {
                        await App.Current.MainPage.DisplayAlert("Network Error...", "Error accessing network or services. Check internet connection and then try again.", "Ok");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(" General Error...", "A general error occured while you were using the application. The error has been logged and recorded for a specialist to look at. Try again in a bit later.", "Ok");
                    }
                });
            }
        }

        public async void SaveSearchSettings(PrivacySettingsModel psm)
        {
            try
            {
                Settings s = new Settings();
                string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
                string memberID = "0";
                if (!String.IsNullOrEmpty(Preferences.Get("UserID", "")))
                    memberID = Preferences.Get("UserID", "");

                await s.SaveSearchSettings(memberID, psm, jwtToken!);
            }
            catch (Exception ex)
            {
                IsRefreshing = false;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (ex.GetType() == typeof(HttpRequestException))
                    {
                        await App.Current.MainPage.DisplayAlert("Network Error...", "Error accessing network or services. Check internet connection and then try again.", "Ok");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(" General Error...", "A general error occured while you were using the application. The error has been logged and recorded for a specialist to look at. Try again in a bit later.", "Ok");
                    }
                });
            }
        }

        public async void LogException(string msg, string stackTrace, string jwt)
        {
            await _settingsSvc.LogException(msg, stackTrace, jwt);
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
