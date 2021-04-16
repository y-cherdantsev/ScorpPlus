using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScorpPlus.Contexts;

namespace ScorpPlusAdminDashboard.Pages
{
    public class Login : PageModel
    {
        private readonly UserContext _userContext;
        
        public Login(UserContext userContext)
        {
            _userContext = userContext;
        }
        
        public async Task<IActionResult>
            OnGetAsync(string username, string password)
        {
            try
            {
                await HttpContext
                    .SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch
            {
                // ignored
            }

            var user = _userContext.Users.Include(x => x.Role).FirstOrDefault(x => x.Username==username && x.Password == EncryptPassword(password));
            
            if(user == null)
                return LocalRedirect("/");
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user!.Username),
                new Claim(ClaimTypes.Role, user.Role.Code),
            };
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = Request.Host.Value
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return LocalRedirect("/");
        }
        
        /// <summary>
        /// Argon2i password encryption 
        /// </summary>
        /// <param name="password">Password that should be hashed</param>
        /// <returns>Hash value of the password</returns>
        private static string EncryptPassword(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var argon2I = new Argon2i(bytes)
            {
                Iterations = 64,
                MemorySize = 4096,
                DegreeOfParallelism = 16
            };
            var hash = argon2I.GetBytes(256);
            return Convert.ToBase64String(hash);
        }
    }
}