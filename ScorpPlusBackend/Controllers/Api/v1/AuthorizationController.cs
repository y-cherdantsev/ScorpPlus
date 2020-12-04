using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using ScorpPlusBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ScorpPlusBackend.Services;
using ScorpPlusBackend.Contexts;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ScorpPlusBackend.Controllers.Api.v1
{
    /// <summary>
    /// Authorization activities controller
    /// </summary>
    /// <code>api/v1/authorization</code>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthorizationController : Controller
    {
        /// <summary>
        /// User context
        /// </summary>
        private readonly UserContext _userContext;

        /// <summary>
        /// Notification service
        /// </summary>
        private readonly NotificationService _notificationService;

        /// <summary>
        /// Authorization controller constructor
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="notificationService">Notification service</param>
        public AuthorizationController(UserContext userContext, NotificationService notificationService)
        {
            _userContext = userContext;
            _notificationService = notificationService;
        }

        /// <summary>
        /// Registration route
        /// </summary>
        /// <code>POST /register</code>
        /// <param name="user">User object</param>
        /// <returns>Response with result status and created user</returns>
        /// \todo(Improve username and password validation)
        /// \todo(Add email validation)
        [HttpPost("register")]
        public async Task<IActionResult> PostRegister([FromBody] User user)
        {
            try
            {
                if (user.Username == null) return BadRequest(new {status = false, message = "Username field is empty"});
                if (user.Password == null) return BadRequest(new {status = false, message = "Password field is empty"});
                if (user.Username.Length < 6) return BadRequest(new {status = false, message = "Username too short"});
                if (user.Password.Length < 6) return BadRequest(new {status = false, message = "Password too short"});

                if (_userContext.Users.FirstOrDefault(x => x.Username == user.Username) != null)
                    return StatusCode((int) HttpStatusCode.Conflict,
                        new {status = false, message = "User with given username already exists"});

                var guestRole = _userContext.Roles.FirstOrDefault(x => x.Code == "guest");
                user.Role = guestRole;
                user.RoleId = guestRole!.Id;
                user.Password = EncryptPassword(user.Password);

                await _userContext.Users.AddAsync(user);
                await _userContext.SaveChangesAsync();
                user.Role!.Users = null;
                _notificationService.Notify("admin",
                    $"New user '{user.Username}' with '{user.Id}' id has been created;",
                    "New user ➕");
                return Json(new {status = true, data = user});
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Login route
        /// </summary>
        /// <code>POST /login</code>
        /// <param name="username">Username of an user</param>
        /// <param name="password">Password of an user</param>
        /// <returns>Response with result status and bearer token</returns>
        [HttpPost("login")]
        public IActionResult PostLogin(string username, string password)
        {
            password = EncryptPassword(password);

            var user = _userContext.Users.Include(x => x.Role)
                .FirstOrDefault(x => x.Username == username && x.Password == password);
            if (user == null)
                return BadRequest(new {status = false, message = "Invalid username or password"});

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Code)
            };
            var identity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

            var jwt = new JwtSecurityToken(
                Options.JwtOptions.Issuer,
                Options.JwtOptions.Audience,
                notBefore: DateTime.UtcNow,
                claims: identity.Claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(Options.JwtOptions.Lifetime)),
                signingCredentials: new SigningCredentials(Options.JwtOptions.SymmetricSecurityKey,
                    SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                status = true,
                data = new
                {
                    token = encodedJwt,
                    username = identity.Name
                }
            };

            _notificationService.Notify("admin", $"User '{user.Username}' with '{user.Id}' id entered the system;",
                "Authorization 🔐");
            return Json(response);
        }

        /// <summary>
        /// Logout route
        /// </summary>
        /// <code>POST /logout</code>
        /// <returns>Response with result status</returns>
        /// \todo(Implement logout)
        [HttpPost("logout")]
        public IActionResult PostLogout()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// SHA512 password encryption 
        /// </summary>
        /// <param name="password">Password that should be hashed</param>
        /// <returns>Hash value of the password</returns>
        private static string EncryptPassword(string password)
        {
            var sha256 = new SHA256Managed();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}