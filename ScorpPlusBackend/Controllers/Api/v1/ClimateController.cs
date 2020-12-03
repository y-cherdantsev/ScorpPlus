using System;
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
    /// Climate activities controller
    /// </summary>
    /// <code>api/v1/climate</code>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClimateController : Controller
    {
        /// <summary>
        /// Climate context
        /// </summary>
        private readonly ClimateContext _climateContext;

        /// <summary>
        /// Climate controller constructor
        /// </summary>
        /// <param name="climateContext">Climate context</param>
        public ClimateController(ClimateContext climateContext)
        {
            _climateContext = climateContext;
        }

        /// <summary>
        /// Get all climate route
        /// </summary>
        /// <code>GET /all</code>
        /// <returns>Response status and list of climate</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpGet("all")]
        public IActionResult GetAllClimate()
        {
            try
            {
                // Get data
                var climateList = _climateContext.ClimateList
                    .Include(x => x.Device)
                    .Include(x => x.Room)
                    .ToList();


                // Remove redundant data
                climateList.ForEach(climate =>
                {
                    climate.Room.ClimateList = null;
                    climate.Device.ClimateList = null;
                    climate.Room.Devices = null;
                    climate.Device.Room.Devices = null;
                });

                return Json(new
                {
                    status = true,
                    data = climateList
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Route for getting climate by specified id
        /// </summary>
        /// <code>GET /{id}</code>
        /// <param name="id">Climate id</param>
        /// <returns>Response status and climate object</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpGet("{id}")]
        public IActionResult GetClimate([FromRoute] int id)
        {
            try
            {
                // Get data
                var climate = _climateContext.ClimateList
                    .Include(x => x.Device)
                    .Include(x => x.Room)
                    .FirstOrDefault(x => x.Id == id);

                if (climate == null)
                    return NotFound(new
                    {
                        status = false,
                        message = "Such climate doesn't exist"
                    });


                // Remove redundant data
                climate.Room.ClimateList = null;
                climate.Device.ClimateList = null;
                climate.Room.Devices = null;
                climate.Device.Room.Devices = null;

                return Json(new
                {
                    status = true,
                    data = climate
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Adding new climate route
        /// </summary>
        /// <code>POST /</code>
        /// <param name="climate">Climate object</param>
        /// <returns>Response with result status and created climate</returns>
        /// \todo(Check existence of device)
        [Authorize(Roles = "admin,manager")]
        [HttpPost]
        public async Task<IActionResult> PostClimate([FromBody] Climate climate)
        {
            try
            {
                await _climateContext.ClimateList.AddAsync(climate);
                await _climateContext.SaveChangesAsync();
                return Json(new {status = true, data = climate});
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Route for deleting climate by specified id
        /// </summary>
        /// <code>DELETE /{id}</code>
        /// <param name="id">Climate id</param>
        /// <returns>Status of response</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClimate([FromRoute] int id)
        {
            try
            {
                var climate = _climateContext.ClimateList.FirstOrDefault(x => x.Id == id);
                if (climate == null)
                    return NotFound(new {status = false, message = "There is no such climate in DB"});
                _climateContext.ClimateList.Remove(climate);
                await _climateContext.SaveChangesAsync();
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