// Hubs/ChatHub.cs
using System.Threading.Tasks;
using AgriEnergyConnect.Web.Data;
using AgriEnergyConnect.Web.Models;
using Microsoft.AspNetCore.SignalR;

namespace AgriEnergyConnect.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _db;
        public ChatHub(ApplicationDbContext db) => _db = db;

        public async Task SendMessage(string user, string message)
        {
            var chat = new ChatMessage
            {
                UserName = user,                    // who sent it
                Text = message,                 // what they sent
                Timestamp = DateTime.Now             // when they sent it
            };
            _db.ChatMessages.Add(chat);             // queue for DB insert
            await _db.SaveChangesAsync();           // commit to DB

            // push to all clients
            await Clients.Others.SendAsync(
                "ReceiveMessage",
                chat.UserName,
                chat.Text,
                chat.Timestamp.ToString("HH:mm")
            );
        }

    }
}
