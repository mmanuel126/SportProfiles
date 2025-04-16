
using sportprofiles.ViewModels;

namespace sportprofiles.Views.Account;

public partial class LogoutPage : ContentPage
{
    public LogoutPage()
    {
        InitializeComponent();

        // delete all data stored in the Preferences interface
        Preferences.Default.Clear();

        //delete all data stored in SecureStorage interface
        SecureStorage.Default.RemoveAll();

        // redirect to login page
        App.Current.MainPage = new sportprofiles.Views.Account.LoginPage(new MemberViewModel(new Services.Members()));
    }

}