using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TRMDesktopUILibrary.Models;

namespace TRMDesktopUILibrary.Api
{
    public class APIHelper : IAPIHelper
    {
        private HttpClient _apiClient;
        private ILoggedInUserModel _loggedInUserModel;
        private readonly IConfiguration _configuration;

        public APIHelper(ILoggedInUserModel loggedInUserModel, IConfiguration configuration)
        {
            _loggedInUserModel = loggedInUserModel;
            _configuration = configuration;
            InitializeClient();
        }

        public HttpClient ApiClient {
            get
            {
                return _apiClient;
            }
        }

        public void InitializeClient()
        {
            string api = _configuration.GetValue<string>("api");

            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(api);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticatedUserModel> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            using (HttpResponseMessage response = await _apiClient.PostAsync("/Token", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUserModel>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public void LogOffUser()
        {
            _apiClient.DefaultRequestHeaders.Clear();
        }

        public async Task GetLoggedInUserInfo(string authToken)
        {
            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient.DefaultRequestHeaders.Add("Authorization", $"bearer {authToken}");

            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("authorization", $"bearer {authToken}")
            });

            using (HttpResponseMessage response = await _apiClient.GetAsync("/api/User"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();
                    _loggedInUserModel.CreatedDate = result.CreatedDate;
                    _loggedInUserModel.FirstName = result.FirstName;
                    _loggedInUserModel.LastName = result.LastName;
                    _loggedInUserModel.EmailAddress = result.EmailAddress;
                    _loggedInUserModel.Token = authToken;
                    _loggedInUserModel.Id = result.Id;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
