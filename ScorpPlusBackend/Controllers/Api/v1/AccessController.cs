﻿using System;
using System.Linq;
using System.Threading.Tasks;
using ScorpPlusBackend.Models;
using Microsoft.AspNetCore.Mvc;
using ScorpPlusBackend.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ScorpPlusBackend.Controllers.Api.v1
{
    /// <summary>
    /// Access activities controller
    /// </summary>
    /// <code>api/v1/access</code>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccessController : Controller
    {
        /// <summary>
        /// Access context
        /// </summary>
        private readonly AccessContext _accessContext;

        /// <summary>
        /// Access controller constructor
        /// </summary>
        /// <param name="accessContext">Access context</param>
        public AccessController(AccessContext accessContext)
        {
            _accessContext = accessContext;
        }

        /// <summary>
        /// Get all access rights route
        /// </summary>
        /// <code>GET /all</code>
        /// <returns>Response status and list of access rights</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpGet("all")]
        public IActionResult GetAllAccesses()
        {
            try
            {
                // Get data
                var accesses = _accessContext.Accesses
                    .Include(x => x.Employee)
                    .Include(x => x.Room)
                    .ToList();


                // Remove redundant data
                accesses.ForEach(access =>
                {
                    access.Employee.Accesses = null;
                    access.Room.Accesses = null;
                });

                return Json(new
                {
                    status = true,
                    data = accesses
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Route for getting access by specified id
        /// </summary>
        /// <code>GET /{id}</code>
        /// <param name="id">Access id</param>
        /// <returns>Response status and access object</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpGet("{id}")]
        public IActionResult GetAccess([FromRoute] int id)
        {
            try
            {
                // Get data
                var access = _accessContext.Accesses
                    .Include(x => x.Employee)
                    .Include(x => x.Room)
                    .FirstOrDefault(x => x.Id == id);

                if (access == null)
                    return NotFound(new
                    {
                        status = false,
                        message = "Such access doesn't exist"
                    });

                // Remove redundant data
                access.Employee.Accesses = null;
                access.Room.Accesses = null;

                return Json(new
                {
                    status = true,
                    data = access
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Adding new access route
        /// </summary>
        /// <code>POST /</code>
        /// <param name="access">Access object</param>
        /// <returns>Response with result status and created access</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpPost]
        public async Task<IActionResult> PostAccess([FromBody] Access access)
        {
            try
            {
                await _accessContext.Accesses.AddAsync(access);
                await _accessContext.SaveChangesAsync();
                return Json(new {status = true, data = access});
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Route for deleting access by specified id
        /// </summary>
        /// <code>DELETE /{id}</code>
        /// <param name="id">Access id</param>
        /// <returns>Status of response</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccess([FromRoute] int id)
        {
            try
            {
                var access = _accessContext.Accesses.FirstOrDefault(x => x.Id == id);
                if (access == null)
                    return NotFound(new {status = false, message = "There is no such access in DB"});
                _accessContext.Accesses.Remove(access);
                await _accessContext.SaveChangesAsync();
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