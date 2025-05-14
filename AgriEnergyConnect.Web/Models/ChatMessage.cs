// Models/ChatMessage.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.Web.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; } = null!;   // e.g. IdentityUser.UserName

        [Required]
        public string Text { get; set; } = null!;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
