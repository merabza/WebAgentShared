using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Installer.AgentClients;
using Installer.Models;
using LibProjectsApi.CommandRequests;
using LibWebAgentMessages;
using MediatR;
using MessagingAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneOf;
using SystemToolsShared;
using WebAgentMessagesContracts;

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
        if (request.ServiceName is null)
            return await Task.FromResult(new[] { ProjectsErrors.SameParametersAreEmpty });

        var installerSettings = InstallerSettings.Create(_config);

        var agentClient = AgentClientsFabric.CreateAgentClient(_logger, false, installerSettings.InstallFolder,
            _messagesDataManager, request.UserName);

        if (agentClient is null)
            return await Task.FromResult(new[] { ProjectsErrors.AgentClientDoesNotCreated });

        if (request.ServiceName is null)
        {
            if (await agentClient.RemoveProject(request.ProjectName))
                return new Unit();
        }
        else
        {
            if (await agentClient.RemoveProjectAndService(request.ProjectName, request.ServiceName))
                return new Unit();
        }

        var err = ProjectsErrors.ProjectServiceCannotBeRemoved(request.ProjectName, request.ServiceName);
        _logger.LogError(err.ErrorMessage);
        return await Task.FromResult(new[] { err });
    }
}