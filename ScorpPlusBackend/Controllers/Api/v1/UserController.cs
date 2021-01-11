using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScorpPlusBackend.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ScorpPlusBackend.Controllers.Api.v1
{
    /// <summary>
    /// User activities controller
    /// </summary>
    /// <code>api/v1/user</code>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : Controller
    {
        /// <summary>
        /// User context
        /// </summary>
        private readonly UserContext _userContext;

        /// <summary>
        /// User controller constructor
        /// </summary>
        /// <param name="userContext">User context</param>
        public UserController(UserContext userContext)
        {
            _userContext = userContext;
        }

        /// <summary>
        /// Get all users route
        /// </summary>
        /// <code>GET /all</code>
        /// <returns>Response status and list of users</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpGet("all")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _userContext.Users.Include(x => x.Role).Select(x => new
                {
                    id = x.Id,
                    username = x.Username,
                    email = x.Email,
                    role = x.Role.Code
                }).ToList();

                return Json(new
                {
                    status = true,
                    data = users
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Route for getting user by specified id
        /// </summary>
        /// <code>GET /{id}</code>
        /// <param name="id">User id</param>
        /// <returns>Response status and user object</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpGet("{id}")]
        public IActionResult GetUser([FromRoute] int id)
        {
            try
            {
                var user = _userContext.Users.Include(x => x.Role).Select(x => new
                {
                    id = x.Id,
                    username = x.Username,
                    email = x.Email,
                    role = x.Role.Code
                }).FirstOrDefault(x => x.id == id);
                if (user == null)
                    return NotFound(new
                    {
                        status = false,
                        message = "Such user doesn't exist"
                    });

                return Json(new
                {
                    status = true,
                    data = user
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Route for deleting user by specified id
        /// </summary>
        /// <code>DELETE /{id}</code>
        /// <param name="id">User id</param>
        /// <returns>Status of response</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            try
            {
                var user = _userContext.Users.FirstOrDefault(x => x.Id == id);
                if (user == null)
                    return NotFound(new {status = false, message = "There is no such user in DB"});
                _userContext.Users.Remove(user);
                await _userContext.SaveChangesAsync();
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Route for getting current user
        /// </summary>
        /// <code>GET /</code>
        /// <returns>Status of response and user object</returns>
        [HttpGet]
        public IActionResult GetCurrentUser()
        {
            try
            {
                var user = _userContext.Users.Include(x => x.Role).Select(x => new
                {
                    id = x.Id,
                    username = x.Username,
                    email = x.Email,
                    role = x.Role.Code
                }).FirstOrDefault(x => x.username == User.Identity.Name);
                if (user == null)
                    return NotFound(new {status = false, message = "There is no such user in DB"});
                return Json(new {status = true, data = user});
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Route for deleting current user
        /// </summary>
        /// <code>DELETE /</code>
        /// <returns>Status of response</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCurrentUser()
        {
            try
            {
                var user = _userContext.Users.FirstOrDefault(x => x.Username == User.Identity.Name);
                if (user == null)
                    return NotFound(new {status = false, message = "There is no such user in DB"});
                _userContext.Users.Remove(user!);
                await _userContext.SaveChangesAsync();
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }
    }
}