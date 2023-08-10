using System.Collections.Generic;
using System.Threading.Tasks;
using TRMDesktopUILibrary.Models;

namespace TRMDesktopUILibrary.Api
{
    public interface IUserEndpoint
    {
        Task AddUserToRole(string userId, string roleName);
        Task<Dictionary<string, string>> GetAllRoles();
        Task<List<UserModel>> GetAllUsers();
        Task RemoveUserFromRole(string userId, string roleName);
    }
}