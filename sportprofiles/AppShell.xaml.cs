using sportprofiles.Views.Member;

namespace sportprofiles;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("memberprofile", typeof(ProfilePage));
	}
}
