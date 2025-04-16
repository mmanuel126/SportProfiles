namespace sportprofiles.Models
{
    public class RecentPostsModel
    {
        public string? PostID { get; set; }
        public string? Description { get; set; }
        public string? DatePosted { get; set; }
        public string? PicturePath { get; set; }
        public string? MemberName { get; set; }
        public string? FirstName { get; set; }
        public string? MemberID { get; set; }
        public string? SelectedStateIcon { get; set; }
        public string? DeselectedStateIcon { get; set; }
        public bool IsSelected { get; set; }
        public Action<RecentPostsModel>? OnClickListener { get; set; }
        public List<RecentPostsModel>? ChildItems { get; set; }
        public int ReplyCount { get; set; }

        private List<RecentPostChildModel>? Children;
        public List<RecentPostChildModel>? GetChildren()
        {
            return Children;
        }

        public void SetChildren(List<RecentPostChildModel> children)
        {
            this.Children = children;
        }
    }

}
