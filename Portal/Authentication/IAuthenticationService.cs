using Portal.Models;

namespace Portal.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticatedUserModel> LogIn(AuthenticationUserModel userForAuthentication);
        Task LogOut();
    }
}