using Client_For_Messenger.DTOs;
using Client_For_Messenger.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Client_For_Messenger.Models
{
    public class ChatModel
    {
        public Guid Id { get; set; }
        public string CompanionUsername { get; set; }
        public string AvatarURL { get; set; }
        public ChatMessageDto LastMessage { get; set; }
        public string Initials => string.IsNullOrWhiteSpace(CompanionUsername)
            ? "?" : CompanionUsername[0].ToString().ToUpper();
        public bool IsVisibleImageFlag => !string.IsNullOrEmpty(AvatarURL);
        public bool IsVisibleInitialFlag => !IsVisibleImageFlag;
    }
}
