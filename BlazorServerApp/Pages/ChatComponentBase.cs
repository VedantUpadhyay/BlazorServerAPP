using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorServerApp.Pages
{
    public class ChatComponentBase : ComponentBase
    {

        [Inject] NavigationManager NavigationManager { get; set; }
        private HubConnection hubConn;

        public List<string> Messages { get; set; } = new();

        public string Message { get; set; } = string.Empty;

        public string MyUserName { get; set; }

        protected async override Task OnInitializedAsync()
        {
            hubConn = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/myHub"))
                .Build();

            hubConn.On<string,string>("ReceiveMessage", (message, fromUser) =>
             {
                 Messages.Add($"{fromUser} : {message}");
                 StateHasChanged();
                 return Task.CompletedTask;
             });

            await hubConn.StartAsync();
        }

        public async Task SendMessage()
        {
            await hubConn.InvokeAsync("SendAll", Message, MyUserName);
        }
    }
}
