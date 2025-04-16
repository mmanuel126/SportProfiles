using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using sportprofiles.Models;

namespace sportprofiles.Services
{
    /// <summary>
    /// implementation of the Settings service class deriving from the ISettings interface
    /// </summary>
    public class Settings: ISettings
    {
        //http api url paths
        private static readonly string SETTING_SERVICE_URI = "https://www.sportprofiles.space/services/setting/";
        private static readonly string COMMON_SERVICE_URI = "https://www.sportprofiles.space/services/common/";
        private static readonly string MEMBERS_SERVICE_URI = "https://www.sportprofiles.space/services/member/";
        private static readonly HttpClient httpClient = new();
       
        /// <summary>
        /// Save member name info.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="firstName"></param>
        /// <param name="middleName"></param>
        /// <param name="lastName"></param>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public async Task SaveMemberNameInfo(string memberId, string firstName, string middleName, string lastName, string jwtToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var content = new StringContent("Encoding.UTF8, application/json");
            var reqUrl = "SaveMemberNameInfo/" + memberId + "?memberID=" + memberId + "&fName=" + firstName + "&mName=" + middleName + "&lName=" + lastName;
            await httpClient.PutAsync(SETTING_SERVICE_URI + reqUrl, content);
        }

        /// <summary>
        /// save password information.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="password"></param>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public async Task SavePasswordInfo(string memberId, string password, string jwtToken)
        {
            var  body = new 
            {
                memberID = memberId,
                pwd = password
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var jsonContent = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
             //send the post request with the serialized object
            await httpClient.PutAsync(SETTING_SERVICE_URI + "SavePasswordInfo", content);
        }

        /// <summary>
        /// Upload image.
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="content"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public async Task UploadImage(string memberID, MultipartFormDataContent content, string jwt)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var requestUrl = MEMBERS_SERVICE_URI + "UploadProfilePhoto/" + memberID;
            var response = await httpClient.PostAsync(requestUrl, content);
        }

        /// <summary>
        /// Get member name info.
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public async Task<List<AccountSettingsInfoModel>> GetMemberNameInfo(string memberID,string jwtToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await httpClient.GetAsync(SETTING_SERVICE_URI + "GetMemberNameInfo/" + memberID);
            response.EnsureSuccessStatusCode(); //ensures that the Http status code indicates success
            var responseBody = await response.Content.ReadAsStringAsync(); //read the JSON content from the response body as a string
            var userData = JsonConvert.DeserializeObject<List<AccountSettingsInfoModel>>(responseBody);
            return userData!;  
        }

        /// <summary>
        /// save security questions.
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public async Task SaveSecurityQuestionInfo(string memberID, string question, string answer, string jwt)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var requestUrl = SETTING_SERVICE_URI + "SaveSecurityQuestionInfo/" + memberID + "?questionID=" + question + "&answer=" + answer;
            var content = new StringContent("Encoding.UTF8, application/json");
            await httpClient.PutAsync(requestUrl, content);
        }

        /// <summary>
        /// Get member notifications.
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public async Task<NotificationsSettingModel> GetMemberNotifications(string memberID, string jwt)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var response = await httpClient.GetAsync(SETTING_SERVICE_URI + "GetMemberNotifications/" + memberID);
            response.EnsureSuccessStatusCode(); //ensures that the Http status code indicates success
            var responseBody = await response.Content.ReadAsStringAsync(); //read the JSON content from the response body as a string
            var userData = JsonConvert.DeserializeObject<List<NotificationsSettingModel>>(responseBody);
             if (userData != null)
                if (userData.Count != 0)
                    return userData[0];
                else
                    return null!;
            else
                return null!;
        }

        /// <summary>
        /// save notifications settings.
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="body"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public async Task SaveNotificationSettings(string memberID, NotificationsSettingModel body, string jwt)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var jsonContent = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
             //send the post request with the serialized object
            await httpClient.PutAsync(SETTING_SERVICE_URI + "SaveNotificationSettings/" + memberID, content);
        }

        /// <summary>
        /// deactivate account.
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="reason"></param>
        /// <param name="explanation"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public async Task DeactivateAccount(string memberID, string reason, string explanation, string jwt)
        {
            bool futureEmail = false;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            string url = "DeactivateAccount/" + memberID + "?reason=" + reason + "&explanation=" + explanation + "&futureEmail=" + futureEmail;
            var requestUrl = SETTING_SERVICE_URI + url;
            var content = new StringContent("Encoding.UTF8, application/json");
            await httpClient.PutAsync(requestUrl, content);
        }

        /// <summary>
        /// Get profile settings.
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public async Task<PrivacySettingsModel> GetProfileSettings(string memberID, string? jwt)
        {
            jwt = await SecureStorage.Default.GetAsync("AccessToken");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var response = await httpClient.GetAsync(SETTING_SERVICE_URI + "GetProfileSettings/" + memberID);
            response.EnsureSuccessStatusCode(); //ensures that the Http status code indicates success
            var responseBody = await response.Content.ReadAsStringAsync(); //read the JSON content from the response body as a string
            var userData = JsonConvert.DeserializeObject<List<PrivacySettingsModel>>(responseBody);
            if (userData != null)
                if (userData.Count != 0)
                    return userData[0];
                else
                    return null!;
            else
                return null!;
        }

        /// <summary>
        /// Save profile settings..
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="body"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public async Task SaveProfileSettings(string memberID, PrivacySettingsModel body, string jwt)
        {
            body.MemberID = memberID; body.ID = memberID; body.Email = ""; body.Visibility = "1";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var jsonContent = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
             //send the post request with the serialized object
            await httpClient.PutAsync(SETTING_SERVICE_URI + "SaveProfileSettings/" + memberID, content);
        }

        /// <summary>
        /// save search settings.
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="body"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public async Task SaveSearchSettings(string memberID, PrivacySettingsModel body,string jwt)
        {
            var url = "SavePrivacySearchSettings/" + memberID + "?visibility=" + body.Visibility;
            url = url + "&viewProfilePicture=" + body.ViewProfilePicture + "&viewFriendsList=" + body.ViewFriendsList;
            url = url + "&viewLinkToRequestAddingYouAsFriend=" + body.ViewLinksToRequestAddingYouAsFriend;
            url = url + "&viewLinkToSendYouMsg=" + body.ViewLinkTSendYouMsg;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var requestUrl = SETTING_SERVICE_URI + url;
            var content = new StringContent("Encoding.UTF8, application/json");
            await httpClient.PutAsync(requestUrl, content);
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

    /// <summary>
    /// Interfaces for the Settings class
    /// </summary>
    public interface ISettings
    {
        Task UploadImage(string memberID, MultipartFormDataContent content, string jwt);
        Task SaveMemberNameInfo(string memberId, string firstName, string middleName, string lastName, string jwtToken);
        Task<List<AccountSettingsInfoModel>> GetMemberNameInfo(string memberID, string jwtToken);
        Task SaveSecurityQuestionInfo(string memberID, string question, string answer, string jwt);
        Task SaveNotificationSettings(string memberID, NotificationsSettingModel body, string jwt);
        Task<PrivacySettingsModel> GetProfileSettings(string memberID, string jwt);
        Task SaveProfileSettings(string memberID, PrivacySettingsModel body, string jwt);
        Task<NotificationsSettingModel> GetMemberNotifications(string memberID, string jwt);
        Task SavePasswordInfo(string memberId, string password, string jwtToken);
        Task DeactivateAccount(string memberID, string reason, string explanation, string jwt);
        Task LogException(string msg, string stackTrace, string jwtToken);
    }

}
