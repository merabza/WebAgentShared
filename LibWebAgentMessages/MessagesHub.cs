using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace LibWebAgentMessages;

[Authorize(Policy = "CustomHubAuthorizatioPolicy")]
public class MessagesHub : Hub
{
    private readonly IMessagesDataManager _progressDataManager;

    public MessagesHub(IMessagesDataManager progressDataManager)
    {
        _progressDataManager = progressDataManager;
    }

    public override Task OnConnectedAsync()
    {
        //_userCount ++;
        _progressDataManager.UserConnected(Context.ConnectionId, Context.UserIdentifier);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        //_userCount --;
        _progressDataManager.UserDisconnected(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}