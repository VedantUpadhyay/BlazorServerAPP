using BlazorServerApp.Services;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BlazorServerApp.Pages
{
    public class EditEmployeeBase : ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IEmployeeService EmployeeService { get; set; }
        [Inject]
        public IDepartmentService DepartmentService { get; set; }

        public string PageHeaderText { get; set; }

        public Employee Employee { get; set; } = new();
        public int DepartmentId { get; set; }

        [Parameter]
        public string Id { get; set; }

        public ConfirmBase DeleteConfirmation { get; set; }

        public string ExceptionMessage { get; set; }

        public IEnumerable<Department> Departments { get; set; }

        protected async override Task OnInitializedAsync()
        {
            if(!AuthenticationStateTask.Result.User.Identity.IsAuthenticated)
            {
                string returnUrl = WebUtility.UrlEncode($"/editemployee/{Id}");
                
                NavigationManager.NavigateTo($"/identity/account/login?returnUrl={returnUrl}");
            }

            if (int.TryParse(Id, out int newEmployeeId) && newEmployeeId != 0)
            {
                PageHeaderText = "Edit Employee";
                Employee = await EmployeeService.GetEmployee(int.Parse(Id));
            }
            else
            {
                PageHeaderText = "Create Employee";
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
                }
                NavigationManager.NavigateTo("/");
            }
 
        }

        public async Task ConfirmDelete_Click(bool confirm)
        {
            if (confirm)
            {
                if (await EmployeeService.DeleteEmployee(Employee.EmployeeId))
                {
                    NavigationManager.NavigateTo("/");
                }
            }
        }

        protected void Delete_Click(dynamic e)
        {
            DeleteConfirmation.Show();
        }
    }
}
