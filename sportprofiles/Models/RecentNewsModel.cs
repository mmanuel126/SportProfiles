using System;

namespace sportprofiles.Models
{
    public class RecentNewsModel
    {
        public required string ImageUrl { get; set; } 
        public required string HeaderText { get; set; }
        public DateTime PostingDate { get; set; }
        public required string TextField { get; set; }
        public required string NavigateUrl { get; set; }
        public int Id { get; set; }
        public string imgUrl => "https://www.sportprofiles.space/images" + ImageUrl.Replace("~","").Replace("Images","");
        public string Description => $"{TextField}".Substring(0, 120) + "...";
    }
}
