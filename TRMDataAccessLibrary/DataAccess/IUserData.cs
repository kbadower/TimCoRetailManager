using System.Collections.Generic;
using TRMDataAccessLibrary.Models;

namespace TRMDataAccessLibrary.DataAccess
{
    public interface IUserData
    {
        void CreateUser(UserModel user);
        List<UserModel> GetUserById(string Id);
    }
}