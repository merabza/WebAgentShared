using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ParametersManagement.LibFileParameters.Models;
using SystemTools.ApiContracts.Errors;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;
using SystemTools.SystemToolsShared;
using ToolsManagement.Installer.Models;
using ToolsManagement.Installer.ProjectManagers;
using WebAgentShared.LibProjectsApi.CommandRequests;
using WebAgentShared.LibWebAgentData;

// ReSharper disable ConvertToPrimaryConstructor

namespace WebAgentShared.LibProjectsApi.Handlers;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class ProjectUpdateCommandHandler : ICommandHandler<ProjectUpdateRequestCommand, string>
{
    private readonly IApplication _application;
    private readonly IConfiguration _config;
    private readonly ILogger<ProjectUpdateCommandHandler> _logger;
    private readonly IMessagesDataManager _messagesDataManager;

    public ProjectUpdateCommandHandler(IConfiguration config, ILogger<ProjectUpdateCommandHandler> logger,
        IMessagesDataManager messagesDataManager, IApplication application)
    {
        _config = config;
        _logger = logger;
        _messagesDataManager = messagesDataManager;
        _application = application;
    }

    public async Task<Result<string>> Handle(ProjectUpdateRequestCommand request, CancellationToken cancellationToken)
    {
        var installerSettings = InstallerSettings.Create(_config);

        string? programArchiveDateMask = request.ProgramArchiveDateMask ?? installerSettings.ProgramArchiveDateMask;

        string? programArchiveExtension = request.ProgramArchiveExtension ?? installerSettings.ProgramArchiveExtension;

        string? parametersFileDateMask = request.ParametersFileDateMask ?? installerSettings.ParametersFileDateMask;

        string? parametersFileExtension = request.ParametersFileExtension ?? installerSettings.ParametersFileExtension;

        if (request.ProjectName is null || request.EnvironmentName is null || programArchiveDateMask is null ||
            programArchiveExtension is null || parametersFileDateMask is null || parametersFileExtension is null)
        {
            return ApiErrors.SomeRequestParametersAreNotValid;
        }

        if (string.IsNullOrWhiteSpace(installerSettings.ProgramExchangeFileStorageName))
        {
            return ProjectsErrors.ProgramExchangeFileStorageNameDoesNotSpecified;
        }

        var appSettings = AppSettings.Create(_config);
        if (appSettings is null)
        {
            return ProjectsErrors.AppSettingsIsNotCreated;
        }

        var fileStorages = new FileStorages(appSettings.FileStorages);

        FileStorageData? fileStorageForUpload =
            fileStorages.GetFileStorageDataByKey(installerSettings.ProgramExchangeFileStorageName);
        if (fileStorageForUpload is null)
        {
            return ProjectsErrors.FileStorageDoesNotExists(installerSettings.ProgramExchangeFileStorageName);
        }

        IIProjectsManagerWithFileStorage? agentClient =
            await ProjectManagersFactory.CreateAgentClientWithFileStorage(_application.AppName, _logger,
                installerSettings, fileStorageForUpload, false, _messagesDataManager, request.UserName,
                cancellationToken);

        if (agentClient is null)
        {
            return ProjectsErrors.AgentClientDoesNotCreated;
        }

        Result<string> installProgramResult = await agentClient.InstallProgram(request.ProjectName,
            request.EnvironmentName, programArchiveDateMask, programArchiveExtension, parametersFileDateMask,
            parametersFileExtension, cancellationToken);

        if (installProgramResult.IsSuccess)
        {
            return installProgramResult.Value;
        }

        Error err = ProjectsErrors.CannotBeUpdatedProject(request.ProjectName);

        _logger.LogError("Project update error: {Description}", err.Description);
        return new ValidationError([.. installProgramResult.Error.ToErrorArray(), err]);
    }
}
