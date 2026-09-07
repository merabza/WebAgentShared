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

    public async Task<Result> Handle(StartServiceRequestCommand request, CancellationToken cancellationToken)
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

        Result startServiceResult =
            await agentClient.StartService(request.ProjectName, request.EnvironmentName, cancellationToken);
        if (startServiceResult.IsSuccess)
        {
            return Result.Success();
        }

        Error err = ProjectsErrors.CannotBeStartedService(request.ProjectName);

        _logger.LogError("Service start failed: {Description}", err.Description);
        return new ValidationError([.. startServiceResult.Error.ToErrorArray(), err]);
    }
}
