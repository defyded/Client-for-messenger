using System;
using System.Collections.Generic;
using System.Text;

namespace Client_For_Messenger.Models
{
    public class LoginModel
    {
        private string Email { get; set; }
        private string Password { get; set; }
        private bool ActiveBtn { get; set; }
        private bool VisibleError { get; set; }
        private string Error { get; set; }

        private async Task Request()
        {
            if (!ActiveBtn)
            {
                return;
            }
            ActiveBtn = false;
        }
    }
}
