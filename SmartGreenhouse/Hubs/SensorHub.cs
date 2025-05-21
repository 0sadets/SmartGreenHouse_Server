using Microsoft.AspNetCore.SignalR;

namespace SmartGreenhouse.Hubs
{
    public class SensorHub : Hub
    {
        public async Task JoinGroup(string greenhouseId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, greenhouseId);
        }

        public async Task LeaveGroup(string greenhouseId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, greenhouseId);
        }
    }
}
