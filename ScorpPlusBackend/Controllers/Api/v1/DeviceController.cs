using System;
using System.Net;
using System.Linq;
using ScorpPlus.Models;
using ScorpPlus.Contexts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ScorpPlusBackend.Controllers.Api.v1
{
    /// <summary>
    /// Device activities controller
    /// </summary>
    /// <code>api/v1/device</code>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DeviceController : Controller
    {
        /// <summary>
        /// Device context
        /// </summary>
        private readonly DeviceContext _deviceContext;

        /// <summary>
        /// Device controller constructor
        /// </summary>
        /// <param name="deviceContext">Device context</param>
        public DeviceController(DeviceContext deviceContext)
        {
            _deviceContext = deviceContext;
        }

        /// <summary>
        /// Get all devices route
        /// </summary>
        /// <code>GET /all</code>
        /// <returns>Response status and list of devices</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpGet("all")]
        public IActionResult GetAllDevices()
        {
            try
            {
                // Get data
                var devices = _deviceContext.Devices
                    .Include(x => x.Type)
                    .Include(x => x.Room)
                    .ToList();


                // Remove redundant data
                devices.ForEach(device =>
                {
                    device.Type.Devices = null;
                    device.Room.Devices = null;
                    device.Room.Accesses = null;
                });

                return Json(new
                {
                    status = true,
                    data = devices
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Route for getting device by specified id
        /// </summary>
        /// <code>GET /{id}</code>
        /// <param name="id">Device id</param>
        /// <returns>Response status and device object</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpGet("{id}")]
        public IActionResult GetDevice([FromRoute] int id)
        {
            try
            {
                // Get data
                var device = _deviceContext.Devices
                    .Include(x => x.Type)
                    .Include(x => x.Room)
                    .FirstOrDefault(x => x.Id == id);

                if (device == null)
                    return NotFound(new
                    {
                        status = false,
                        message = "Such device doesn't exist"
                    });


                // Remove redundant data
                device.Type.Devices = null;
                device.Room.Devices = null;
                device.Room.Accesses = null;

                return Json(new
                {
                    status = true,
                    data = device
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Adding new device route
        /// </summary>
        /// <code>POST /</code>
        /// <param name="device">Device object</param>
        /// <returns>Response with result status and created device</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpPost]
        public async Task<IActionResult> PostDevice([FromBody] Device device)
        {
            try
            {
                // Code validation
                if (_deviceContext.Devices.FirstOrDefault(x => x.Code == device.Code) != null)
                    return StatusCode((int) HttpStatusCode.Conflict,
                        new {status = false, message = "Device with given code already exists"});

                await _deviceContext.Devices.AddAsync(device);
                await _deviceContext.SaveChangesAsync();
                return Json(new {status = true, data = device});
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Route for deleting device by specified id
        /// </summary>
        /// <code>DELETE /{id}</code>
        /// <param name="id">Device id</param>
        /// <returns>Status of response</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice([FromRoute] int id)
        {
            try
            {
                var device = _deviceContext.Devices.FirstOrDefault(x => x.Id == id);
                if (device == null)
                    return NotFound(new {status = false, message = "There is no such device in DB"});
                _deviceContext.Devices.Remove(device);
                await _deviceContext.SaveChangesAsync();
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
        /// Get all device types route
        /// </summary>
        /// <code>GET /type/all</code>
        /// <returns>Response status and list of device types</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpGet("type/all")]
        public IActionResult GetAllDeviceTypes()
        {
            try
            {
                // Get data
                var deviceTypes = _deviceContext.DeviceTypes
                    .Include(x => x.Devices)
                    .ThenInclude(x => x.Room)
                    .ToList();


                // Remove redundant data
                deviceTypes.ForEach(deviceType =>
                {
                    deviceType.Devices.ForEach(x =>
                    {
                        x.Type = null;
                        x.Room.Devices = null;
                    });
                });

                return Json(new
                {
                    status = true,
                    data = deviceTypes
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Route for getting device type by specified id
        /// </summary>
        /// <code>GET /type/{id}</code>
        /// <param name="id">DeviceType id</param>
        /// <returns>Response status and device type object</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpGet("type/{id}")]
        public IActionResult GetDeviceType([FromRoute] int id)
        {
            try
            {
                // Get data
                var deviceType = _deviceContext.DeviceTypes
                    .Include(x => x.Devices)
                    .ThenInclude(x => x.Room)
                    .FirstOrDefault(x => x.Id == id);

                if (deviceType == null)
                    return NotFound(new
                    {
                        status = false,
                        message = "Such device type doesn't exist"
                    });


                // Remove redundant data
                deviceType.Devices.ForEach(x =>
                {
                    x.Type = null;
                    x.Room.Devices = null;
                });

                return Json(new
                {
                    status = true,
                    data = deviceType
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }
    }
}