using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace LibWebAgentMessages.Authorization;

public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        // Implement user id acquiring logic  
        var nameClaim = connection.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
        return nameClaim?.Value;

        //_httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString();
    }
}