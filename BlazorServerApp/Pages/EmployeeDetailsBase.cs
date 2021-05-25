using BlazorServerApp.Services;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Pages
{
    public class EmployeeDetailsBase : ComponentBase
    {
        public Employee Employee { get; set; } = new Employee();


        [Inject]
        IEmployeeService EmployeeService { get; set; }

        [Parameter]
        public string Id { get; set; }
        public string Coordinates { get; set; }
        public string ButtonText { get; set; } = "Hide Footer";

        public string CssClass { get; set; } = null;


        protected void Button_Click()
        {
            if (ButtonText == "Hide Footer")
            {
                ButtonText = "Show Footer";
                CssClass = "hideFooter";
            }
            else
            {
                ButtonText = "Hide Footer";
                CssClass = null;
            }
        }

        protected async override Task OnInitializedAsync()
        {
            Id ??= "1";

            Employee = await EmployeeService.GetEmployee(int.Parse(Id));
        }

        protected void Mouse_Move(MouseEventArgs e)
        {
            Coordinates = $"X : {e.ClientX} Y : {e.ClientY}";
        }
    }
}
