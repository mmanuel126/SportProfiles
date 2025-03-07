
namespace sportprofiles.Models.Members 
{
public class UserModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? MemberID { get; set; }
        public string? PicturePath { get; set; }
        public string? AccessToken { get; set; }
        public string? Title { get; set; }
        public string? ExpiredDate { get; set; }
        public string? CurrentStatus { get; set; }
    }
}