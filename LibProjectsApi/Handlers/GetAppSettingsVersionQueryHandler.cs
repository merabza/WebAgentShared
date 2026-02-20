using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LibProjectsApi.QueryRequests;
using Microsoft.Extensions.Logging;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared.Errors;
using SystemTools.TestApiContracts;

// ReSharper disable ConvertToPrimaryConstructor

namespace LibProjectsApi.Handlers;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class GetAppSettingsVersionQueryHandler : IQueryHandler<GetAppSettingsVersionRequestQuery, string>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GetAppSettingsVersionQueryHandler> _logger;

    public GetAppSettingsVersionQueryHandler(ILogger<GetAppSettingsVersionQueryHandler> logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<OneOf<string, Err[]>> Handle(GetAppSettingsVersionRequestQuery request,
        CancellationToken cancellationToken = default)
    {
        var webAgentClient = new TestApiClient(_logger, _httpClientFactory,
            $"http://localhost:{request.ServerSidePort}/api/{request.ApiVersionId}/", false);
        return await webAgentClient.GetAppSettingsVersion(cancellationToken);
    }
}