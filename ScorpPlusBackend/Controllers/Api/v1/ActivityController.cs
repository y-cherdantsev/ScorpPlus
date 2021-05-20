using System;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScorpPlus.Contexts;
using ScorpPlus.Models;

namespace ScorpPlusBackend.Controllers.Api.v1
{
    /// <summary>
    /// Employee activities controller
    /// </summary>
    /// <code>api/v1/employee</code>
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/v1/[controller]")]
    public class ActivityController : Controller
    {
        /// <summary>
        /// Device context
        /// </summary>
        private readonly DeviceContext _deviceContext;

        /// <summary>
        /// Climate context
        /// </summary>
        private readonly ClimateContext _climateContext;

        /// <summary>
        /// Employee context
        /// </summary>
        private readonly EmployeeContext _employeeContext;

        /// <summary>
        /// Access context
        /// </summary>
        private readonly AccessContext _accessContext;

        /// <summary>
        /// Event controller constructor
        /// </summary>
        /// <param name="deviceContext">Device context</param>
        /// <param name="climateContext">Climate context</param>
        /// <param name="employeeContext">Employee context</param>
        /// <param name="accessContext">Access context</param>
        public ActivityController(DeviceContext deviceContext, ClimateContext climateContext,
            EmployeeContext employeeContext, AccessContext accessContext)
        {
            _deviceContext = deviceContext;
            _climateContext = climateContext;
            _employeeContext = employeeContext;
            _accessContext = accessContext;
        }

        /// <summary>
        /// Get all devices route
        /// </summary>
        /// <code>GET /all</code>
        /// <returns>Response status and list of devices</returns>
        [HttpGet("climate")]
        public IActionResult Climate(string deviceCode, string temperature, string humidity, string pressure)
        {
            var deviceId = GetDeviceId(deviceCode);
            if (deviceId == 0) return BadRequest("Device doesn't exist");

            try
            {
                _climateContext.ClimateHistories.Add(new ClimateHistory
                {
                    DeviceId = deviceId,
                    Temperature = double.Parse(temperature),
                    Humidity = double.Parse(humidity),
                    Pressure = double.Parse(pressure),
                    Relevance = DateTime.Now,
                    RoomId = GetDeviceRoomId(deviceId)
                });
                _climateContext.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.StackTrace);
            }
        }

        /// <summary>
        /// Get all devices route
        /// </summary>
        /// <code>GET /all</code>
        /// <returns>Response status and list of devices</returns>
        [HttpGet("temperature")]
        public IActionResult Temperature(string deviceCode, string temperature)
        {
            var deviceId = GetDeviceId(deviceCode);
            if (deviceId == 0) return BadRequest("Device doesn't exist");

            try
            {
                _climateContext.AccurateTemperatureHistories.Add(new AccurateTemperatureHistory
                {
                    DeviceId = deviceId,
                    Temperature = double.Parse(temperature),
                    Relevance = DateTime.Now,
                    RoomId = GetDeviceRoomId(deviceId)
                });
                _climateContext.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.StackTrace);
            }
        }

        /// <summary>
        /// Get all devices route
        /// </summary>
        /// <code>GET /all</code>
        /// <returns>Response status and list of devices</returns>
        [HttpGet("water")]
        public IActionResult Water(string deviceCode, string level)
        {
            var deviceId = GetDeviceId(deviceCode);
            if (deviceId == 0) return BadRequest("Device doesn't exist");

            try
            {
                _climateContext.WaterLevelHistories.Add(new WaterLevelHistory()
                {
                    DeviceId = deviceId,
                    Level = double.Parse(level),
                    Relevance = DateTime.Now,
                    RoomId = GetDeviceRoomId(deviceId)
                });
                _climateContext.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.StackTrace);
            }
        }

        /// <summary>
        /// Get all devices route 0 - null, 1 - entered, 2 - exited
        /// </summary>
        /// <code>GET /all</code>
        /// <returns>Response status and list of devices</returns>
        [HttpGet("access")]
        public IActionResult Access(string deviceCode, int employeeId, bool status)
        {
            var deviceId = GetDeviceId(deviceCode);
            if (deviceId == 0) return BadRequest("Device doesn't exist");

            try
            {
                var device = _deviceContext.Devices.Include(x => x.Room).ThenInclude(x => x.Accesses)
                    .FirstOrDefault(x => x.Id == deviceId);
                var hasAccess = device!.Room.Accesses.FirstOrDefault(x => x.EmployeeId == employeeId) != null;
                bool? statusBool = null;
                if (hasAccess)
                    statusBool = status;

                _accessContext.AccessHistories.Add(new AccessHistory
                {
                    DeviceId = deviceId,
                    EmployeeId = employeeId,
                    RoomId = GetDeviceRoomId(deviceId),
                    Relevance = DateTime.Now,
                    Status = statusBool
                });
                _accessContext.SaveChanges();
                if (hasAccess)
                    return Ok();
                return Forbid();
            }
            catch (Exception e)
            {
                return BadRequest(e.StackTrace);
            }
        }

        /// <summary>
        /// Returns Id of device and 0 if device doesn't exist in current system
        /// </summary>
        /// <param name="deviceCode">Device code name</param>
        /// <returns></returns>
        private int GetDeviceId(string deviceCode)
        {
            var device = _deviceContext.Devices.FirstOrDefault(x => x.Code == deviceCode);
            return device?.Id ?? 0;
        }

        /// <summary>
        /// Returns Id of device and 0 if device doesn't exist in current system
        /// </summary>
        /// <param name="deviceId">Device id</param>
        /// <returns></returns>
        private int GetDeviceRoomId(int deviceId)
        {
            var device = _deviceContext.Devices.Include(x => x.Room).FirstOrDefault(x => x.Id == deviceId);
            return device?.RoomId ?? 0;
        }
    }
}