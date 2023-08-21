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
    public class UserData
    {
        private readonly IConfiguration _configuration;

        public UserData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<UserModel> GetUserById(string Id)
        {
            SqlDataAccess _da = new SqlDataAccess(_configuration);
            var output =  _da.LoadData<UserModel, dynamic>("dbo.spUser_Get", new { Id }, "TRMData");
            return output;
        }
    }
}
