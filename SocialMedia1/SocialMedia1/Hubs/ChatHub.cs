namespace SocialMedia1.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using SocialMedia1.Data.Models;

    [Authorize]
    public class ChatHub : Hub
    {
        public async Task Send(string message)
        {
            await Clients.All.SendAsync(
                "NewMessage",
                new Message
                {
                    User = Context.User.Identity.Name,
                    Text = message,
                });
        }
    }
}
