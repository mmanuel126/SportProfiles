using System.Net.Http.Headers;
using Newtonsoft.Json;
using sportprofiles.Models;

namespace sportprofiles.Services
{
    public class Commons : ICommons
    {
        private readonly string COMMON_SERVICE_URI = "https://www.sportprofiles.space/services/common/";
        private static readonly HttpClient httpClient = new();

        public Commons()
        {
        }

        /// <summary>
        /// Gets the recent news.
        /// </summary>
        /// <returns>The recent news.</returns>
        public Task<List<RecentNewsModel>> GetRecentNews()
        {
            return GetNews();
        }

        async Task<List<RecentNewsModel>> GetNews()
        {
            string? jwtToken = await SecureStorage.Default.GetAsync("AccessToken");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await httpClient.GetAsync(COMMON_SERVICE_URI + "GetRecentNews");
            response.EnsureSuccessStatusCode(); //ensures that the Http status code indicates success
            var responseBody = await response.Content.ReadAsStringAsync(); //read the JSON content from the response body as a string
            var userData = JsonConvert.DeserializeObject<List<RecentNewsModel>>(responseBody);
            return userData!;
        }

        /// <summary>
        /// get schools by state.
        /// </summary>
        /// <param name="strState"></param>
        /// <param name="instType"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public async Task<List<SchoolsByStateModel>> GetSchoolsByState(string strState, string instType, string jwt)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var response = await httpClient.GetAsync(COMMON_SERVICE_URI + "GetSchoolByState?state=" + strState + "&institutionType=" + instType);
            response.EnsureSuccessStatusCode(); //ensures that the Http status code indicates success
            var responseBody = await response.Content.ReadAsStringAsync(); //read the JSON content from the response body as a string
            var data = JsonConvert.DeserializeObject<List<SchoolsByStateModel>>(responseBody);
            return data!;
        }

        /// <summary>
        /// get states.
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public async Task<List<StatesModel>> GetStates(string jwtToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await httpClient.GetAsync(COMMON_SERVICE_URI + "GetStates");
            response.EnsureSuccessStatusCode(); //ensures that the Http status code indicates success
            var responseBody = await response.Content.ReadAsStringAsync(); //read the JSON content from the response body as a string
            var data = JsonConvert.DeserializeObject<List<StatesModel>>(responseBody);
            return data!;
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

    public interface ICommons
    {
        Task<List<RecentNewsModel>> GetRecentNews();
        Task<List<SchoolsByStateModel>> GetSchoolsByState(string strState, string instType, string jwt);
        Task<List<StatesModel>> GetStates(string jwtToken);
        Task LogException(string msg, string stackTrace, string jwtToken);
    }
}
