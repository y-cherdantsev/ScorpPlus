using System;
using System.Net;
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
    /// Room activities controller
    /// </summary>
    /// <code>api/v1/room</code>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RoomController : Controller
    {
        /// <summary>
        /// Room context
        /// </summary>
        private readonly RoomContext _roomContext;

        /// <summary>
        /// Room controller constructor
        /// </summary>
        /// <param name="roomContext">Room context</param>
        public RoomController(RoomContext roomContext)
        {
            _roomContext = roomContext;
        }

        /// <summary>
        /// Get all rooms route
        /// </summary>
        /// <code>GET /all</code>
        /// <returns>Response status and list of rooms</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpGet("all")]
        public IActionResult GetAllRooms()
        {
            try
            {
                // Get data
                var rooms = _roomContext.Rooms
                    .Include(x => x.Devices)
                    .ThenInclude(x => x.Type)
                    .Include(x => x.Accesses)
                    .ThenInclude(x => x.Employee)
                    .Include(x => x.ClimateHistories)
                    .ToList();


                // Remove redundant data
                rooms.ForEach(room =>
                {
                    room.Accesses.ForEach(access =>
                    {
                        access.Room = null;
                        access.Employee.Accesses = null;
                    });
                    room.Devices.ForEach(access =>
                    {
                        access.Room = null;
                        access.Type.Devices = null;
                    });
                    room.ClimateHistories.ForEach(climate =>
                    {
                        climate.Room = null;
                        climate.Device = null;
                    });
                });

                return Json(new
                {
                    status = true,
                    data = rooms
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Route for getting room by specified id
        /// </summary>
        /// <code>GET /{id}</code>
        /// <param name="id">Room id</param>
        /// <returns>Response status and room object</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpGet("{id}")]
        public IActionResult GetRoom([FromRoute] int id)
        {
            try
            {
                // Get data
                var room = _roomContext.Rooms
                    .Include(x => x.Devices)
                    .ThenInclude(x => x.Type)
                    .Include(x => x.Accesses)
                    .ThenInclude(x => x.Employee)
                    .Include(x => x.ClimateHistories)
                    .FirstOrDefault(x => x.Id == id);

                if (room == null)
                    return NotFound(new
                    {
                        status = false,
                        message = "Such room doesn't exist"
                    });


                // Remove redundant data
                room.Accesses.ForEach(access =>
                {
                    access.Room = null;
                    access.Employee.Accesses = null;
                });
                room.Devices.ForEach(access =>
                {
                    access.Room = null;
                    access.Type.Devices = null;
                });
                room.ClimateHistories.ForEach(climate =>
                {
                    climate.Room = null;
                    climate.Device = null;
                });

                return Json(new
                {
                    status = true,
                    data = room
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Adding new room route
        /// </summary>
        /// <code>POST /</code>
        /// <param name="room">Room object</param>
        /// <returns>Response with result status and created room</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpPost]
        public async Task<IActionResult> PostRoom([FromBody] Room room)
        {
            try
            {
                // Code validation
                if (_roomContext.Rooms.FirstOrDefault(x => x.Code == room.Code) != null)
                    return StatusCode((int) HttpStatusCode.Conflict,
                        new {status = false, message = "Room with given code already exists"});

                await _roomContext.Rooms.AddAsync(room);
                await _roomContext.SaveChangesAsync();
                return Json(new {status = true, data = room});
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Route for deleting room by specified id
        /// </summary>
        /// <code>DELETE /{id}</code>
        /// <param name="id">Room id</param>
        /// <returns>Status of response</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom([FromRoute] int id)
        {
            try
            {
                var room = _roomContext.Rooms.FirstOrDefault(x => x.Id == id);
                if (room == null)
                    return NotFound(new {status = false, message = "There is no such room in DB"});
                _roomContext.Rooms.Remove(room);
                await _roomContext.SaveChangesAsync();
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