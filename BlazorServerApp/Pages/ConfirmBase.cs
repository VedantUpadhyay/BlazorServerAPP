using EmployeeManagement.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Pages
{
    public class ConfirmBase : ComponentBase
    {
        [Parameter]
        public EventCallback<bool> ConfirmationChanged{ get; set; }


        public bool ShowDialog { get; set; }

        public void Show()
        {
            ShowDialog = true;
            StateHasChanged();
        }

        public async Task OnConfirmationChanged(bool value)
        {
            ShowDialog = false;
            StateHasChanged();
            await ConfirmationChanged.InvokeAsync(value);
        }
    }
}
