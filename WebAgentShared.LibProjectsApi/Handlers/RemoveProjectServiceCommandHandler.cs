using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;
using SystemTools.SystemToolsShared;
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

    public async Task<Result> Handle(RemoveProjectServiceRequestCommand request, CancellationToken cancellationToken)
    {
        var installerSettings = InstallerSettings.Create(_config);

        IProjectsManager? agentClient = await ProjectManagersFactory.CreateAgentClient(_logger, false,
            installerSettings.InstallFolder, _messagesDataManager, request.UserName, cancellationToken);

        if (agentClient is null)
        {
            return ProjectsErrors.AgentClientDoesNotCreated;
        }

        Result removeProjectAndServiceResult = await agentClient.RemoveProjectAndService(request.ProjectName,
            request.EnvironmentName, request.IsService, cancellationToken);
        if (removeProjectAndServiceResult.IsSuccess)
        {
            return Result.Success();
        }

        Error err = ProjectsErrors.ProjectServiceCannotBeRemoved(request.ProjectName);

        _logger.LogError("Error removing project service: {Description}", err.Description);
        return new ValidationError([.. removeProjectAndServiceResult.Error.ToErrorArray(), err]);
    }
}
