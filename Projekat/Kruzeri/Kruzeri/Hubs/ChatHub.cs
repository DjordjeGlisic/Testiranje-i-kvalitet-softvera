using Microsoft.AspNetCore.SignalR;

namespace Kruzeri.Hubs
{
    public class ChatHub:Hub
    {
        public async Task NotifyClientsAboutMessageChange()
        {
            await Clients.All.SendAsync("MessageSetChanged");
        }
        
    }
}
