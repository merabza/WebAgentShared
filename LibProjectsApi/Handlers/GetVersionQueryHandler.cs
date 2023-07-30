using Installer.AgentClients;
using LibProjectsApi.QueryRequests;
using MessagingAbstractions;
using Microsoft.Extensions.Logging;
using OneOf;
using SystemToolsShared;

namespace LibProjectsApi.Handlers;

// ReSharper disable once UnusedType.Global
public sealed class GetVersionQueryHandler : IQueryHandler<GetVersionQueryRequest, string?>
{
    private readonly ILogger<GetVersionQueryHandler> _logger;

    public GetVersionQueryHandler(ILogger<GetVersionQueryHandler> logger)
    {
        _logger = logger;
    }

    public async Task<OneOf<string?, IEnumerable<Err>>> Handle(GetVersionQueryRequest request,
        CancellationToken cancellationToken)
    {
        var webAgentClient =
            new WebAgentClient(_logger, $"http://localhost:{request.ServerSidePort}/api/{request.ApiVersionId}/", null);
        return await webAgentClient.GetVersion();
    }
}