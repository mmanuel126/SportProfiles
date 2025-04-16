using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using sportprofiles.Models.Contacts;
using Newtonsoft.Json;

namespace sportprofiles.Services
{
    public class Contacts : IContacts
    {
        private readonly string CONTACT_SERVICE_URI = "https://www.sportprofiles.space/services/contact/";
        private static readonly string COMMON_SERVICE_URI = "https://www.sportprofiles.space/services/common/";
        private static readonly HttpClient httpClient = new();

        public Contacts()
        {
        }

        /// <summary>
        /// get my contacts.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ContactsModel>> GetMyContacts()
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            string memberID = Preferences.Default.Get("UserID", "");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await httpClient.GetAsync(CONTACT_SERVICE_URI + "GetMemberContacts?memberID=" + memberID + "&show=");
            response.EnsureSuccessStatusCode(); //ensures that the Http status code indicates success
            var responseBody = await response.Content.ReadAsStringAsync(); //read the JSON content from the response body as a string
            var dynJson = JsonConvert.DeserializeObject<List<ContactsModel>>(responseBody);

            for (var i = 0; i < dynJson!.Count; i++)
            {
                if (String.IsNullOrEmpty(dynJson[i].PicturePath))
                {
                    dynJson[i].PicturePath = "https://www.sportprofiles.space/images/members/default.png";
                }
                else
                {
                    dynJson[i].PicturePath = "https://www.sportprofiles.space/images/members/" + dynJson[i].PicturePath;
                }
                var st = dynJson[i].Params;
                if (String.IsNullOrEmpty(dynJson[i].Params))
                {
                    dynJson[i].Params = "Unknown title";
                }
                else if (String.IsNullOrWhiteSpace(dynJson[i].Params))
                {
                    dynJson[i].Params = "Unknown title";
                }

                if (dynJson[i].LabelText == "Add as Contact")
                {
                    dynJson[i].LabelText = "True"; dynJson[i].ParamsAV = "False";
                }
                else
                {
                    dynJson[i].LabelText = "False"; dynJson[i].ParamsAV = "True";
                }
            }
            return dynJson;
        }

        /// <summary>
        /// get my contacts.
        /// </summary>
        /// <returns></returns>
        public async Task<ObservableCollection<ContactsModel>> GetMyContactsList()
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            string memberID = Preferences.Default.Get("UserID", "");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await httpClient.GetAsync(CONTACT_SERVICE_URI + "GetMemberContacts?memberID=" + memberID + "&show=");
            response.EnsureSuccessStatusCode(); //ensures that the Http status code indicates success
            var responseBody = await response.Content.ReadAsStringAsync(); //read the JSON content from the response body as a string
            var dynJson = JsonConvert.DeserializeObject<ObservableCollection<ContactsModel>>(responseBody);

