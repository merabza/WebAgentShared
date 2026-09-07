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

// ReSharper disable once UnusedType.Global
// ReSharper disable once ClassNeverInstantiated.Global
public sealed class StopServiceCommandHandler : ICommandHandler<StopServiceRequestCommand>
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

    public async Task<Result> Handle(StopServiceRequestCommand request, CancellationToken cancellationToken)
    {
        if (request.ProjectName is null)
        {
            return ProjectsErrors.SameParametersAreEmpty;
        }

        var installerSettings = InstallerSettings.Create(_config);

        IProjectsManager? agentClient = await ProjectManagersFactory.CreateAgentClient(_logger, false,
            installerSettings.InstallFolder, _messagesDataManager, request.UserName, cancellationToken);

        if (agentClient is null)
        {
            return ProjectsErrors.AgentClientDoesNotCreated;
        }

        Result stopServiceResult =
            await agentClient.StopService(request.ProjectName, request.EnvironmentName, cancellationToken);
        if (stopServiceResult.IsSuccess)
        {
            return Result.Success();
        }

        Error err = ProjectsErrors.CannotBeStoppedService(request.ProjectName);

        _logger.LogError("{Description}", err.Description);
        return new ValidationError([.. stopServiceResult.Error.ToErrorArray(), err]);
    }
}
