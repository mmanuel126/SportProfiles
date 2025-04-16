using System;

namespace sportprofiles.Models
{ 

    public class RecentPostChildModel
    {
        public int PostResponseID { get; set; }
        public int PostID { get; set; }
        public string? Description { get; set; }
        public string? DateResponded { get; set; }
        public int MemberID { get; set; }
        public string? MemberName { get; set; }
        public string? FirstName { get; set; }
        public string? PicturePath { get; set; }
        public string? SelectedStateIcon { get; set; }
        public string? DeselectedStateIcon { get; set; }
        public bool IsSelected { get; set; }
        public Action<RecentPostChildModel>? OnClickListener { get; set; }
        
    }
}
