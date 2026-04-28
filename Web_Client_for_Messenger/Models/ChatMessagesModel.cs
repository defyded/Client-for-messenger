using Client_For_Messenger.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client_For_Messenger.Models
{
    public class ChatMessagesModel
    {
        public string Message { get; set; }
        public string FlowDirection { get; set; }
        public string SendAt { get; set; }
        public bool IsOutgoing => FlowDirection == "End";

    }
}
