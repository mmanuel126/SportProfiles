using sportprofiles.Models;
using sportprofiles.Models.Contacts;
using sportprofiles.Services;
using sportprofiles.ViewModels;
using sportprofiles.Views.Member;
namespace sportprofiles.Views.Contact;

public partial class ContactsPage : ContentPage
{
    private readonly ContactViewModel _contactViewModel;
    public ContactsPage(ContactViewModel contactViewModel)
    {
        InitializeComponent();
        _contactViewModel = contactViewModel;
        this.BindingContext = _contactViewModel;
    }

    async void OnTapGestureRecognizerTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("memberprofile");
    }

}