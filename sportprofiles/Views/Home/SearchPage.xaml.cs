using sportprofiles.ViewModels;

namespace sportprofiles.Views.Home;

public partial class SearchPage : ContentPage
{
    private readonly SearchListViewModel _searchViewModel;
    public SearchPage(SearchListViewModel searchViewModel)
    {
        InitializeComponent();
        _searchViewModel = searchViewModel;
        this.BindingContext = _searchViewModel;
    }

    async void OnTapGestureRecognizerTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Redirect to User's Profile", "...Go to user's profile...not implemented yet.", "Ok");
    }

    async void OnConnectClicked(object sender, EventArgs e)
    {
        try
        {
            //  var swipeItem = sender as SwipeItem;
            // var data = swipeItem.BindingContext as SearchModel;

            bool ans = await DisplayAlert("Connection Request", "Please note the member will have to confirm your request. You should send this request only if you know this person. Are you sure you want to send this connection request?", "Yes", "No");
            if (ans)
            {
                await DisplayAlert("Code to Send Request", "...Send the request here...not implemented yet.", "Ok");
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
            }
        }
    }

    async void OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(searchBar.Text))
            {
                sportprofiles.Services.Contacts conSvc = new sportprofiles.Services.Contacts();
                var Result = await conSvc.GetSearchResult(searchBar.Text);
                searchList.ItemsSource = Result;
            }
            else
            {
                searchList.ItemsSource = null;
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
                _searchViewModel.LogException(ex.Message, ex.StackTrace!, "");
            }
        }
    }

}
