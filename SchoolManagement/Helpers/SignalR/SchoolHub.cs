using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace SchoolManagement.Helpers.SignalR
{
    public class SchoolHub : Hub
    {
        public async Task SendMessage(string message, object data = null)
        {
            await Clients.All.SendAsync("ReceiveMessage", message, data);
        }

        //public override async Task OnConnectedAsync()
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, "ConnectedUsers");
        //    await base.OnConnectedAsync();
        //    Console.WriteLine("Client Connected");

        //}

        //public override async Task OnDisconnectedAsync(Exception exception)
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR");
        //    await base.OnDisconnectedAsync(exception);

        //    Console.WriteLine("Removed: " + Context.ConnectionId);
        //}
    }
}
