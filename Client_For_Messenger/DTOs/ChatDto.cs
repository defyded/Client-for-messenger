using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Client_For_Messenger.DTOs
{
    public class ChatDto
    {
        [JsonPropertyName("chatId")]
        public Guid ChatId { get; set; }

        [JsonPropertyName("companionId")]
        public Guid CompanionId { get; set; }

        [JsonPropertyName("companionUsername")]
        public string CompanionUsername { get; set; }

        [JsonPropertyName("avatarURL")]
        public string? AvatarURL { get; set; } 

        [JsonPropertyName("lastMessage")]
        public ChatMessageDto? LastMessage { get; set; } 

        [JsonPropertyName("blocked")]
        public bool Blocked { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        public string Initials => string.IsNullOrWhiteSpace(CompanionUsername)
            ? "?" : CompanionUsername[0].ToString().ToUpper();

        public bool IsVisibleImageFlag => !string.IsNullOrEmpty(AvatarURL);
        public bool IsVisibleInitialFlag => !IsVisibleImageFlag;
    }

    public class ChatMessageDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("senderId")]
        public Guid SenderId { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("readAt")]
        public DateTime? ReadAt { get; set; } 
    }
}
