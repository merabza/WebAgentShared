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
using SignalRContracts;
using SystemToolsShared;

// ReSharper disable ConvertToPrimaryConstructor

namespace LibProjectsApi.Handlers;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class RemoveProjectServiceCommandHandler : ICommandHandler<RemoveProjectServiceCommandRequest>
{
    private readonly IConfiguration _config;
    private readonly ILogger<RemoveProjectServiceCommandHandler> _logger;
    private readonly IMessagesDataManager _messagesDataManager;

    public RemoveProjectServiceCommandHandler(IConfiguration config, ILogger<RemoveProjectServiceCommandHandler> logger,
        IMessagesDataManager messagesDataManager)
    {
        _config = config;
        _logger = logger;
        _messagesDataManager = messagesDataManager;
    }

    public async Task<OneOf<Unit, IEnumerable<Err>>> Handle(RemoveProjectServiceCommandRequest request,
        CancellationToken cancellationToken)
    {
        var installerSettings = InstallerSettings.Create(_config);

        var agentClient = await AgentClientsFabric.CreateAgentClient(_logger, false, installerSettings.InstallFolder,
            _messagesDataManager, request.UserName, cancellationToken);

        if (agentClient is null)
            return await Task.FromResult(new[] { ProjectsErrors.AgentClientDoesNotCreated });

        if (await agentClient.RemoveProjectAndService(request.ProjectName,
                request.EnvironmentName, request.IsService, cancellationToken))
            return new Unit();

        var err = ProjectsErrors.ProjectServiceCannotBeRemoved(request.ProjectName);

        _logger.LogError(err.ErrorMessage);
        return await Task.FromResult(new[] { err });
    }
}