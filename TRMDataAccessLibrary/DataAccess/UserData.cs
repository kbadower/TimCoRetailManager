﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void CreateUser(UserModel user)
        {
            _da.SaveData("dbo.spUser_Insert", new { user.Id, user.FirstName, user.LastName, user.EmailAddress }, "TRMData");
        }
    }
}
