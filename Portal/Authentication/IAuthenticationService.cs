using Portal.Models;
using TRMDesktopUILibrary.Models;

namespace Portal.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticatedUserModel> LogIn(AuthenticationUserModel userForAuthentication);
        Task LogOut();
    }
}