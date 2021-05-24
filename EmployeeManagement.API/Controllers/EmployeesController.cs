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

                    var emp = await _employeeRepository.GetEmployeeByEmail(employee.Email);
                    if (emp != null)
                    {
                        ModelState.AddModelError("email", "Employee email already in use.");
                        return BadRequest(ModelState);
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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEmployee(int id,Employee employee)
        {
            try
            {
                if (id != employee.EmployeeId)
                {
                    return BadRequest("Employee ID mismatch.");
                }


                Employee employeeFromDb = await _employeeRepository.GetEmployee(id);

                if (employeeFromDb == null)
                {
                    return NotFound($"Employee with {id} not found.");
                }

                return (IActionResult)await _employeeRepository.UpdateEmployee(employee);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
   
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var empToDelete = await _employeeRepository.GetEmployee(id);
                if (empToDelete == null)
                {
                    return NotFound($"Employee with {id} not found.");
                }

                await Task.Run(()=>{
                    _employeeRepository.DeleteEmployee(id);
                });

                return StatusCode(StatusCodes.Status200OK,empToDelete);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Employee>>> Search(string name, Gender? gender)
        {
            try
            {
                var result = await _employeeRepository.Seach(name, gender);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
    } 
}
