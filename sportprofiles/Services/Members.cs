using sportprofiles.Models.Members;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Web;
using Newtonsoft.Json;

namespace sportprofiles.Services
{
    public class Members : IMembers
    {
        //api account service URI
        private static readonly string ACCOUNT_SERVICE_URI = "https://www.sportprofiles.space/services/account/";
        private static readonly string MEMBER_SERVICE_URI = "https://www.sportprofiles.space/services/member/";
        private static readonly string COMMON_SERVICE_URI = "https://www.sportprofiles.space/services/common/";
        private static readonly HttpClient httpClient = new();

        public Members()
        {
        }

        /// <summary>
        /// Gets the member basic info.
        /// </summary>
        /// <returns>The member basic info.</returns>
        /// <param name="memberID">Member identifier.</param>
        public async Task<MemberProfileBasicInfoModel> GetMemberBasicInfo(string memberID)
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");

            httpClient.DefaultRequestHeaders.Authorization
                        = new AuthenticationHeaderValue("Bearer", jwtToken);
            var rsp = await httpClient.GetAsync(MEMBER_SERVICE_URI + "GetMemberGeneralInfoV2/" + memberID.ToString());
            var dynJson = await rsp.Content.ReadFromJsonAsync<MemberProfileBasicInfoModel>();
            dynJson!.memberProfileTitle = dynJson.CurrentStatus;
            dynJson.memProfileName = dynJson.FirstName + " " + dynJson.MiddleName + " " + dynJson.LastName;

            if (String.IsNullOrEmpty(dynJson.memProfileImage))
            {
                dynJson.memProfileImage = "https://www.sportprofiles.space/images/members/" + "default.png";
            }
            else
            {
                dynJson.memProfileImage = "https://www.sportprofiles.space/images/members/" + dynJson.memProfileImage;
            }

            dynJson.memProfileDOB = dynJson.DOBMonth + "/" + dynJson.DOBDay + "/" + dynJson.DOBYear;

            string str = "";
            if (dynJson.LookingForEmployment)
                str = "Employment, ";

            if (dynJson.LookingForNetworking)
                str += "Networking, ";

            if (dynJson.LookingForPartnership)
                str += "Partnership, ";

            if (dynJson.LookingForRecruitment)
                str += "Recruitment, ";

            dynJson.memProfileLookingFor = str.TrimEnd().TrimEnd(',');

            if (dynJson.InterestedInType == "8")
                dynJson.InterestedDesc = "Athletes and Sports";
            else if (dynJson.InterestedInType == "9")
                dynJson.InterestedDesc = "Athletic Trainers";
            else if (dynJson.InterestedInType == "39")
                dynJson.InterestedDesc = "Exercise Physiologists";
            else if (dynJson.InterestedInType == "43")
                dynJson.InterestedDesc = "Fitness Entrepreneur";
            else if (dynJson.InterestedInType == "90")
                dynJson.InterestedDesc = "Recreation Leader";
            else if (dynJson.InterestedInType == "101")
                dynJson.InterestedDesc = "Sports Announcers";
            else if (dynJson.InterestedInType == "102")
                dynJson.InterestedDesc = "Sports Coaches and Teachers";
            else if (dynJson.InterestedInType == "103")
                dynJson.InterestedDesc = "Sportscaster";

            httpClient.DefaultRequestHeaders.Authorization
                       = new AuthenticationHeaderValue("Bearer", jwtToken);
            var rsp2 = await httpClient.GetAsync(MEMBER_SERVICE_URI + "GetYoutubeChannel/" + memberID.ToString());
            var result2 = await rsp2.Content.ReadAsStringAsync();

            var channelID = "";

            if (result2 != null)
                channelID = result2;
            dynJson.ChannelID = channelID;

            return dynJson;
        }

