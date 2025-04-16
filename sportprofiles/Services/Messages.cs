using Newtonsoft.Json;
using sportprofiles.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace sportprofiles.Services
{
    public class Messages : IMessages
    {
        static readonly string MESSAGE_SERVICE_URI = "https://www.sportprofiles.space/services/message/";
        private static readonly string MEMBER_SERVICE_URI = "https://www.sportprofiles.space/services/member/";
        private static readonly string COMMON_SERVICE_URI = "https://www.sportprofiles.space/services/common/";
        private static readonly HttpClient httpClient = new();

        public Messages()
        {
        }

        #region public post method implementations...

        /// <summary>
        /// Gets the recent posts.
        /// </summary>
        /// <returns>The recent posts.</returns>
        public async Task<List<RecentPostsModel>> GetRecentPosts()
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            string memberID = Preferences.Default.Get("UserID", "");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await httpClient.GetAsync(MEMBER_SERVICE_URI + "getRecentPosts/" + memberID.ToString());
            response.EnsureSuccessStatusCode(); //ensures that the Http status code indicates success
            var responseBody = await response.Content.ReadAsStringAsync(); //read the JSON content from the response body as a string
            var dynJson = JsonConvert.DeserializeObject<List<RecentPostsModel>>(responseBody);

            List<RecentPostsModel> lst = new List<RecentPostsModel>();

            for (int i = 0; i < dynJson!.Count; i++)
            {
                RecentPostsModel mp = new RecentPostsModel();
                List<RecentPostChildModel> l = await GetChildPosts(Convert.ToInt32(dynJson[i].PostID));
                mp.ReplyCount = l.Count;

                if (l.Count != 0)
                {
                    mp.ChildItems = new List<RecentPostsModel>();
                    foreach (var ls in l)
                    {
                        var child = new RecentPostsModel();
                        child.MemberName = ls.MemberName;
                        child.PostID = ls.PostResponseID.ToString();
                        child.Description = ls.Description;
                        child.PicturePath = ls.PicturePath;
                        child.DatePosted = ls.DateResponded;
                        child.IsSelected = true;
                        mp.ChildItems.Add(child);
                    }
                }

                lst.Add(mp);
            }
            return dynJson!;
        }

        /// <summary>
        /// Gets the child posts.
        /// </summary>
        /// <returns>The child posts.</returns>
        /// <param name="postID">Post identifier.</param>
        public async Task<List<RecentPostChildModel>> GetChildPosts(int postID)
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            string memberID = Preferences.Default.Get("UserID", "");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await httpClient.GetAsync(MEMBER_SERVICE_URI + "getRecentPostResponses/" + postID.ToString());
            response.EnsureSuccessStatusCode(); //ensures that the Http status code indicates success
            var responseBody = await response.Content.ReadAsStringAsync(); //read the JSON content from the response body as a string
            var dynJson = JsonConvert.DeserializeObject<List<RecentPostChildModel>>(responseBody);
            return dynJson!;
        }

        /// <summary>
        /// Saves the posts.
        /// </summary>
        /// <returns>The posts.</returns>
        /// <param name="memberID">Member identifier.</param>
        /// <param name="postID">Post identifier.</param>
        /// <param name="postMsg">Post message.</param>
        public async Task<string> SavePosts(int memberID, int postID, string postMsg)
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            string resource = "SavePosts/" + memberID + "/" + postID + "?postMsg=" + postMsg;
            var response = await httpClient.PostAsync(MEMBER_SERVICE_URI + resource, null);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        #endregion

        #region private messaging method implementations

         /// <summary>
        /// Sends a message 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="toWho"></param>
        /// <param name="sub"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<string> SendMessage(string from, string toWho, string sub, string msg)
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            string request = "CreateMessage?from=" + from + "&to=" + toWho + "&subject=" + sub + "&body=" + msg;
            var response = await httpClient.PostAsync(MESSAGE_SERVICE_URI + request, null);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        /// <summary>
        /// Get members messages
        /// </summary>
        /// <param name="type"></param>
        /// <param name="showType"></param>
        /// <returns></returns>
        public async Task<List<MessageInfoModel>> GetMemberMessages(string type, string showType)
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            string memberID = Preferences.Default.Get("UserID", "");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await httpClient.GetAsync(MESSAGE_SERVICE_URI + "GetMemberMessages/" + memberID + "?type=" + type + "&showType=" + showType );
            response.EnsureSuccessStatusCode(); //ensures that the Http status code indicates success
            var responseBody = await response.Content.ReadAsStringAsync(); //read the JSON content from the response body as a string
            List<MessageInfoModel>  dynJson = JsonConvert.DeserializeObject<List<MessageInfoModel>>(responseBody)!;

            for (int i = 0; i < dynJson.Count; i++)
            {
                if (string.IsNullOrEmpty(dynJson[i].SenderImage))
                {
                    dynJson[i].SenderImage = "https://www.sportprofiles.space/images/members/default.png";
                }
                else
                {
                    dynJson[i].SenderImage = "https://www.sportprofiles.space/images/members/" + dynJson[i].SenderImage;
                }

                if (string.IsNullOrEmpty(dynJson[i].SenderTitle))
                {
                    dynJson[i].SenderTitle = "Unknown Title";
                }

            }
            return dynJson;
        }


        /// <summary>
        /// Get message info by ID
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public async Task<List<MessageDetails>> GetMessageInfoByID(string msgID, string folder)
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await httpClient.GetAsync(MESSAGE_SERVICE_URI + "GetMessageInfoByID/" + msgID + "?folder=" + folder);
            var dynJson = await response.Content.ReadFromJsonAsync <List<MessageDetails>>();
        
            for (int i = 0; i < dynJson!.Count; i++)
            {
                if (string.IsNullOrEmpty(dynJson[i].SenderPicture))
                {
                    dynJson[i].SenderPicture = "https://www.sportprofiles.space/images/members/default.png";
                }
                else
                {
                    dynJson[i].SenderPicture = "https://www.sportprofiles.space/images/members/"  + dynJson[i].SenderPicture;
                }
            }
            return dynJson;
        }

        /// <summary>
        /// Toggle message state
        /// </summary>
        /// <param name="state"></param>
        /// <param name="msgID"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public async Task<string> ToggleMessageState(string state, string msgID, string folder)
        {
             string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            string request = "ToggleMessageState?status=" + state + "&msgID=" + msgID + "&folder=" + folder;
            var response = await httpClient.PutAsync(MESSAGE_SERVICE_URI + request, null);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        /// <summary>
        /// Delete message
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public async Task<string> DeleteMessage(string msgID, string folder)
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            string request = "DeleteMessage/" + msgID ;
            var response = await httpClient.DeleteAsync(MESSAGE_SERVICE_URI + request);
            var result = await response.Content.ReadAsStringAsync();
            return result;
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

        #endregion

    }

    public interface IMessages
    {
        //public posts methods
        Task<List<RecentPostsModel>> GetRecentPosts();
        Task<List<RecentPostChildModel>> GetChildPosts(int postID);
        Task<string> SavePosts(int memberID, int postID, string postMsg);

        //private messaging methods
        Task<string> SendMessage(string from, string toWho, string sub, string msg);
        Task<List<MessageInfoModel>> GetMemberMessages(string type, string showType);
        Task<List<MessageDetails>> GetMessageInfoByID(string msgID, string folder);
        Task<string> ToggleMessageState(string state, string msgID, string folder);
        Task<string> DeleteMessage(string msgID, string folder);
        Task LogException(string msg, string stackTrace, string jwtToken);
    }

}