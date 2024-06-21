﻿using LibProjectsApi.QueryRequests;
using MessagingAbstractions;
using Microsoft.Extensions.Logging;
using OneOf;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SystemToolsShared.Errors;
using TestApiContracts;

// ReSharper disable ConvertToPrimaryConstructor

namespace LibProjectsApi.Handlers;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class GetAppSettingsVersionQueryHandler : IQueryHandler<GetAppSettingsVersionQueryRequest, string?>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GetAppSettingsVersionQueryHandler> _logger;

    public GetAppSettingsVersionQueryHandler(ILogger<GetAppSettingsVersionQueryHandler> logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<OneOf<string?, IEnumerable<Err>>> Handle(GetAppSettingsVersionQueryRequest request,
        CancellationToken cancellationToken)
    {
        var webAgentClient = new TestApiClient(_logger, _httpClientFactory,
            $"http://localhost:{request.ServerSidePort}/api/{request.ApiVersionId}/", false);
        var getAppSettingsVersionResult = await webAgentClient.GetAppSettingsVersion(cancellationToken);
        if (getAppSettingsVersionResult.IsT1)
            return getAppSettingsVersionResult.AsT1;
        return getAppSettingsVersionResult.AsT0;
    }
}