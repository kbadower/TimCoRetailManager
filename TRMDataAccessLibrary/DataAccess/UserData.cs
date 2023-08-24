using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataAccessLibrary.Internal.DataAccess;
using TRMDataAccessLibrary.Models;

namespace TRMDataAccessLibrary.DataAccess
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _da;

        public UserData(ISqlDataAccess da)
        {
            _da = da;
        }

        public List<UserModel> GetUserById(string Id)
        {
            var output = _da.LoadData<UserModel, dynamic>("dbo.spUser_Get", new { Id }, "TRMData");
            return output;
        }
    }
}
