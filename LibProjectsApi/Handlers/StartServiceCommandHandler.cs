using System.Threading;
using System.Threading.Tasks;
using LibProjectsApi.CommandRequests;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared;
using SystemTools.SystemToolsShared.Errors;
using ToolsManagement.Installer.Models;
using ToolsManagement.Installer.ProjectManagers;

// ReSharper disable ConvertToPrimaryConstructor

namespace LibProjectsApi.Handlers;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class StartServiceCommandHandler : ICommandHandler<StartServiceRequestCommand>
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

    public async Task<OneOf<Unit, Err[]>> Handle(StartServiceRequestCommand request,
        CancellationToken cancellationToken = default)
    {
        if (request.ProjectName is null)
            return await Task.FromResult(new[] { ProjectsErrors.SameParametersAreEmpty });

        var installerSettings = InstallerSettings.Create(_config);

        var agentClient = await ProjectManagersFactory.CreateAgentClient(_logger, false,
            installerSettings.InstallFolder, _messagesDataManager, request.UserName, cancellationToken);

        if (agentClient is null)
            return await Task.FromResult(new[] { ProjectsErrors.AgentClientDoesNotCreated });

        if (await agentClient.StartService(request.ProjectName, request.EnvironmentName, cancellationToken))
            return new Unit();

        var err = ProjectsErrors.CannotBeStartedService(request.ProjectName);

        _logger.LogError(err.ErrorMessage);
        return await Task.FromResult(new[] { err });
    }
}