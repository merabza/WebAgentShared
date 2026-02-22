using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared;
using SystemTools.SystemToolsShared.Errors;
using SystemTools.TestApiContracts;
using WebAgentShared.LibProjectsApi.QueryRequests;

// ReSharper disable ConvertToPrimaryConstructor

namespace WebAgentShared.LibProjectsApi.Handlers;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class GetVersionQueryHandler : IQueryHandler<GetVersionRequestQuery, string?>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GetVersionQueryHandler> _logger;
    private readonly IMessagesDataManager _messagesDataManager;

    public GetVersionQueryHandler(ILogger<GetVersionQueryHandler> logger, IHttpClientFactory httpClientFactory,
        IMessagesDataManager messagesDataManager)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _messagesDataManager = messagesDataManager;
    }

    public async Task<OneOf<string?, Err[]>> Handle(GetVersionRequestQuery request, CancellationToken cancellationToken)
    {
        var errors = new List<Err>();
        const int maxTryCount = 3;
        int tryCount = 0;
        while (tryCount < maxTryCount)
        {
            if (tryCount > 0)
            {
                const string waitingMessage = "waiting for 3 second...";
                await _messagesDataManager.SendMessage(request.UserName, waitingMessage, cancellationToken);
                _logger.LogInformation(waitingMessage);
                await Task.Delay(3000, cancellationToken);
            }

            tryCount++;
            try
            {
                await _messagesDataManager.SendMessage(request.UserName, $"Try to get Version {tryCount}...",
                    cancellationToken);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Try to get Version {TryCount}...", tryCount);
                }

                var webAgentClient = new TestApiClient(_logger, _httpClientFactory,
                    $"http://localhost:{request.ServerSidePort}/api/{request.ApiVersionId}/", false);
                OneOf<string, Err[]> getVersionResult = await webAgentClient.GetVersion(cancellationToken);
                if (getVersionResult.IsT0 && !string.IsNullOrWhiteSpace(getVersionResult.AsT0))
                    //აქ თუ მოვედით, ყველაფერი კარგად არის
                {
                    return getVersionResult.AsT0;
                }

                errors.AddRange(getVersionResult.AsT1);

                await _messagesDataManager.SendMessage(request.UserName, $"could not get version on try {tryCount}",
                    cancellationToken);
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("could not get version on try {TryCount}", tryCount);
                }
            }
            catch (Exception ex)
            {
                await _messagesDataManager.SendMessage(request.UserName, $"Error when get version on try {tryCount}",
                    cancellationToken);
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "Error when get version on try {TryCount}", tryCount);
                }
            }
        }

        //---
        if (errors.Count > 0)
        {
            return errors.ToArray();
        }

        return new Err[]
        {
            new() { ErrorCode = "GetVersionUnknownError", ErrorMessage = "Unknown Error returned GetVersion" }
        };
    }
}
