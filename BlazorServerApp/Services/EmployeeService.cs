using EmployeeManagement.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorServerApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient HttpClient;

        public EmployeeService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }


        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            Console.WriteLine((await HttpClient.GetAsync("api/employees")).Content);
            return await HttpClient.GetJsonAsync<Employee[]>("api/employees");
        }
    }
}
