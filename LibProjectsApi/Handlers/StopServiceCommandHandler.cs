using System.Collections.Generic;
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
using WebAgentMessagesContracts;

namespace LibProjectsApi.Handlers;

// ReSharper disable once UnusedType.Global
// ReSharper disable once ClassNeverInstantiated.Global
public sealed class StopServiceCommandHandler : ICommandHandler<StopServiceCommandRequest>
{
    private readonly IConfiguration _config;
    private readonly ILogger<StopServiceCommandHandler> _logger;
    private readonly IMessagesDataManager _messagesDataManager;

    public StopServiceCommandHandler(IConfiguration config, ILogger<StopServiceCommandHandler> logger,
        IMessagesDataManager messagesDataManager)
    {
        _config = config;
        _logger = logger;
        _messagesDataManager = messagesDataManager;
    }

    public async Task<OneOf<Unit, IEnumerable<Err>>> Handle(StopServiceCommandRequest request,
        CancellationToken cancellationToken)
    {
        if (request.ServiceName is null)
            return await Task.FromResult(new[] { ProjectsErrors.SameParametersAreEmpty });

        var installerSettings = InstallerSettings.Create(_config);

        var agentClient = AgentClientsFabric.CreateAgentClient(_logger, false, installerSettings.InstallFolder,
            _messagesDataManager, request.UserName);

        if (agentClient is null)
            return await Task.FromResult(new[] { ProjectsErrors.AgentClientDoesNotCreated });

        if (agentClient.StopService(request.ServiceName))
            return new Unit();

        var err = ProjectsErrors.CannotBeStoppedService(request.ServiceName);

        await _messagesDataManager.SendMessage(request.UserName, err.ErrorMessage);
        _logger.LogError(err.ErrorMessage);
        return await Task.FromResult(new[] { err });
    }
}