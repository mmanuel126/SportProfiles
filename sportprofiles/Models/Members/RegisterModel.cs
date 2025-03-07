namespace sportprofiles.Models.Members
{
public class RegisterModel
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPwd { get; set; }
        public required string Gender { get; set; }
        public required string Month { get; set; }
        public required string Day { get; set; }
        public required string Year { get; set; }
        public required string Code { get; set; }
        public required string ProfileType { get; set; }
    }
}