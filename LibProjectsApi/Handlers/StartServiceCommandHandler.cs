﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Installer.AgentClients;
using Installer.Models;
using LibProjectsApi.CommandRequests;
using MediatR;
using MessagingAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneOf;
using SystemToolsShared;

// ReSharper disable ConvertToPrimaryConstructor

namespace LibProjectsApi.Handlers;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class StartServiceCommandHandler : ICommandHandler<StartServiceCommandRequest>
{
    private readonly IConfiguration _config;
    private readonly ILogger<StartServiceCommandHandler> _logger;
    private readonly IMessagesDataManager _messagesDataManager;

    public StartServiceCommandHandler(IConfiguration config, ILogger<StartServiceCommandHandler> logger,
        IMessagesDataManager messagesDataManager)
    {
        _config = config;
        _logger = logger;
        _messagesDataManager = messagesDataManager;
    }

    public async Task<OneOf<Unit, IEnumerable<Err>>> Handle(StartServiceCommandRequest request,
        CancellationToken cancellationToken)
    {
        if (request.ServiceName is null)
            return await Task.FromResult(new[] { ProjectsErrors.SameParametersAreEmpty });

        var installerSettings = InstallerSettings.Create(_config);

        var agentClient = await AgentClientsFabric.CreateAgentClient(_logger, false, installerSettings.InstallFolder,
            _messagesDataManager, request.UserName, cancellationToken);

        if (agentClient is null)
            return await Task.FromResult(new[] { ProjectsErrors.AgentClientDoesNotCreated });

        if (await agentClient.StartService(request.ServiceName, request.EnvironmentName, cancellationToken))
            return new Unit();

        var err = ProjectsErrors.CannotBeStartedService(request.ServiceName);

        _logger.LogError(err.ErrorMessage);
        return await Task.FromResult(new[] { err });
    }
}