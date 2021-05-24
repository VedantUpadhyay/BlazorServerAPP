using EmployeeManagement.API.Repositories;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            return Ok(_employeeRepository.GetEmployees());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            try
            {
                var result = await _employeeRepository.GetEmployee(id);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,e.Message);
            }
        } 

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }
                else
                {
                    if (!TryValidateModel(employee))
                    {
                        return BadRequest();
                    }

                    var createdEmployee = await _employeeRepository.AddEmployee(employee);

                    return CreatedAtAction(nameof(GetEmployee), new
                    {
                        id = createdEmployee.EmployeeId
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
