using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Installer.AgentClients;
using LibProjectsApi.QueryRequests;
using MessagingAbstractions;
using Microsoft.Extensions.Logging;
using OneOf;
using SystemToolsShared;
// ReSharper disable ConvertToPrimaryConstructor

namespace LibProjectsApi.Handlers;

// ReSharper disable once ClassNeverInstantiated.Global
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
        var webAgentClient = new TestApiClient(_logger,
            $"http://localhost:{request.ServerSidePort}/api/{request.ApiVersionId}/");
        var getAppSettingsVersionResult = await webAgentClient.GetAppSettingsVersion(cancellationToken);
        if (getAppSettingsVersionResult.IsT1)
            return getAppSettingsVersionResult.AsT1;
        return getAppSettingsVersionResult.AsT0;
    }
}