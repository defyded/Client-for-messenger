using System;
using System.Collections.Generic;
using System.Text;

namespace Client_For_Messenger.Models
{
    public record UserModel
    {
        public Guid UserId { get; set; }
        public string AvatarUrl { get; set; }
        public string Username { get; set; }
        private bool IsVisibleImageFlag { get; set; }
    }
}
