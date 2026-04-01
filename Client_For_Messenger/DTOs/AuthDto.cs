using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Client_For_Messenger.DTOs
{
    public sealed record LoginRequest(string Email, string Password);
    public sealed record RegisterRequest(string Email, string Username,string Password);
    public sealed record LoginResponse(string AccessToken, DateTimeOffset ExpiresAtUtc, Guid id);

    public sealed record ApiResult<T>(
        bool IsSuccess,
        T? Data,
        string? ErrorMessage,
        HttpStatusCode? StatusCode
    );
}
