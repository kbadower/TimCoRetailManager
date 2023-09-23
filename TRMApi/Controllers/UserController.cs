using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TRMDataAccessLibrary.Models;
using TRMDataAccessLibrary.DataAccess;
using TRMApi.Models;
using TRMApi.Data;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserData _data;
        private readonly ILogger<UserController> _logger;

        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IUserData data, ILogger<UserController> logger)
        {
            _context = context;
            _userManager = userManager;
            _data = data;
            _logger = logger;
        }

        [HttpGet]
        public UserModel GetById()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return _data.GetUserById(userId).First();
        }

        public record UserRegistrationModel (
            string FirstName,
            string LastName,
            string EmailAddress,
            string Password);

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegistrationModel user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.EmailAddress);
                if (existingUser == null)
                {
                    IdentityUser newUser = new()
                    {
                        Email = user.EmailAddress,
                        EmailConfirmed = true,
                        UserName = user.EmailAddress
                    };

                    IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);

                    if (result.Succeeded)
                    {
                        existingUser = await _userManager.FindByEmailAsync(user.EmailAddress);

                        if (existingUser == null)
                        {
                            return BadRequest();
                        }

                        UserModel u = new()
                        {
                            Id = existingUser.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            EmailAddress = user.EmailAddress
                        };

                        _data.CreateUser(u);
                        return Ok();
                    }
                }
            }

            return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            var output = new List<ApplicationUserModel>();

            var users = _context.Users.ToList();
            var userRoles = from ur in _context.UserRoles
                            join r in _context.Roles on ur.RoleId equals r.Id
                            select new {ur.UserId, ur.RoleId, r.Name};

            foreach (var user in users)
            {
                ApplicationUserModel u = new ApplicationUserModel
                {
                    Id = user.Id,
                    Email = user.Email
                };

                u.Roles = userRoles.Where(x => x.UserId == u.Id).ToDictionary(key => key.RoleId, val => val.Name);

                output.Add(u);
            }
            return output;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Admin/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
            var roles = _context.Roles.ToDictionary(x => x.Id, x => x.Name);
            return roles;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Admin/AddRole")]
        public async Task AddRole(UserRolePairModel userRolePair)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var adminUser = _data.GetUserById(userId).First();

            var user = await _userManager.FindByIdAsync(userRolePair.UserId);
            await _userManager.AddToRoleAsync(user, userRolePair.RoleName);

            _logger.LogInformation("{User} was added to {Role} role by {Admin}", user.Id, userRolePair.RoleName, adminUser.Id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Admin/RemoveRole")]
        public async Task RemoveRole(UserRolePairModel userRolePair)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var adminUser = _data.GetUserById(userId).First();

            var user = await _userManager.FindByIdAsync(userRolePair.UserId);
            await _userManager.RemoveFromRoleAsync(user, userRolePair.RoleName);

            _logger.LogInformation("{User} was removed from {Role} role by {Admin}", user.Id, userRolePair.RoleName, adminUser.Id);
        }
    }
}
