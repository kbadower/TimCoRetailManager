using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUI.Models;

namespace TRMDesktopUI.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUserModel> Authenticate(string username, string password);
        void InitializeClient();
    }
}