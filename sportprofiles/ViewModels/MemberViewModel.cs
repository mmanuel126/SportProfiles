using sportprofiles.Services;
using sportprofiles.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
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
            ProfileBasicInfo = await _membersSvc.GetMemberBasicInfo();
        }

        public async Task GetMemberContactInfo()
        {
            ProfileContactInfo = await _membersSvc.GetMemberContactInfo();
        }

        public async Task GetMemberEducationInfo()
        {
            ProfileEducation =  await _membersSvc.GetMemberEducationInfo();
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

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
