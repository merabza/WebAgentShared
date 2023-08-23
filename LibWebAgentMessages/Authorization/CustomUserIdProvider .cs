using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace LibWebAgentMessages.Authorization;

public class CustomUserIdProvider : IUserIdProvider
{
    readonly IHttpContextAccessor _httpContextAccessor;

    public CustomUserIdProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetUserId(HubConnectionContext connection)
    {
        // Implement user id acquiring logic  
        return _httpContextAccessor.HttpContext?.Request.Query["username"];
    }
}