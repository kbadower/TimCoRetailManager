using System.Collections.Generic;
using TRMDataAccessLibrary.Models;

namespace TRMDataAccessLibrary.DataAccess
{
    public interface IUserData
    {
        List<UserModel> GetUserById(string Id);
    }
}