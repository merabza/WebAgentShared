﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalRClient;
using SystemToolsShared;

namespace LibWebAgentMessages;

public class MessagesHub : Hub<IMessenger>
{
    private readonly IMessagesDataManager _messagesDataManager;

    public MessagesHub(IMessagesDataManager messagesDataManager)
    {
        _messagesDataManager = messagesDataManager;
    }

    public override Task OnConnectedAsync()
    {
        //_userCount ++;
        if (Context.UserIdentifier is not null)
            _messagesDataManager.UserConnected(Context.ConnectionId, Context.UserIdentifier);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        //_userCount --;
        if (Context.UserIdentifier is not null)
            _messagesDataManager.UserDisconnected(Context.ConnectionId, Context.UserIdentifier);
        return base.OnDisconnectedAsync(exception);
    }
}