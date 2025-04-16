
namespace sportprofiles.Views.Home;

public partial class PostsPage : ContentPage
{

    public PostsPage()
    {
        InitializeComponent();
        this.treeView.QueryNodeSize += treeView_QueryNodeSize!;  
    }

    public void treeView_QueryNodeSize(System.Object sender, Syncfusion.Maui.TreeView.QueryNodeSizeEventArgs e)
    {
        if (e.Node!.Level != 0)
        {
            e.Height = e.GetActualNodeHeight();
            e.Handled = true;
        }
    }

}