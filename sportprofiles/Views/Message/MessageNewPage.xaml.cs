using System.Windows.Input;
using Microsoft.Maui.Controls;
using sportprofiles.Models;
using sportprofiles.Models.Contacts;
using sportprofiles.ViewModels;

namespace sportprofiles.Views.Message;

public partial class MessageNewPage : ContentPage
{
    public string contactID = "0";

    private readonly ContactAutocompleteViewModel _contactAutocompleteViewModel;
    public MessageNewPage(ContactAutocompleteViewModel contactAutocompleteViewModel)
    {
        InitializeComponent();
        _contactAutocompleteViewModel = contactAutocompleteViewModel;
        this.BindingContext = _contactAutocompleteViewModel;
    }

    private  void autoComplete_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        var current = e.CurrentSelection;
        ContactsModel nm = (ContactsModel)current![0];
        if (nm != null)
           contactID = nm.ContactID;
    }

    async void OnCancel_Clicked(object sender, EventArgs args)
    {
        await Navigation.PopModalAsync();
    }

    async void OnAddNew_Clicked(object sender, EventArgs args)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(txtSubject.Text))
            {
                await DisplayAlert("Subject Text Required...", "Please enter a subject text for the message!", "Ok");
                txtSubject.Focus();
            }
            else if (String.IsNullOrWhiteSpace(txtMessage.Text))
            {
                await DisplayAlert("Subject Message Required...", "Please enter a message text!", "Ok");
                txtMessage.Focus();
            }
            else if (contactID == "0")
            {
                await DisplayAlert("Message 'To' is Required...", "Please select a connection to send the message to!", "Ok");
                txtMessage.Focus();
            }
            else
            {
                await _contactAutocompleteViewModel.SendMessage(contactID, txtSubject.Text, txtMessage.Text);
                txtSubject.Text = "";
                txtMessage.Text = "";
                contactID ="0";
                autoComplete.Text = ""; txtSubject.Focus();
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
                _contactAutocompleteViewModel.LogException(ex.Message, ex.StackTrace!, "");
            }
        }
    }
}