using sportprofiles.ViewModels;

namespace sportprofiles.Views.Member;

public partial class ProfilePage : ContentPage
{
    private readonly MemberViewModel _profileViewModel;

    public ProfilePage(MemberViewModel profileViewModel)
    {
        InitializeComponent();
        _profileViewModel = profileViewModel;
        BindingContext = profileViewModel;
    
        imgProfile.Source = _profileViewModel.ProfileBasicInfo.memProfileImage;
        lblName.Text = _profileViewModel.ProfileBasicInfo.memProfileName;
        lblTitle.Text = _profileViewModel.ProfileBasicInfo.memberProfileTitle;
    }


    async void OnRefreshProfile_Clicked(object sender, EventArgs e)
    {
        
    }

    async void OnEducationSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
       
    }

    async void OnPhotosButtonClicked(object sender, EventArgs args)
    {
    
    }

    async void OnVideoSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
    }
}
