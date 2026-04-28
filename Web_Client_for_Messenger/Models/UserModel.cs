using System;
using System.Collections.Generic;
using System.Text;

namespace Client_For_Messenger.Models
{
    public record UserModel
    {
        public string AvatarUrl { get; set; }
        public string Username { get; set; }
        private bool IsVisibleImageFlag { get; set; }
    }
}
