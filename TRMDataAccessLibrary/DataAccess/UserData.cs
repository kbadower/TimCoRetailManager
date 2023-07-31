using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataAccessLibrary.Internal.DataAccess;
using TRMDataAccessLibrary.Models;

namespace TRMDataAccessLibrary.DataAccess
{
    public class UserData
    {
        SqlDataAccess _da = new SqlDataAccess();

        public List<UserModel> GetUserById(string Id)
        {
            var output =  _da.LoadData<UserModel, dynamic>("dbo.spGetUser", new { Id }, "TRMData");
            return output;
        }
    }
}
