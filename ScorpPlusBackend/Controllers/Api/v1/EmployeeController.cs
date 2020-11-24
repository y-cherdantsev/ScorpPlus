using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using ScorpPlusBackend.Models;
using Microsoft.AspNetCore.Mvc;
using ScorpPlusBackend.Contexts;
using Microsoft.AspNetCore.Authorization;

namespace ScorpPlusBackend.Controllers.Api.v1
{
    /// <summary>
    /// Employee activities controller
    /// </summary>
    /// <code>api/v1/employee</code>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeeController : Controller
    {
        /// <summary>
        /// Employee context
        /// </summary>
        private readonly EmployeeContext _employeeContext;

        /// <summary>
        /// Employee controller constructor
        /// </summary>
        /// <param name="employeeContext">Employee context</param>
        public EmployeeController(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }

        /// <summary>
        /// Get all employees route
        /// </summary>
        /// <code>GET /all</code>
        /// <returns>Response status and list of employees</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpGet("all")]
        public IActionResult GetAllEmployees()
        {
            try
            {
                var employees = _employeeContext.Employees.ToList();

                return Json(new
                {
                    status = true,
                    data = employees
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Route for getting employee by specified id
        /// </summary>
        /// <code>GET /{id}</code>
        /// <param name="id">Employee id</param>
        /// <returns>Response status and employee object</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpGet("{id}")]
        public IActionResult GetEmployee([FromRoute] int id)
        {
            try
            {
                var employee = _employeeContext.Employees.FirstOrDefault(x => x.Id == id);
                if (employee == null)
                    return NotFound(new
                    {
                        status = false,
                        message = "Such employee doesn't exist"
                    });

                return Json(new
                {
                    status = true,
                    data = employee
                });
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Adding new employee route
        /// </summary>
        /// <code>POST /</code>
        /// <param name="employee">Employee object</param>
        /// <returns>Response with result status and created employee</returns>
        [Authorize(Roles = "admin,manager")]
        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] Employee employee)
        {
            try
            {
                // IIN validation
                if (employee.Iin != null)
                {
                    if (employee.Iin.ToString().Length > 12)
                        return BadRequest(new
                            {status = false, message = "IIN field should be less or equal to 12 characters"});
                    if (_employeeContext.Employees.FirstOrDefault(x => x.Iin == employee.Iin) != null)
                        return StatusCode((int) HttpStatusCode.Conflict,
                            new {status = false, message = "Employee with given IIN already exists"});
                }

                await _employeeContext.Employees.AddAsync(employee);
                await _employeeContext.SaveChangesAsync();
                return Json(new {status = true, data = employee});
            }
            catch (Exception e)
            {
                return BadRequest(new {status = false, message = e.Message});
            }
        }

        /// <summary>
        /// Route for deleting employee by specified id
        /// </summary>
        /// <code>DELETE /{id}</code>
        /// <param name="id">Employee id</param>
        /// <returns>Status of response</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            try
            {
                var employee = _employeeContext.Employees.FirstOrDefault(x => x.Id == id);
                if (employee == null)
                    return NotFound(new {status = false, message = "There is no such employee in DB"});
                _employeeContext.Employees.Remove(employee);
                await _employeeContext.SaveChangesAsync();
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