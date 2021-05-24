using EmployeeManagement.API.Data;
using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.API.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _db;

        public EmployeeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            var result = await _db.Employees.AddAsync(employee);
            await _db.SaveChangesAsync();
            return result.Entity;
        }

        public async void DeleteEmployee(int employeeId)
        {
            var result = await _db.Employees.FindAsync(employeeId);

            if (result != null)
            {
                _db.Employees.Remove(result);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<Employee> GetEmployee(int employeeId)
        {
            return await _db.Employees.FindAsync(employeeId);
        }

        public List<Employee> GetEmployees()
        {
            return _db.Employees.ToList();
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            var result = _db.Employees
                .FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);

            if (result != null)
            {
                result.FirstName = employee.FirstName;
                result.LastName = employee.LastName;
                result.Email = employee.Email;
                result.DateOfBrith = employee.DateOfBrith;
                result.Gender = employee.Gender;
                result.DepartmentId = employee.DepartmentId;
                result.PhotoPath = employee.PhotoPath;

                await _db.SaveChangesAsync();

                return result;
            }
            return null;
        }
    }
}
