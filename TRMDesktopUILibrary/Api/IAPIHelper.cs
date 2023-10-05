using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUILibrary.Models;

namespace TRMDesktopUILibrary.Api
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUserModel> Authenticate(string username, string password);

        Task<T> GetAsync<T>(string url);

        Task GetLoggedInUserInfo(string authToken);

        void InitializeClient();

        void LogOffUser();

        HttpClient ApiClient { get; }
    }
}