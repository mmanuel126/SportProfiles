
using sportprofiles.Services;
using sportprofiles.ViewModels;

namespace sportprofiles.Views.Account;

public partial class ForgotPasswordPage: ContentPage
{
    private readonly MemberViewModel _memberViewModel;
    public ForgotPasswordPage(MemberViewModel memberViewModel)
	{
		InitializeComponent();
        _memberViewModel = memberViewModel;
        this.BindingContext = memberViewModel;
    }

    private async void Login_tap_Tapped(object sender, EventArgs e)
    {
        var LoginPage = new LoginPage( new MemberViewModel(new Members()));
        await Navigation.PushModalAsync(LoginPage);
    }

    private async void ForgotPwdButton_Clicked(object sender, EventArgs e)
    {
        // Check for a valid email address.
        if (String.IsNullOrEmpty(EmailText.Text))
        {
            await DisplayAlert("Email Required...", "Please enter your email address!", "Ok");
            EmailText.Focus();
        }
        else
        {
            try
            {
                //call service via vm and do things
                await _memberViewModel.ResetPassword(EmailText.Text);
                Preferences.Set("ResetPwdEmail",EmailText.Text);
                var resetPwdPage = new ResetPasswordPage(_memberViewModel);
                await Navigation.PushModalAsync(new NavigationPage(resetPwdPage));
            }
            catch (FormatException)
            {
                await DisplayAlert("Network Error...", "Error accessing network or services. Check internet connection and then try again.", "Ok");
            }
        }
    }
}