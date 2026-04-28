using System;
using System.Collections.Generic;
using System.Text;

namespace Client_For_Messenger.DTOs
{
    public record SearchUserRequest(string Username);
    public record UserDto(Guid userId, string avatarUrl, string username);
    public record SearchUsersResponce(List<UserDto> Users);
}
