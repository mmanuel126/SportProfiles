using System;
using sportprofiles.Models.Members;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using sportprofiles;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace sportprofiles.Services
{
    public class Members : IMembers
    {
        //api account service URI
        private static readonly string ACCOUNT_SERVICE_URI = "https://www.sportprofiles.space/services/account/";
        private static readonly string MEMBER_SERVICE_URI = "https://www.sportprofiles.space/services/member/";
        private static readonly HttpClient httpClient = new();
        public Members()
        {

        }


        /// <summary>
        /// Gets the member basic info.
        /// </summary>
        /// <returns>The member basic info.</returns>
        /// <param name="memberID">Member identifier.</param>
        public async Task<MemberProfileBasicInfoModel> GetMemberBasicInfo()
        {
            //simulate an async operation (e.g. data fetch from a DB or API)
            await Task.Delay(1000); //simulate a delay

            MemberProfileBasicInfoModel lst = new()
            {
                memProfileImage = "profile.png",
                memProfileName = "Marc Manuel",
                memberProfileTitle = "Pro Basketball",
                memProfileStatus = "Athlete (Professional)",
                memProfileGender = "Male",
                memProfileDOB = "7/13/1965",
                memProfileInterestedInc = "",
                memProfileLookingFor = "Networking",
                CurrentCity = "",
                CurrentStatus = "",
                FirstName = "Marc",
                InterestedInType = "",
                JoinedDate = "12/23/2000",
                LastName = "Manuel",
                LookingForEmployment = true,
                LookingForNetworking = true,
                LookingForPartnership = true,
                LookingForRecruitment = true,
                MemberID = "1",
                MiddleName = "P.",
                Sport = "Basketball",
                LeftRightHandFoot = "Left",
                PreferredPosition = "Shooting Guard",
                SecondaryPosition = " Small Forward",
                InterestedDesc = "",
                Height = "6' 9''",
                Weight = "200 lbs",
                Bio = "Software Architect/Developer who enjoys all kinds of sports.",
                TitleDesc = "",
                Sex = "Male"
            };
            return lst;
        }

        public async Task<MemberProfileContactInfoModel> GetMemberContactInfo()
        {
            //simulate an async operation (e.g. data fetch from a DB or API)
            await Task.Delay(1000); //simulate a delay

            MemberProfileContactInfoModel lst = new()
            {
                Email = "myemael@myemail.com",
                OtherEmail = "second@email.com",
                Website = "www.test.com",
                HomePhone = "444-555-6666",
                CellPhone = "334-555-1234",
                Address = "455 Boston Avenue",
                City = "Dorchester",
                Neighborhood = "Mattapan",
                State = "MA",
                Zip = "43344",
                ShowAddress = true,
                ShowEmailToMembers = false,
                ShowCellPhone = true,
                ShowHomePhone = false,
                Facebook = "www.facebook.com/myfbhandler",
                Instagram = "www.instagram.com/myinstragramhandler",
                Twitter = "www.x.com/mytwitterhander"
            };
            return lst;
        }

        /// <summary>
        /// Gets the member education info.
        /// </summary>
        /// <returns>The member education info.</returns>
        /// <param name="memberID">Member identifier.</param>
        public async Task<List<MemberProfileEducationModel>> GetMemberEducationInfo()
        {
            //simulate an async operation (e.g. data fetch from a DB or API)
            await Task.Delay(1000); //simulate a delay

            //Create and populate the list with data
            List<MemberProfileEducationModel> lst =
            [
                new() {
                    SchoolID = "1",
                    SchoolName = "SPSU",
                    SchoolImage ="profile.png",
                    SchoolAddress = "2333 Marietta Boulevard",
                    Major = "1989 - Computer Science",
                    Degree = "MS",
                    YearClass = "1997",
                    SchoolType = "College",
                    Societies = "Alpha",
                    WebSite = "www.spsu.com",
                    SportLevelType = "Division 1"
                },
                new() {
                   SchoolID = "2",
                    SchoolName = "UMass",
                    SchoolImage ="profile.png",
                    SchoolAddress = "2333 Morrisey Boulevard",
                    Major = "1989 - Computer Science",
                    Degree = "BS",
                    YearClass = "1989",
                    SchoolType = "College",
                    Societies = "OMega",
                    WebSite = "www.umass.com",
                    SportLevelType = "Division 1"
                },
                new() {
                    SchoolID = "3",
                    SchoolName = "West Roxbury",
                    SchoolImage ="profile.png",
                    SchoolAddress = "2333 West Roxbury Boulevard",
                    Major = "1985 - High School Prep",
                    Degree = "HS Degree",
                    YearClass = "1985",
                    SchoolType = "High School",
                    Societies = "Science",
                    WebSite = "www.westroxbury.com",
                    SportLevelType = "Division 1"
                }
            ];
            return lst;
        }

        /// <summary>
        /// Registers the user to lg.
        /// </summary>
        /// <returns>existing or newemail string.</returns>
        /// <param name="register">Register.</param>
        public async Task<string> RegisterUser(RegisterModel register)
        {
            var jsonContent = JsonConvert.SerializeObject(register);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            //send the post request with the serialized object
            var response = await httpClient.PostAsync(ACCOUNT_SERVICE_URI + "register", content);
            response.EnsureSuccessStatusCode(); //ensures that the Http status code indicates success

            var rtnStr = response.Content.ReadAsStringAsync(); //read the JSON content from the response body as a string
            return rtnStr.Result;  // a string that returns a message like if the email already exist or not. cannot have dup emails
        }

        /// <summary>
        /// Authenticates the User.
        /// </summary>
        /// <returns>The LGU ser.</returns>
        /// <param name="username">Email.</param>
        /// <param name="pwd">Pwd.</param>
        public async Task<UserModel> AuthenticateUser(string username, string pwd)
        {
            var body = new 
            {
                email = username,
                password = pwd
            };
            var jsonContent = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
             //send the post request with the serialized object
            var response = await httpClient.PostAsync(ACCOUNT_SERVICE_URI + "login", content);
            response.EnsureSuccessStatusCode(); //ensures that the Http status code indicates success
            var responseBody = await response.Content.ReadAsStringAsync(); //read the JSON content from the response body as a string
            var userData = JsonConvert.DeserializeObject<UserModel>(responseBody);
            return userData!;  
        }

        /// <summary>
        /// reset password.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<string> ResetPassword(string email)
        {
            var response = await httpClient.GetAsync(MEMBER_SERVICE_URI + "ResetPassword?email=" + email);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        /// <summary>
        /// checks if reset code expired.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<string> IsResetCodeExpired(string code)
        {
            var response = await httpClient.GetAsync(MEMBER_SERVICE_URI + "IsResetCodeExpired?code=" + code);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
        
        /// <summary>
        /// changes password.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> ChangePassword(RegisterModel model)
        {
            var request = MEMBER_SERVICE_URI +  "ChangePassword?pwd=" + model.ConfirmPwd + "&email=" + model.Email + "&code=" + model.Code;
            var response = await httpClient.GetAsync(request);
            var dynJson = await response.Content.ReadAsStringAsync();
            return dynJson;
        }

    }

    public interface IMembers
    {
        Task<MemberProfileBasicInfoModel> GetMemberBasicInfo();
        Task<MemberProfileContactInfoModel> GetMemberContactInfo();
        Task<List<MemberProfileEducationModel>> GetMemberEducationInfo();
        Task<string> RegisterUser(RegisterModel register);
        Task<UserModel> AuthenticateUser(string username, string pwd);

        Task<string> ResetPassword(string email);
        Task<string> IsResetCodeExpired(string code);
        Task<string> ChangePassword(RegisterModel model);

    }
}
