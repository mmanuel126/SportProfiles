using Microsoft.Extensions.Logging;
using sportprofiles.Services;
using sportprofiles.ViewModels;
using sportprofiles.Views.Home;
using sportprofiles.Views.Contact;
using sportprofiles.Views.Member;
using Syncfusion.Maui.Toolkit.Hosting;
using sportprofiles.Views.Account;

namespace sportprofiles;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>().ConfigureSyncfusionToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.RegisterAppServices()
			.RegisterModelViews()
			.RegisterViews();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<ICommons, Commons>();
		mauiAppBuilder.Services.AddSingleton<sportprofiles.Services.IContacts, sportprofiles.Services.Contacts>();
		mauiAppBuilder.Services.AddTransient<IMembers, Members>();
        return mauiAppBuilder;
    }

	public static MauiAppBuilder RegisterModelViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<NewsViewModel>();
		mauiAppBuilder.Services.AddTransient<ContactViewModel>();
		mauiAppBuilder.Services.AddTransient<SearchListViewModel>();
		mauiAppBuilder.Services.AddTransient<MemberViewModel>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<NewsPage>();
		mauiAppBuilder.Services.AddTransient<ContactsPage>();
        mauiAppBuilder.Services.AddTransient<SearchPage>();
		mauiAppBuilder.Services.AddTransient<ProfilePage>();
		mauiAppBuilder.Services.AddTransient<RegisterPage>();
        return mauiAppBuilder;
    }

}
