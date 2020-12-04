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
        /// Get all climate histories route
        /// </summary>
        /// <code>GET /climateHistory/all</code>
        /// <returns>Response status and list of climate histories</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpGet("climateHistory/all")]
        public IActionResult GetAllClimateHistories()
        {
            try
            {
                // Get data
                var climateHistories = _climateContext.ClimateHistories
                    .Include(x => x.Device)
                    .Include(x => x.Room)
                    .ToList();


                // Remove redundant data
                climateHistories.ForEach(climateHistory =>
                {
                    climateHistory.Room.ClimateHistories = null;
                    climateHistory.Device.ClimateHistories = null;
                    climateHistory.Room.Devices = null;
                    climateHistory.Device.Room.Devices = null;
                });

                return Json(new
                {
                    status = true,
                    data = climateHistories
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Route for getting climate history by specified id
        /// </summary>
        /// <code>GET /climateHistory/{id}</code>
        /// <param name="id">Climate history id</param>
        /// <returns>Response status and climate history object</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpGet("climateHistory/{id}")]
        public IActionResult GetClimateHistory([FromRoute] int id)
        {
            try
            {
                // Get data
                var climateHistory = _climateContext.ClimateHistories
                    .Include(x => x.Device)
                    .Include(x => x.Room)
                    .FirstOrDefault(x => x.Id == id);

                if (climateHistory == null)
                    return NotFound(new
                    {
                        status = false,
                        message = "Such climate history doesn't exist"
                    });


                // Remove redundant data
                climateHistory.Room.ClimateHistories = null;
                climateHistory.Device.ClimateHistories = null;
                climateHistory.Room.Devices = null;
                climateHistory.Device.Room.Devices = null;

                return Json(new
                {
                    status = true,
                    data = climateHistory
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Adding new climate history route
        /// </summary>
        /// <code>POST /climateHistory/</code>
        /// <param name="climateHistory">ClimateHistory object</param>
        /// <returns>Response with result status and created climate</returns>
        /// \todo(Check existence of device)
        [Authorize(Roles = "admin,manager")]
        [HttpPost("climateHistory")]
        public async Task<IActionResult> PostClimateHistory([FromBody] ClimateHistory climateHistory)
        {
            try
            {
                await _climateContext.ClimateHistories.AddAsync(climateHistory);
                await _climateContext.SaveChangesAsync();
                return Json(new {status = true, data = climateHistory});
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Route for deleting climate history by specified id
        /// </summary>
        /// <code>DELETE /climateHistory/{id}</code>
        /// <param name="id">Climate history id</param>
        /// <returns>Status of response</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete("climateHistory/{id}")]
        public async Task<IActionResult> DeleteClimateHistory([FromRoute] int id)
        {
            try
            {
                var climateHistory = _climateContext.ClimateHistories.FirstOrDefault(x => x.Id == id);
                if (climateHistory == null)
                    return NotFound(new {status = false, message = "There is no such climate history in DB"});
                _climateContext.ClimateHistories.Remove(climateHistory);
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