        public async Task<MemberProfileContactInfoModel> GetMemberContactInfo(string memberID)
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await httpClient.GetAsync(MEMBER_SERVICE_URI + "GetMemberContactInfo/" + memberID);
            response.EnsureSuccessStatusCode(); //ensures that the Http status code indicates success
            var responseBody = await response.Content.ReadAsStringAsync(); //read the JSON content from the response body as a string
            var dynJson = JsonConvert.DeserializeObject<MemberProfileContactInfoModel>(responseBody);
            return dynJson!;
        }

        /// <summary>
        /// Gets the member education info.
        /// </summary>
        /// <returns>The member education info.</returns>
        /// <param name="memberID">Member identifier.</param>
        public async Task<List<MemberProfileEducationModel>> GetMemberEducationInfo(string memberID)
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await httpClient.GetAsync(MEMBER_SERVICE_URI + "GetMemberEducationInfo/" + memberID);
            response.EnsureSuccessStatusCode(); //ensures that the Http status code indicates success
            var responseBody = await response.Content.ReadAsStringAsync(); //read the JSON content from the response body as a string
            var res = JsonConvert.DeserializeObject<List<MemberProfileEducationModel>>(responseBody);

            for (var i = 0; i < res!.Count; i++)
            {
                if (res[i].SchoolName != null)
                {
                    if (res[i].SchoolName!.Length > 40)
                    {
                        res[i].SchoolName = res[i].SchoolName!.Substring(0, 40) + "...";
                    }
                }

                if (res[i].SchoolName != null)
                {
                    if (res[i].SchoolAddress!.Length > 48)
                    {
                        res[i].SchoolAddress = res[i].SchoolAddress!.Substring(0, 48) + "...";
                    }
                }

                if (res[i].SchoolImage != null)
                {
                    if (res[i].SchoolImage != "")
                    {
                        res[i].WebSite = res[i].SchoolImage;

                        if (res[i].WebSite!.IndexOf("http") == -1)
                        {
                            res[i].WebSite = "http://" + res[i].WebSite;
                        }

                        res[i].SchoolImage = "https://www.google.com/s2/favicons?domain=" + res[i].SchoolImage!.ToString();
                    }
                    else
                    {
                       res[i].SchoolImage = "https://www.sportprofiles.space/images/members/default.png";
                    }
                }

                if (res[i].YearClass == null)
                {
                    res[i].YearClass = "";
                }

                if (res[i].Major != null)
                {
                    res[i].Major = res[i].Major + " - " + res[i].YearClass;
                }
            }
            return res;
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
            var request = MEMBER_SERVICE_URI + "ChangePassword?pwd=" + model.ConfirmPwd + "&email=" + model.Email + "&code=" + model.Code;
            var response = await httpClient.GetAsync(request);
            var dynJson = await response.Content.ReadAsStringAsync();
            return dynJson;
        }

        /// <summary>
        /// add new school.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="body"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public async Task AddNewSchool(string memberId, MemberProfileEducationModel body, string jwt)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var jsonContent = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            await httpClient.PostAsync(MEMBER_SERVICE_URI + "AddMemberSchool/" + memberId, content);
        }

        /// <summary>
        /// update school.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="body"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public async Task UpdateSchool( string memberId, MemberProfileEducationModel body, string jwt)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var jsonContent = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            await httpClient.PutAsync(MEMBER_SERVICE_URI + "UpdateMemberSchool/" + memberId, content);
        }

        /// <summary>
        /// remove school.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="schoolId"></param>
        /// <param name="instType"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public async Task RemoveSchool(string memberId, string schoolId, string  instType, string jwt)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var request = MEMBER_SERVICE_URI + "RemoveMemberSchool?memberID=" + memberId + "&instID=" + schoolId + "&instType=" + instType;
            await httpClient.DeleteAsync(request);
       }

        /// <summary>
        /// saves member general information.
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="basicInfo"></param>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public async Task SaveMemberGeneralInfo(string memberID, MemberProfileBasicInfoModel basicInfo, string jwtToken)
        {
            if (basicInfo.InterestedDesc == "Athletes and Sports")
                basicInfo.InterestedInType = "8";
            else if (basicInfo.InterestedDesc == "Athletic Trainers")
                basicInfo.InterestedInType = "9";
            else if (basicInfo.InterestedDesc == "Exercise Physiologists")
                basicInfo.InterestedInType = "39";
            else if (basicInfo.InterestedDesc == "Fitness Entrepreneur")
                basicInfo.InterestedInType = "43";
            else if (basicInfo.InterestedDesc == "Recreation Leader")
                basicInfo.InterestedInType = "90";
            else if (basicInfo.InterestedDesc == "Sports Announcers")
                basicInfo.InterestedInType = "101";
            else if (basicInfo.InterestedDesc == "Sports Coaches and Teachers")
                basicInfo.InterestedInType = "102";
            else if (basicInfo.InterestedDesc == "Sportscaster")
                basicInfo.InterestedInType = "103";

            SaveMemberProfileGenInfoModel body = new SaveMemberProfileGenInfoModel();
            body.FirstName = basicInfo.FirstName;
            body.MiddleName = basicInfo.MiddleName;
            body.LastName = basicInfo.LastName;
            body.TitleDesc = basicInfo.TitleDesc;
            body.CurrentStatus = basicInfo.CurrentStatus;
            body.Sport = basicInfo.Sport;
            body.PreferredPosition = basicInfo.PreferredPosition;
            body.SecondaryPosition = basicInfo.SecondaryPosition;
            body.LeftRightHandFoot = basicInfo.LeftRightHandFoot;
            body.Height = basicInfo.Height;
            body.Weight = basicInfo.Weight;
            body.Sex = basicInfo.Sex;
            if (basicInfo.ShowSexInProfile)
                body.ShowSexInProfile = true;
            else
                body.ShowSexInProfile = false;
            body.InterestedInType = basicInfo.InterestedInType;
            body.InterestedDesc = basicInfo.InterestedDesc;
            body.Bio = basicInfo.Bio;
            body.DOBDay = basicInfo.DOBDay;
            body.DOBMonth = basicInfo.DOBMonth;
            body.DOBYear = basicInfo.DOBYear;
            if (basicInfo.LookingForEmployment)
              body.LookingForEmployment = true;
            else
                body.LookingForEmployment = false;

            if (basicInfo.LookingForNetworking)
                body.LookingForNetworking = true;
            else
                body.LookingForNetworking = false;

            if (basicInfo.LookingForPartnership)
                body.LookingForPartnership = true;
            else
                body.LookingForPartnership = false;

            if(basicInfo.LookingForRecruitment)
                body.LookingForRecruitment = true;
            else
                body.LookingForRecruitment = false;   

            var jsonContent = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
             httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            await httpClient.PostAsync(MEMBER_SERVICE_URI + "SaveMemberGeneralInfo/" + memberID, content);
        }

        /// <summary>
        /// saves member contact information.
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="contactInfo"></param>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public async Task SaveMemberContactInfo(string memberID, MemberProfileContactInfoModel contactInfo, string jwtToken)
        {
            var builder = new UriBuilder(MEMBER_SERVICE_URI + "SaveMemberContactInfoV2/" + memberID.ToString());
            var query = HttpUtility.ParseQueryString(builder.Query);
          
            query["Address"]=contactInfo.Address;
            query["CellPhone"]= contactInfo.CellPhone;
            query["City"] = contactInfo.City;
            query["Email"] = contactInfo.Email;
            query["OtherEmail"]= contactInfo.OtherEmail;
            query["Facebook"]= contactInfo.Facebook;
            query["HomePhone"]=  contactInfo.HomePhone;
            query["Instagram"]= contactInfo.Instagram;
            query["Neighborhood"] = contactInfo.Neighborhood;
            query["State"]= contactInfo.State;
            query["Twitter"]= contactInfo.Twitter;
            query["Website"]= contactInfo.Website;
            query["Zip"]= contactInfo.Zip;
            builder.Query = query.ToString();
            string url = builder.ToString();
            var content = new StringContent("Encoding.UTF8, application/json");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            await httpClient.PostAsync(url, content);
        }

        public async Task LogException(string msg, string stackTrace, string? jwt)
        {
            jwt = await SecureStorage.Default.GetAsync("AccessToken");
            msg = "MOBILE ERROR: " + msg; stackTrace = "MOBILE ERROR: " + stackTrace; 
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var requestUrl = COMMON_SERVICE_URI + "Logs?message=" + msg + "&stack=" + stackTrace;
            var requestContent = new StringContent("Encoding.UTF8, application/json");
            var response = await httpClient.GetAsync(requestUrl);
        }
    }

    public interface IMembers
    {
        Task<MemberProfileBasicInfoModel> GetMemberBasicInfo(string memberID);
        Task<MemberProfileContactInfoModel> GetMemberContactInfo(string memberID);
        Task<List<MemberProfileEducationModel>> GetMemberEducationInfo(string memberID);
        Task<string> RegisterUser(RegisterModel register);
        Task<UserModel> AuthenticateUser(string username, string pwd);
        Task<string> ResetPassword(string email);
        Task<string> IsResetCodeExpired(string code);
        Task<string> ChangePassword(RegisterModel model);
        Task RemoveSchool(string memberId, string schoolId, string instType, string jwt);
        Task AddNewSchool(string memberId, MemberProfileEducationModel body, string jwt);
        Task UpdateSchool(string memberId, MemberProfileEducationModel body, string jwt);
        Task SaveMemberGeneralInfo(string memberID, MemberProfileBasicInfoModel basicInfo, string jwtToken);
        Task SaveMemberContactInfo(string memberID, MemberProfileContactInfoModel contactInfo, string jwtToken);
        Task LogException(string msg, string stackTrace, string jwt);
    }
}
