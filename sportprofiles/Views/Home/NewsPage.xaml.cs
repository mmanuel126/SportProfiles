
using sportprofiles.Models;
using sportprofiles.ViewModels;

namespace sportprofiles.Views.Home;

public partial class NewsPage : ContentPage
{
    private readonly NewsViewModel _newsViewModel;

    public NewsPage(NewsViewModel newsViewModel)
    {
        InitializeComponent();
        _newsViewModel  = newsViewModel;
        this.BindingContext = _newsViewModel;
    }

    async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0)
            return;
        var current = e.CurrentSelection;
        RecentNewsModel nm = (RecentNewsModel)current[0];
        await Launcher.OpenAsync(nm.NavigateUrl);
        ((CollectionView)sender).SelectedItem = null;
    }


}