            for (var i = 0; i < dynJson!.Count; i++)
            {
                if (String.IsNullOrEmpty(dynJson[i].PicturePath))
                {
                    dynJson[i].PicturePath = "https://www.sportprofiles.space/images/members/default.png";
                }
                else
                {
                    dynJson[i].PicturePath = "https://www.sportprofiles.space/images/members/" + dynJson[i].PicturePath;
                }
                var st = dynJson[i].Params;
                if (String.IsNullOrEmpty(dynJson[i].Params))
                {
                    dynJson[i].Params = "Unknown title";
                }
                else if (String.IsNullOrWhiteSpace(dynJson[i].Params))
                {
                    dynJson[i].Params = "Unknown title";
                }

                if (dynJson[i].LabelText == "Add as Contact")
                {
                    dynJson[i].LabelText = "True"; dynJson[i].ParamsAV = "False";
                }
                else
                {
                    dynJson[i].LabelText = "False"; dynJson[i].ParamsAV = "True";
                }
            }
            return dynJson;
        }

        /// <summary>
        /// get my contact requests.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ContactsModel>> GetContactRequests()
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            string memberID = Preferences.Default.Get("UserID", "");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await httpClient.GetAsync(CONTACT_SERVICE_URI + "GetRequestsList?memberID=" + memberID);
            response.EnsureSuccessStatusCode(); //ensures that the Http status code indicates success
            var responseBody = await response.Content.ReadAsStringAsync(); //read the JSON content from the response body as a string
            var dynJson = JsonConvert.DeserializeObject<List<ContactsModel>>(responseBody);
            for (var i = 0; i < dynJson!.Count; i++)
            {
                if (String.IsNullOrEmpty(dynJson[i].PicturePath))
                {
                    dynJson[i].PicturePath = "https://www.sportprofiles.space/images/members/default.png";
                }
                else
                {
                    dynJson[i].PicturePath = "https://www.sportprofiles.space/images/members/" + dynJson[i].PicturePath;
                }
                var st = dynJson[i].Params;
                if (String.IsNullOrEmpty(dynJson[i].Params))
                {
                    dynJson[i].Params = "Unknown title";
                }
                else if (String.IsNullOrWhiteSpace(dynJson[i].Params))
                {
                    dynJson[i].Params = "Unknown title";
                }

                if (dynJson[i].LabelText == "Add as Contact")
                {
                    dynJson[i].LabelText = "True"; dynJson[i].ParamsAV = "False";
                }
                else
                {
                    dynJson[i].LabelText = "False"; dynJson[i].ParamsAV = "True";
                }
            }
            return dynJson;
        }

        /// <summary>
        /// get search result.
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public async Task<ObservableCollection<ContactsModel>> GetSearchResult(string searchText)
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            string memberID = Preferences.Default.Get("UserID", "");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await httpClient.GetAsync(CONTACT_SERVICE_URI + "SearchResults?memberID=" + memberID + "&searchText=" + searchText);
            response.EnsureSuccessStatusCode(); //ensures that the Http status code indicates success
            var responseBody = await response.Content.ReadAsStringAsync(); //read the JSON content from the response body as a string
            var dynJson = JsonConvert.DeserializeObject<ObservableCollection<ContactsModel>>(responseBody);

            for (var i = 0; i < dynJson!.Count; i++)
            {
                if (String.IsNullOrEmpty(dynJson[i].PicturePath))
                {
                    dynJson[i].PicturePath = "https://www.sportprofiles.space/images/members/default.png";
                }
                else
                {
                    dynJson[i].PicturePath = "https://www.sportprofiles.space/images/members/" + dynJson[i].PicturePath;
                }
                var st = dynJson[i].Params;
                if (String.IsNullOrEmpty(dynJson[i].Params))
                {
                    dynJson[i].TitleDesc = "Unknown title";
                }
                else if (String.IsNullOrWhiteSpace(dynJson[i].Params))
                {
                    dynJson[i].TitleDesc = "Unknown title";
                }
                else
                {
                    dynJson[i].TitleDesc = dynJson[i].Params;
                }

                if (dynJson[i].LabelText == "Add as Contact")
                {
                    dynJson[i].LabelText = "True"; dynJson[i].ParamsAV = "False";
                }
                else
                {
                    dynJson[i].LabelText = "False"; dynJson[i].ParamsAV = "True";
                }
            }
            return dynJson;
        }

        /// <summary>
        /// delete contact.
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="contactID"></param>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public async Task DeleteContact(string memberID, string contactID, string jwtToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var request = CONTACT_SERVICE_URI + "DeleteContact?memberID=" + memberID + "&contactID=" + contactID;
            await httpClient.DeleteAsync(request);
        }

        /// <summary>
        /// accept request.
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="contactID"></param>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public async Task AcceptRequest(string memberID, string contactID, string jwtToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var request = CONTACT_SERVICE_URI + "AcceptRequest?memberID=" + memberID + "&contactID=" + contactID;
            await httpClient.PutAsync(request, null);
        }

        /// <summary>
        /// reject request.
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="contactID"></param>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public async Task RejectRequest(string memberID, string contactID, string jwtToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var request = CONTACT_SERVICE_URI + "RejectRequest?memberID=" + memberID + "&contactID=" + contactID;
            var res = await httpClient.PutAsync(request, null);
        }

        /// <summary>
        /// logs error message and stackstrace to API services
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="stackTrace"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public async Task LogException(string msg, string stackTrace, string? jwt)
        {
            jwt = await SecureStorage.Default.GetAsync("AccessToken");
            msg = "MOBILE ERROR: " + msg; stackTrace = "MOBILE ERROR: " + stackTrace;
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var requestUrl = COMMON_SERVICE_URI + "Logs?message=" + msg + "&stack=" + stackTrace;
            var requestContent = new StringContent("Encoding.UTF8, application/json");
            var response = await httpClient.PostAsync(requestUrl, requestContent);
        }
    }

    public interface IContacts
    {
        Task<List<ContactsModel>> GetMyContacts();
        Task<ObservableCollection<ContactsModel>> GetMyContactsList();
        Task<List<ContactsModel>> GetContactRequests();
        Task<ObservableCollection<ContactsModel>> GetSearchResult(string searchText);
        Task DeleteContact(string memberid, string contactID, string jwtToken);
        Task AcceptRequest(string memberid, string contactID, string jwtToken);
        Task RejectRequest(string memberid, string contactID, string jwtToken);
        Task LogException(string msg, string stackTrace, string jwtToken);
    }
}
