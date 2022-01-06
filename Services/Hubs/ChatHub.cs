using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Services.Hubs
{
    public class ChatHub : Hub
    {
        public Task SendMessage(string reciverId, string message)
            => Clients.All.SendAsync("ReceiverOne", reciverId, message);
    }
}