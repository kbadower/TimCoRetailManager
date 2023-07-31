using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUI.Models;
using TRMDesktopUILibrary.Models;

namespace TRMDesktopUILibrary.Api
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUserModel> Authenticate(string username, string password);

        Task GetLoggedInUserInfo(string authToken);

        void InitializeClient();
    }
}