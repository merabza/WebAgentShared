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
            new TestApiClient(_logger, $"http://localhost:{request.ServerSidePort}/api/{request.ApiVersionId}/");
        var getVersionResult = await webAgentClient.GetVersion(cancellationToken);
        if (getVersionResult.IsT1)
            return getVersionResult.AsT1;
        return getVersionResult.AsT0;


    }
}