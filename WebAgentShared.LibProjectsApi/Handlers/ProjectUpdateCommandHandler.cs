using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneOf;
using ParametersManagement.LibFileParameters.Models;
using SystemTools.ApiContracts.Errors;
using SystemTools.MediatRMessagingAbstractions;
using SystemTools.SystemToolsShared;
using SystemTools.SystemToolsShared.Errors;
using ToolsManagement.Installer.Models;
using ToolsManagement.Installer.ProjectManagers;
using WebAgentShared.LibProjectsApi.CommandRequests;
using WebAgentShared.LibWebAgentData;

// ReSharper disable ConvertToPrimaryConstructor

namespace WebAgentShared.LibProjectsApi.Handlers;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class ProjectUpdateCommandHandler : ICommandHandler<ProjectUpdateRequestCommand, string>
{
    private readonly IConfiguration _config;
    private readonly ILogger<ProjectUpdateCommandHandler> _logger;
    private readonly IMessagesDataManager _messagesDataManager;

    public ProjectUpdateCommandHandler(IConfiguration config, ILogger<ProjectUpdateCommandHandler> logger,
        IMessagesDataManager messagesDataManager)
    {
        _config = config;
        _logger = logger;
        _messagesDataManager = messagesDataManager;
    }

    public async Task<OneOf<string, Err[]>> Handle(ProjectUpdateRequestCommand request,
        CancellationToken cancellationToken)
    {
        var installerSettings = InstallerSettings.Create(_config);

        string? programArchiveDateMask = request.ProgramArchiveDateMask ?? installerSettings.ProgramArchiveDateMask;

        string? programArchiveExtension = request.ProgramArchiveExtension ?? installerSettings.ProgramArchiveExtension;

        string? parametersFileDateMask = request.ParametersFileDateMask ?? installerSettings.ParametersFileDateMask;

        string? parametersFileExtension = request.ParametersFileExtension ?? installerSettings.ParametersFileExtension;

        if (request.ProjectName is null || request.EnvironmentName is null || programArchiveDateMask is null ||
            programArchiveExtension is null || parametersFileDateMask is null || parametersFileExtension is null)
        {
            return new[] { ApiErrors.SomeRequestParametersAreNotValid };
        }

        if (string.IsNullOrWhiteSpace(installerSettings.ProgramExchangeFileStorageName))
        {
            return new[] { ProjectsErrors.ProgramExchangeFileStorageNameDoesNotSpecified };
        }

        var appSettings = AppSettings.Create(_config);
        if (appSettings is null)
        {
            return await Task.FromResult(new[] { ProjectsErrors.AppSettingsIsNotCreated });
        }

        var fileStorages = new FileStorages(appSettings.FileStorages);

        FileStorageData? fileStorageForUpload =
            fileStorages.GetFileStorageDataByKey(installerSettings.ProgramExchangeFileStorageName);
        if (fileStorageForUpload is null)
        {
            return new[] { ProjectsErrors.FileStorageDoesNotExists(installerSettings.ProgramExchangeFileStorageName) };
        }

        IIProjectsManagerWithFileStorage? agentClient =
            await ProjectManagersFactory.CreateAgentClientWithFileStorage(_logger, installerSettings,
                fileStorageForUpload, false, _messagesDataManager, request.UserName, cancellationToken);

        if (agentClient is null)
        {
            return new[] { ProjectsErrors.AgentClientDoesNotCreated };
        }

        OneOf<string, Err[]> installProgramResult = await agentClient.InstallProgram(request.ProjectName,
            request.EnvironmentName, programArchiveDateMask, programArchiveExtension, parametersFileDateMask,
            parametersFileExtension, cancellationToken);

        if (installProgramResult.IsT1)
        {
            return installProgramResult.AsT1;
        }

        string? assemblyVersion = installProgramResult.AsT0;

        if (assemblyVersion != null)
        {
            return assemblyVersion;
        }

        Err err = ProjectsErrors.CannotBeUpdatedProject(request.ProjectName);

        _logger.LogError("Project update error: {ErrorMessage}", err.ErrorMessage);
        return new[] { err };
    }
}
