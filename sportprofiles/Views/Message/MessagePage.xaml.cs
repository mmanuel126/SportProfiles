using sportprofiles.Models;
using sportprofiles.Services;
using sportprofiles.ViewModels;
using sportprofiles.Views.Others;

namespace sportprofiles.Views.Message;

public partial class MessagePage : ContentPage
{
    private readonly MessageViewModel _messageViewModel;
	public MessagePage(MessageViewModel messageViewModel)
	{
		InitializeComponent();
        _messageViewModel = messageViewModel;
        this.BindingContext = messageViewModel;
    }

    async void OnItemClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new MessageNewPage(new ContactAutocompleteViewModel()));
    }

    async void OnTapGestureRecognizerTapped(object sender, EventArgs e)
    {
        var label = sender as Label;
        var data = label!.BindingContext as MessageInfoModel;

        Preferences.Set("ProfileID", data!.FromID);
        Preferences.Set("ProfileName", data.ContactName);
        Preferences.Set("ProfileTitle", data.SenderTitle);
        Preferences.Set("ProfileImage", data.SenderImage);
        Preferences.Set("ProfileLoginUser", "no");
       await Shell.Current.Navigation.PushModalAsync(new OthersProfilePage( new ProfileViewModel (new Members(), new Commons())));
    }
}
