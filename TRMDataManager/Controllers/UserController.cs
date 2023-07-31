using Microsoft.AspNet.Identity;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using TRMDataAccessLibrary.DataAccess;
using TRMDataAccessLibrary.Models;

namespace TRMDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        [HttpGet]
        public UserModel GetById()
        {
            var userId = RequestContext.Principal.Identity.GetUserId();
            UserData userData = new UserData();

            var output = userData.GetUserById(userId).First();
            return output;
        }
    }
}
