using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibWebAgentMessages;

public class MessagesDataManager : IMessagesDataManager, IDisposable
{
    private readonly IHubContext<MessagesHub> _hub;
    private readonly ILogger<MessagesDataManager> _logger;
    private readonly Dictionary<string, string> _connectedUsers = new();

    public MessagesDataManager(IHubContext<MessagesHub> hub, ILogger<MessagesDataManager> logger)
    {
        _hub = hub;
        _logger = logger;
    }

    public Task? SendMessage(string userName, string message)
    {
        if (!_connectedUsers.TryGetValue(userName, out var connectionId))
            return null;
        _logger.LogInformation("Try to send message: {message}", message);
        return _hub.Clients.Client(connectionId).SendAsync("sendMessage", message);
    }

    public void UserConnected(string connectionId, string userName)
    {
        _connectedUsers.Add(userName, connectionId);
    }

    public void UserDisconnected(string userName)
    {
        _connectedUsers.Remove(userName);
    }

    public void Dispose()
    {
    }
}