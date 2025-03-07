using sportprofiles.Services;
using sportprofiles.ViewModels;
using sportprofiles.Views.Account;
namespace sportprofiles;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new LoginPage(new MemberViewModel(new Members())));
	}
}