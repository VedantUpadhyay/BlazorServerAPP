using BlazorServerApp.Services;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerApp.Pages
{
    public class EmployeeListBase : ComponentBase
    {
        [Inject]
        public IEmployeeService EmployeeService { get; set; }

        public IEnumerable<Employee> Employees { get; set; }

        public ConfirmBase DeleteConfirmation { get; set; }

        public int SelectedEmployeeId { get; set; }


        protected override async Task OnInitializedAsync()
        {
            Employees = (await EmployeeService.GetEmployees()).ToList();
            //await Task.Run(LoadEmployees);
        }

        public async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                if (await EmployeeService.DeleteEmployee(SelectedEmployeeId))
                {
                    Employees = (await EmployeeService.GetEmployees()).ToList();
                }
            }
        }

        protected void Delete_Click(int id)
        {
            SelectedEmployeeId = id;
            DeleteConfirmation.Show();
        }



    }
}
