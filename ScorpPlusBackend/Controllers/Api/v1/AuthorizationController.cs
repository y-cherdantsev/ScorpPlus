using System;
using System.Net;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ScorpPlusBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ScorpPlusBackend.Contexts;
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
        /// Authorization controller constructor
        /// </summary>
        /// <param name="userContext">User context</param>
        public AuthorizationController(UserContext userContext)
        {
            _userContext = userContext;
        }

        /// <summary>
        /// Registration route
        /// </summary>
        /// <code>POST /register</code>
        /// <param name="username">Username of an user</param>
        /// <param name="password">Password of an user</param>
        /// <returns>Response with result status</returns>
        [HttpPost("register")]
        public async Task<IActionResult> PostRegister(string username, string password)
        {
            if (username == null) return BadRequest(new {status = false, message = "Username field is empty"});
            if (password == null) return BadRequest(new {status = false, message = "Password field is empty"});
            if (username.Length < 6) return BadRequest(new {status = false, message = "Username too short"});
            if (password.Length < 6) return BadRequest(new {status = false, message = "Password too short"});

            if (_userContext.Users.FirstOrDefault(x => x.Username == username) != null)
                return StatusCode((int) HttpStatusCode.Conflict,
                    new {status = false, message = "User with given username already exists"});

            var generalRole = _userContext.Roles.FirstOrDefault(x => x.Code == "guest");
            var user = new User
            {
                Username = username,
                Password = password,
                RoleId = generalRole!.Id
            };

            try
            {
                await _userContext.Users.AddAsync(user);
                await _userContext.SaveChangesAsync();
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

            return Json(response);
        }
    }
}