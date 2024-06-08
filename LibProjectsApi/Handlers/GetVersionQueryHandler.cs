using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LibProjectsApi.QueryRequests;
using MessagingAbstractions;
using Microsoft.Extensions.Logging;
using OneOf;
using SystemToolsShared;
using SystemToolsShared.Errors;
using TestApiContracts;

// ReSharper disable ConvertToPrimaryConstructor

namespace LibProjectsApi.Handlers;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class GetVersionQueryHandler : IQueryHandler<GetVersionQueryRequest, string?>
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

    public async Task<OneOf<string?, IEnumerable<Err>>> Handle(GetVersionQueryRequest request,
        CancellationToken cancellationToken)
    {
        //var webAgentClient =
        //    new TestApiClient(_logger, $"http://localhost:{request.ServerSidePort}/api/{request.ApiVersionId}/");
        //var getVersionResult = await webAgentClient.GetVersion(cancellationToken);
        //if (getVersionResult.IsT1)
        //    return getVersionResult.AsT1;
        //return getVersionResult.AsT0;


        //---
//        await _messagesDataManager.SendMessage(request.UserName, $"{nameof(UpdateSettings)} started", cancellationToken);
        var errors = new List<Err>();
        const int maxTryCount = 3;
        var tryCount = 0;
        while (tryCount < maxTryCount)
        {
            if (tryCount > 0)
            {
                const string waitingMessage = "waiting for 3 second...";
                await _messagesDataManager.SendMessage(request.UserName, waitingMessage, cancellationToken);
                _logger.LogInformation(waitingMessage);
                Thread.Sleep(3000);
            }

            tryCount++;
            try
            {
                await _messagesDataManager.SendMessage(request.UserName, $"Try to get Version {tryCount}...",
                    cancellationToken);
                _logger.LogInformation("Try to get Version {tryCount}...", tryCount);

                var webAgentClient = new TestApiClient(_logger, _httpClientFactory,
                    $"http://localhost:{request.ServerSidePort}/api/{request.ApiVersionId}/");
                var getVersionResult = await webAgentClient.GetVersion(cancellationToken);
                if (getVersionResult.IsT0)
                    //აქ თუ მოვედით, ყველაფერი კარგად არის
                    return getVersionResult.AsT0;

                errors.AddRange(getVersionResult.AsT1);

                await _messagesDataManager.SendMessage(request.UserName, $"could not get version on try {tryCount}",
                    cancellationToken);
                _logger.LogInformation("could not get version on try {tryCount}", tryCount);
            }
            catch (Exception)
            {
                await _messagesDataManager.SendMessage(request.UserName, $"Error when get version on try {tryCount}",
                    cancellationToken);
                _logger.LogInformation("Error when get version on try {tryCount}", tryCount);
            }
        }


        //---
        if (errors.Count > 0)
            return errors;

        return new Err[]
            { new() { ErrorCode = "GetVersionUnknownError", ErrorMessage = "Unknown Error returned GetVersion" } };
    }
}