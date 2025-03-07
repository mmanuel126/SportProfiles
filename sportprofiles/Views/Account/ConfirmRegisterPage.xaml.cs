using sportprofiles.Services;
using sportprofiles.ViewModels;

namespace sportprofiles.Views.Account;

public partial class ConfirmRegisterPage : ContentPage
{
    private readonly MemberViewModel _memberViewModel;

	public ConfirmRegisterPage(MemberViewModel memberViewModel)
	{
		InitializeComponent();
        _memberViewModel = memberViewModel;
        this.BindingContext = memberViewModel;
    }

    private async void Register_confirm_tap_Tapped(object sender, EventArgs e)
    {
        //when you touch return to login screen label
        var LoginPage = new LoginPage(new MemberViewModel(new Members()));
        await Navigation.PushModalAsync(LoginPage);
    }
}
