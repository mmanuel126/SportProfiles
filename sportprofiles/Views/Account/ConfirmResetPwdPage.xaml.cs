using sportprofiles.ViewModels;

namespace sportprofiles.Views.Account;

public partial class ConfirmResetPwdPage : ContentPage
{
    private readonly MemberViewModel _memberViewModel;
	public ConfirmResetPwdPage(MemberViewModel memberViewModel)
	{
		InitializeComponent();
        _memberViewModel = memberViewModel;
        this.BindingContext = memberViewModel;
    }

    private async void Login_confirm_tap_Tapped(object sender, EventArgs e)
    {
        //when you tap return to login screen label
        var loginPage = new LoginPage(_memberViewModel);
        await Navigation.PushModalAsync(loginPage);
    }
}
