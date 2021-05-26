using BlazorServerApp.Services;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Pages
{
    public class EditEmployeeBase : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IEmployeeService EmployeeService { get; set; }
        [Inject]
        public IDepartmentService DepartmentService { get; set; }

        public Employee Employee { get; set; } = new();
        public int DepartmentId { get; set; }

        [Parameter]
        public string Id { get; set; }

        public string ExceptionMessage { get; set; }

        public IEnumerable<Department> Departments { get; set; }

        protected async override Task OnInitializedAsync()
        {
            if (int.TryParse(Id, out int newEmployeeId) && newEmployeeId != 0)
            {
                Employee = await EmployeeService.GetEmployee(int.Parse(Id));
            }
            else
            {
                Employee = new Employee
                {
                    DepartmentId = 1,
                    DateOfBrith = DateTime.Now,
                    PhotoPath = "images/nophoto.jpg"
                };
            }
           

            Departments = await DepartmentService.GetDepartments();

            DepartmentId = Employee.DepartmentId;
        }

        protected async Task HandleValidSubmit()
        {

            if (Employee.EmployeeId != 0)
            {
                try
                {
                    var result = await EmployeeService.UpdateEmployee(Employee);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ExceptionMessage = ex.Message;
                    NavigationManager.NavigateTo("/");
                }
            }
            else
            {
                try
                {
                    await EmployeeService.CreateEmployee(Employee);
                }
                catch (Exception)
                {

                    throw;
                }
                NavigationManager.NavigateTo("/");
            }
          
            
            
        }
    }
}
