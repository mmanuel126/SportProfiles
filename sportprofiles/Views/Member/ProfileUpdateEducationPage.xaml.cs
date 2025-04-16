using sportprofiles.Models.Members;
using sportprofiles.ViewModels;

namespace sportprofiles.Views;

public partial class ProfileUpdateEducationPage : ContentPage
{
    private readonly ProfileViewModel _profileViewModel;
    MemberProfileEducationModel educationModel = new MemberProfileEducationModel();
    public ProfileUpdateEducationPage(ProfileViewModel profileViewModel)
    {
        InitializeComponent();
        _profileViewModel = profileViewModel;
        this.BindingContext = profileViewModel;

        educationModel.SchoolImage = Preferences.Get("schoolimage", "");
        educationModel.Major = Preferences.Get("major", "");
        educationModel.Degree = Preferences.Get("degree", "");
        educationModel.YearClass = Preferences.Get("year", "");
        educationModel.SportLevelType = Preferences.Get("competitionlevel", "");
        educationModel.SchoolType = Preferences.Get("schoolType", "");
        educationModel.SchoolID = Preferences.Get("schoolID", "");
        educationModel.SchoolName = Preferences.Get("schoolName", "");
        educationModel.SchoolType = Preferences.Get("schoolType", "");

        imgProfile.Source = educationModel.SchoolImage;

        int i = educationModel.Major.IndexOf('-');
        string sMajor;
        if (i == -1)
            sMajor = educationModel.Major;
        else
            sMajor = educationModel.Major.Substring(0, i - 1);

        if (educationModel.SchoolName.Length >= 30)
            educationModel.SchoolName = educationModel.SchoolName.Substring(0, 30) + "...";

        lblName.Text = educationModel.SchoolName;
        lblMajor.Text = sMajor;

        if (educationModel.Degree == "1")
            educationModel.Degree = "Undergraduate";
        else if (educationModel.Degree == "2")
            educationModel.Degree = "Post Graduate";
        else if (educationModel.Degree == "3")
            educationModel.Degree = "High School Diploma";
        else if (educationModel.Degree == "4")
            educationModel.Degree = "GED";

        DegreePicker.SelectedItem = educationModel.Degree;
        YearPicker.SelectedItem = educationModel.YearClass;
        SportLevelPicker.SelectedItem = educationModel.SportLevelType;
    }

    async void OnCancel_Clicked(object sender, EventArgs args)
    {
        await Navigation.PopModalAsync();
    }

    async void OnUpdate_Clicked(object sender, EventArgs args)
    {
        try
        {
            //do update here
            educationModel.SchoolName = lblName.Text;
            educationModel.Major = lblMajor.Text;
            educationModel.Degree = DegreePicker.SelectedItem.ToString();
            educationModel.YearClass = YearPicker.SelectedItem.ToString();
            educationModel.SportLevelType = SportLevelPicker.SelectedItem.ToString();

            if (String.IsNullOrEmpty(educationModel.Societies))
                educationModel.Societies = "";

            if (educationModel.Degree == "Undergraduate")
                educationModel.Degree = "1";
            else if (educationModel.Degree == "Post Graduate")
                educationModel.Degree = "2";
            else if (educationModel.Degree == "High School Diploma")
                educationModel.Degree = "3";
            else if (educationModel.Degree == "GED")
                educationModel.Degree = "4";

            await _profileViewModel.UpdateEducation(educationModel);
            await Navigation.PopModalAsync();
            MessagingCenter.Send<ProfileUpdateEducationPage>(this, "RefreshEducation");
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
                _profileViewModel.LogException(ex.Message,ex.StackTrace!,"");
            }
        }
    }
}
