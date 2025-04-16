using sportprofiles.Services;
using sportprofiles.ViewModels;
using sportprofiles.Views.Home;
using sportprofiles.Views.Contact;
using sportprofiles.Views.Member;
using Syncfusion.Maui.Toolkit.Hosting;
using sportprofiles.Views.Account;
using sportprofiles.Views.Setting;
using sportprofiles.Views;
using Syncfusion.Maui.Core.Hosting;
using sportprofiles.Views.Message;
using Microsoft.Extensions.Logging;

namespace sportprofiles;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>().ConfigureSyncfusionToolkit().ConfigureSyncfusionCore()
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
		mauiAppBuilder.Services.AddSingleton<ISettings, Settings>();
		mauiAppBuilder.Services.AddSingleton<IMessages, sportprofiles.Services.Messages>();
        return mauiAppBuilder;
    }

	public static MauiAppBuilder RegisterModelViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<NewsViewModel>();
		mauiAppBuilder.Services.AddTransient<ContactViewModel>();
		mauiAppBuilder.Services.AddTransient<SearchListViewModel>();
		mauiAppBuilder.Services.AddTransient<MemberViewModel>();
		mauiAppBuilder.Services.AddTransient<SettingsPrivacyViewModel>();
        mauiAppBuilder.Services.AddTransient<SettingsAccountViewModel>();
		mauiAppBuilder.Services.AddTransient<ProfileViewModel>();
		mauiAppBuilder.Services.AddTransient<PostsViewModel>();
		mauiAppBuilder.Services.AddTransient<ContactAutocompleteViewModel>();
        mauiAppBuilder.Services.AddTransient<MessageDetailsViewModel>();
		mauiAppBuilder.Services.AddTransient<MessageViewModel>();
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<NewsPage>();
		mauiAppBuilder.Services.AddTransient<ContactsPage>();
        mauiAppBuilder.Services.AddTransient<SearchPage>();
		mauiAppBuilder.Services.AddTransient<ProfilePage>();
		mauiAppBuilder.Services.AddTransient<ProfileEditPage>();
        mauiAppBuilder.Services.AddTransient<ProfileAddEducationPage>();
        mauiAppBuilder.Services.AddTransient<ProfileUpdateEducationPage>();
		mauiAppBuilder.Services.AddTransient<RegisterPage>();
		mauiAppBuilder.Services.AddTransient<PrivacySettingsPage>();
        mauiAppBuilder.Services.AddTransient<AccountSettingsPage>();
		mauiAppBuilder.Services.AddTransient<MessagePage>();
        mauiAppBuilder.Services.AddTransient<MessageNewPage>();
        mauiAppBuilder.Services.AddTransient<MessageDetailsPage>();
        return mauiAppBuilder;
    }
}
