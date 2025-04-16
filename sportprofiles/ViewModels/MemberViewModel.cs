using sportprofiles.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using sportprofiles.Models.Members;

namespace sportprofiles.ViewModels
{

    public class MemberViewModel: INotifyPropertyChanged
    {
        private readonly IMembers _membersSvc;

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

        public MemberViewModel(IMembers membersSvc)
        {
            _membersSvc = membersSvc;

              Task.Run(() => GetMemberBasicInfo().Wait());
              Task.Run(() => GetMemberContactInfo().Wait());
              Task.Run(() => GetMemberEducationInfo().Wait());
        }

        public async Task GetMemberBasicInfo()
        {
            string memberID = GetMemberID();
            ProfileBasicInfo = await _membersSvc.GetMemberBasicInfo(memberID);
        }

        public async Task GetMemberContactInfo()
        {
            string memberID = GetMemberID();
            ProfileContactInfo = await _membersSvc.GetMemberContactInfo(memberID);
        }

        public async Task GetMemberEducationInfo()
        {
            string memberID = GetMemberID();
            ProfileEducation =  await _membersSvc.GetMemberEducationInfo(memberID);
        }

        public async Task<string> Register(RegisterModel register)
        {
            Members _mem = new();
            return await _mem.RegisterUser(register);
        }

        public async Task<UserModel> AuthenticateUser(string username, string pwd)
        {
            return await _membersSvc.AuthenticateUser(username, pwd);
        }

        public async Task<string> ResetPassword(string email)
        {
            return await _membersSvc.ResetPassword(email);
        }

        public async Task<string> IsResetCodeExpired(string code)
        {
            return await _membersSvc.IsResetCodeExpired(code);
        }

        public async Task<string> ChangePassword(RegisterModel register)
        {
            return await _membersSvc.ChangePassword(register);
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

        public async void LogException(string msg, string stackTrace, string jwt)
        {
            await _membersSvc.LogException(msg, stackTrace, jwt);
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
