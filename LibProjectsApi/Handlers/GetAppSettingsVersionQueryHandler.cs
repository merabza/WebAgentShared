using Installer.AgentClients;
using LibProjectsApi.QueryRequests;
using MessagingAbstractions;
using Microsoft.Extensions.Logging;
using OneOf;
using SystemToolsShared;

namespace LibProjectsApi.Handlers;

// ReSharper disable once UnusedType.Global
public sealed class GetAppSettingsVersionQueryHandler : IQueryHandler<GetAppSettingsVersionQueryRequest, string?>
{
    private readonly ILogger<GetAppSettingsVersionQueryHandler> _logger;

    public GetAppSettingsVersionQueryHandler(ILogger<GetAppSettingsVersionQueryHandler> logger)
    {
        _logger = logger;
    }

    public async Task<OneOf<string?, IEnumerable<Err>>> Handle(GetAppSettingsVersionQueryRequest request,
        CancellationToken cancellationToken)
    {
        var webAgentClient =
            new WebAgentClient(_logger, $"http://localhost:{request.ServerSidePort}/api/{request.ApiVersionId}/", null);
        return await webAgentClient.GetAppSettingsVersion();
    }
}