using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Installer.AgentClients;
using LibProjectsApi.QueryRequests;
using MessagingAbstractions;
using Microsoft.Extensions.Logging;
using OneOf;
using SystemToolsShared;
using WebAgentMessagesContracts;

namespace LibProjectsApi.Handlers;

// ReSharper disable once UnusedType.Global
public sealed class GetVersionQueryHandler : IQueryHandler<GetVersionQueryRequest, string?>
{
    private readonly ILogger<GetVersionQueryHandler> _logger;
    private readonly IMessagesDataManager? _messagesDataManager;

    public GetVersionQueryHandler(ILogger<GetVersionQueryHandler> logger, IMessagesDataManager? messagesDataManager)
    {
        _logger = logger;
        _messagesDataManager = messagesDataManager;
    }

    public async Task<OneOf<string?, IEnumerable<Err>>> Handle(GetVersionQueryRequest request,
        CancellationToken cancellationToken)
    {
        var webAgentClient =
            new WebAgentClient(_logger, $"http://localhost:{request.ServerSidePort}/api/{request.ApiVersionId}/", null,
                _messagesDataManager, request.UserName);
        return await webAgentClient.GetVersion();
    }
}