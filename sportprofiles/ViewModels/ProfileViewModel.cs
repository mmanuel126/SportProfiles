using System.ComponentModel;
using System.Runtime.CompilerServices;
using sportprofiles.Services;
using sportprofiles.Views;
using sportprofiles.Models.Members;
using sportprofiles.Models;

namespace sportprofiles.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        public Command<MemberProfileEducationModel> DeleteCommand { get; set; }
        public Command<MemberProfileEducationModel> EditCommand { get; set; }
        public Command AddNewCommand { get; set; }
        public Command RefreshCommand { get; set; }

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

        MemberProfileBasicInfoModel _memberProfileBasicInfo;
        public MemberProfileBasicInfoModel ProfileBasicInfo
        {
            get { return _memberProfileBasicInfo; }
            set { _memberProfileBasicInfo = value; OnPropertyChanged(); }
        }

        MemberProfileContactInfoModel _memberProfileContactInfo;
        public MemberProfileContactInfoModel ProfileContactInfo
        {
            get { return _memberProfileContactInfo; }
            set { _memberProfileContactInfo = value; OnPropertyChanged(); }
        }

        List<MemberProfileEducationModel> _memberEducation;
        public List<MemberProfileEducationModel> ProfileEducation
        {
            get { return _memberEducation; }
            set { _memberEducation = value; OnPropertyChanged(); }
        }

        public Item SelectedSport { get; set; }
        List<Item> _sportsList;
        public List<Item> SportsList
        {
            get { return _sportsList; }
            set { _sportsList = value; OnPropertyChanged(); }
        }

        List<SchoolsByStateModel> _schools;
        public List<SchoolsByStateModel> Schools
        {
            get { return _schools; }
            set { _schools = value; OnPropertyChanged(); }
        }

        List<StatesModel> _states;
        public List<StatesModel> States
        {
            get { return _states; }
            set { _states = value; OnPropertyChanged(); }
        }

        private readonly IMembers _membersSvc;
        private readonly ICommons _commonsSvc;
        public ProfileViewModel(IMembers membersSvc, ICommons commonsSvc)
        {
            try
            {
                _membersSvc = membersSvc;
                _commonsSvc = commonsSvc;

                IsRefreshing = true;

                Task.Run(() => GetMemberBasicInfo().Wait());
                //GetSportsList();
                Task.Run(() => GetMemberContactInfo().Wait());

                DeleteCommand = new Command<MemberProfileEducationModel>(OnDeleteEducation);
                EditCommand = new Command<MemberProfileEducationModel>(OnEditEducation);
                AddNewCommand = new Command(OnAddNewEducation);
                RefreshCommand = new Command(OnRefreshCommandExecuted);

                Task.Run(() => GetMemberEducation().Wait());
                Task.Run(() => GetStates().Wait());
                //GetSchools();

                MessagingCenter.Subscribe<ProfileUpdateEducationPage>(this, "RefreshEducation", (sender) =>
                    {
                        Task.Run(() => GetMemberEducation().Wait());
                    });

                MessagingCenter.Subscribe<ProfileAddEducationPage>(this, "RefreshEducation", (sender) =>
                    {
                        Task.Run(() => GetMemberEducation().Wait());
                    });
                
                IsRefreshing = false;    

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
                        await _membersSvc!.LogException(ex.Message, ex.StackTrace!, "");
                    }
                });
            }

        }

        async void OnAddNewEducation()
        {
            //call the add new edu info page 
            await Application.Current.MainPage.Navigation.PushModalAsync(new ProfileAddEducationPage(this));
        }

        private void OnRefreshCommandExecuted() => Task.Run(() => DoRefreshPosts());

        async Task DoRefreshPosts()
        {
            try
            {
               
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
                        await _membersSvc.LogException(ex.Message, ex.StackTrace!, "");
                    }
                });
            }
        }

        async void OnEditEducation(MemberProfileEducationModel educationModel)
        {
            //store current edu info in local storage
            Preferences.Set("schoolimage", educationModel.SchoolImage);
            Preferences.Set("major", educationModel.Major);
            Preferences.Set("degree", educationModel.Degree);
            Preferences.Set("year", educationModel.YearClass);
            Preferences.Set("competitionlevel", educationModel.SportLevelType);
            Preferences.Set("schoolType", educationModel.SchoolType);
            Preferences.Set("schoolID", educationModel.SchoolID);
            Preferences.Set("schoolName", educationModel.SchoolName);
            //call update edu page
            await Application.Current.MainPage.Navigation.PushModalAsync(new ProfileUpdateEducationPage(this));
        }

        async void OnDeleteEducation(MemberProfileEducationModel educationModel)
        {
            //get jwt token and member id from local storage
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            string memberID = Preferences.Get("UserID", "");
            //call service that makes API call to remove the school
            await _membersSvc.RemoveSchool(memberID, educationModel.SchoolID!, educationModel.SchoolType!, jwtToken!);
            //get latest edu info to be able to refresh screen with changes
            ProfileEducation = await _membersSvc.GetMemberEducationInfo(memberID);
        }

        public async Task AddNewEducation(MemberProfileEducationModel schoolInfo)
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            string memberID = Preferences.Get("UserID", "");
            await _membersSvc.AddNewSchool(memberID, schoolInfo, jwtToken!);
            await GetMemberEducation();
        }

        public async Task UpdateEducation(MemberProfileEducationModel schoolInfo)
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            string memberID = Preferences.Get("UserID", "");
            await _membersSvc.UpdateSchool(memberID, schoolInfo, jwtToken!);
            await GetMemberEducation();
        }

        public async Task GetMemberBasicInfo()
        {
            string memberID = GetMemberID();
            ProfileBasicInfo = await _membersSvc.GetMemberBasicInfo(memberID);
        }

        public async Task SaveMemberGeneralInfo(MemberProfileBasicInfoModel model)
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            string memberID = Preferences.Get("UserID", "");
            await _membersSvc.SaveMemberGeneralInfo(memberID, model, jwtToken!);
        }

        public async Task GetMemberContactInfo()
        {
            //string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            string memberID = GetMemberID ();
            var pcInfoLst = await _membersSvc.GetMemberContactInfo(memberID);
            ProfileContactInfo = pcInfoLst;
        }

        public async Task SaveMemberContactInfo(MemberProfileContactInfoModel model)
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            string memberID = Preferences.Get("UserID", "");
            await _membersSvc.SaveMemberContactInfo(memberID, model, jwtToken!);
        }


        public async Task GetMemberEducation()
        {
            string memberID = GetMemberID();
            //get latest member edu info
            ProfileEducation = await _membersSvc.GetMemberEducationInfo(memberID);
        }

        private async Task GetStates()
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            States = await _commonsSvc.GetStates(jwtToken!);
        }

        private string GetMemberID()
        {
            string memberID = "0";
            string isLoginUser = Preferences.Get("ProfileLoginUser", "yes");
            if (isLoginUser == "yes")
            {
                if (!String.IsNullOrEmpty(Preferences.Get("UserID", "")))
                {
                    memberID = Preferences.Get("UserID", "");
                }
            }
            else if (isLoginUser== "no")
            {
                if (!String.IsNullOrEmpty(Preferences.Get("ProfileID", "")))
                {
                    memberID = Preferences.Get("ProfileID", "");
                }
            }
            return memberID;
        }

        private async Task GetSchools()
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            Schools = await _commonsSvc.GetSchoolsByState("AZ", "3", jwtToken!);
        }

        public async void LogException(string msg, string stackTrace, string jwt)
        {
            await _membersSvc.LogException(msg, stackTrace, jwt);
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged!;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}