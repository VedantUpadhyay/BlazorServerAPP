using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.API.Repositories
{
    public interface IEmployeeRepository
    {
        List<Employee> GetEmployees();
        Task<Employee> GetEmployee(int employeeId);
        Task<Employee> AddEmployee(Employee employee);
        Task<IActionResult> UpdateEmployee(Employee employee);
        void DeleteEmployee(int employeeId);

        Task<Employee> GetEmployeeByEmail(string email);

        Task<List<Employee>> Seach(string name, Gender? gender);
    }
}
