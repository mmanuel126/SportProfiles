using System.Diagnostics;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using sportprofiles.Models.Contacts;
using sportprofiles.Services;
using sportprofiles.ViewModels;
using sportprofiles.Views.Others;
namespace sportprofiles.Views.Contact;

public partial class ContactsPage : ContentPage
{
    private readonly ContactViewModel _contactViewModel;
    public ContactsPage(ContactViewModel contactViewModel)
    {
        InitializeComponent();
        _contactViewModel = contactViewModel;
        this.BindingContext = _contactViewModel;
         On<iOS>().SetUseSafeArea(true);
    }

    async void OnTapGestureRecognizerTapped(object sender, TappedEventArgs e)
    {
         try
            {
                if (e?.Parameter is ContactsModel model) {
                var data = (ContactsModel)e.Parameter!;

                Preferences.Set("ProfileID", data.ContactID);
                Preferences.Set("ProfileName", data.FriendName);
                Preferences.Set("ProfileTitle", data.TitleDesc);
                Preferences.Set("ProfileImage", data.PicturePath);
                Preferences.Set("ProfileLoginUser", "no");
                await Shell.Current.Navigation.PushModalAsync(new OthersProfilePage(new ProfileViewModel(new Members(), new Commons())));
                }else {
                    Debug.WriteLine("Tapped parameter is null or wrong type.");
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(HttpRequestException))
                {
                    await DisplayAlert("Network Error...", "Error accessing network or services. Check internet connection and then try again.", "Ok");
                }
                else
                {
                    await DisplayAlert(" General Error...", "A general error occured while you were using the application. The error has been logged and recorded for a specialist to look at. Try again in a bit later.", "Ok");
                    _contactViewModel.LogException(ex.Message, ex.StackTrace!, "");
                }
            }
    }

}