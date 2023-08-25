using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAgentMessagesContracts;

namespace LibWebAgentMessages;

public class MessagesDataManager : IMessagesDataManager, IDisposable
{
    private readonly IHubContext<MessagesHub, IMessenger> _hub;
    private readonly ILogger<MessagesDataManager> _logger;
    private readonly Dictionary<string, List<string>> _connectedUsers = new();

    public MessagesDataManager(IHubContext<MessagesHub, IMessenger> hub, ILogger<MessagesDataManager> logger)
    {
        _hub = hub;
        _logger = logger;
    }

    public async Task SendMessage(string? userName, string message, params string[] parameters)
    {
        if (userName is null)
            return;
        if (!_connectedUsers.TryGetValue(userName, out var conList))
            return;
        var mes = string.Format(message, parameters.Cast<object?>());
        _logger.LogInformation("Try to send message: {mes}", mes);
        var webAgentMessage = new WebAgentMessage(message, parameters);
        foreach (var connectionId in conList)
            await _hub.Clients.Client(connectionId).SendMessage(webAgentMessage);
        //await _hub.Clients.All.SendMessage(message);
    }

    public void UserConnected(string connectionId, string userName)
    {
        if (!_connectedUsers.ContainsKey(userName))
            _connectedUsers.Add(userName, new List<string>());
        var conList = _connectedUsers[userName];
        if (!conList.Contains(connectionId))
            conList.Add(connectionId);
    }

    public void UserDisconnected(string connectionId, string userName)
    {
        if (!_connectedUsers.ContainsKey(userName))
            return;
        var conList = _connectedUsers[userName];
        if (!conList.Contains(connectionId))
            return;
        conList.Remove(connectionId);
        if (conList.Count == 0)
            _connectedUsers.Remove(userName);
    }

    public void Dispose()
    {
    }
}