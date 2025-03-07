namespace sportprofiles.Controls
{
    public partial class FlyoutHeader : ContentView
    {
        public FlyoutHeader()
        {
            InitializeComponent();
            
            imgProfile.Source =  "https://www.sportprofiles.space/images/members/"  + Preferences.Get("UserImage","").ToString();
            lblName.Text = Preferences.Get("UserName","").ToString();
            lblTitle.Text = Preferences.Get("UserTitle","").ToString();
        }
    }
}