namespace sportprofiles.Controls
{
    public partial class FlyoutHeader : ContentView
    {
        public FlyoutHeader()
        {
            InitializeComponent();
            
            imgProfile.Source =  "profile.png";
            lblName.Text = "Marc Manuel";
            lblTitle.Text =  "Amateur Basketball Player";
        }
    }
}