using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneOf;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared;
using SystemTools.SystemToolsShared.Errors;
using ToolsManagement.Installer.Models;
using ToolsManagement.Installer.ProjectManagers;
using WebAgentShared.LibProjectsApi.CommandRequests;

// ReSharper disable ConvertToPrimaryConstructor

namespace WebAgentShared.LibProjectsApi.Handlers;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class RemoveProjectServiceCommandHandler : ICommandHandler<RemoveProjectServiceRequestCommand>
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

    public async Task<OneOf<Unit, Err[]>> Handle(RemoveProjectServiceRequestCommand request,
        CancellationToken cancellationToken)
    {
        var installerSettings = InstallerSettings.Create(_config);

        var agentClient = await ProjectManagersFactory.CreateAgentClient(_logger, false,
            installerSettings.InstallFolder, _messagesDataManager, request.UserName, cancellationToken);

        if (agentClient is null)
        {
            return await Task.FromResult(new[] { ProjectsErrors.AgentClientDoesNotCreated });
        }

        if (await agentClient.RemoveProjectAndService(request.ProjectName, request.EnvironmentName, request.IsService,
                cancellationToken))
        {
            return new Unit();
        }

        var err = ProjectsErrors.ProjectServiceCannotBeRemoved(request.ProjectName);

        _logger.LogError("Error removing project service: {ErrorMessage}", err.ErrorMessage);
        return await Task.FromResult(new[] { err });
    }
}
