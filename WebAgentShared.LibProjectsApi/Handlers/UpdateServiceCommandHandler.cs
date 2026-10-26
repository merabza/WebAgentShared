using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ParametersManagement.LibFileParameters.Models;
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
public sealed class UpdateServiceCommandHandler : ICommandHandler<UpdateServiceRequestCommand, string>
{
    private readonly IApplication _application;
    private readonly IConfiguration _config;
    private readonly ILogger<UpdateServiceCommandHandler> _logger;
    private readonly IMessagesDataManager _messagesDataManager;

    public UpdateServiceCommandHandler(IConfiguration config, ILogger<UpdateServiceCommandHandler> logger,
        IMessagesDataManager messagesDataManager, IApplication application)
    {
        _config = config;
        _logger = logger;
        _messagesDataManager = messagesDataManager;
        _application = application;
    }

    public async Task<Result<string>> Handle(UpdateServiceRequestCommand request, CancellationToken cancellationToken)
    {
        var installerSettings = InstallerSettings.Create(_config);

        string? programArchiveDateMask = request.ProgramArchiveDateMask ?? installerSettings.ProgramArchiveDateMask;

        string? programArchiveExtension = request.ProgramArchiveExtension ?? installerSettings.ProgramArchiveExtension;

        string? parametersFileDateMask = request.ParametersFileDateMask ?? installerSettings.ParametersFileDateMask;

        string? parametersFileExtension = request.ParametersFileExtension ?? installerSettings.ParametersFileExtension;

        var errors = new List<Error>();

        if (request.ProjectName is null)
        {
            errors.Add(ProjectsErrors.ParametersIsEmpty(nameof(request.ProjectName)));
        }

        if (request.EnvironmentName is null)
        {
            errors.Add(ProjectsErrors.ParametersIsEmpty(nameof(request.EnvironmentName)));
        }

        if (request.AppSettingsFileName is null)
        {
            errors.Add(ProjectsErrors.ParametersIsEmpty(nameof(request.AppSettingsFileName)));
        }

        if (request.ServiceUserName is null)
        {
            errors.Add(ProjectsErrors.ParametersIsEmpty(nameof(request.ServiceUserName)));
        }

        if (programArchiveDateMask is null)
        {
            errors.Add(ProjectsErrors.ParametersIsEmpty(nameof(programArchiveDateMask)));
        }

        if (programArchiveExtension is null)
        {
            errors.Add(ProjectsErrors.ParametersIsEmpty(nameof(programArchiveExtension)));
        }

        if (parametersFileDateMask is null)
        {
            errors.Add(ProjectsErrors.ParametersIsEmpty(nameof(parametersFileDateMask)));
        }

        if (parametersFileExtension is null)
        {
            errors.Add(ProjectsErrors.ParametersIsEmpty(nameof(parametersFileExtension)));
        }

        if (string.IsNullOrWhiteSpace(installerSettings.ProgramExchangeFileStorageName))
        {
            errors.Add(ProjectsErrors.ProgramExchangeFileStorageNameDoesNotSpecified);
        }

        var appSettings = AppSettings.Create(_config);
        FileStorages? fileStorages = null;
        if (appSettings is null)
        {
            errors.Add(ProjectsErrors.AppSettingsIsNotCreated);
        }
        else
        {
            fileStorages = new FileStorages(appSettings.FileStorages);
        }

        if (string.IsNullOrWhiteSpace(installerSettings.ProgramExchangeFileStorageName) || fileStorages == null)
        {
            return new ValidationError([.. errors]);
        }

        FileStorageData? fileStorageForUpload =
            fileStorages.GetFileStorageDataByKey(installerSettings.ProgramExchangeFileStorageName);
        if (fileStorageForUpload is null)
        {
            errors.Add(ProjectsErrors.FileStorageDoesNotExists(installerSettings.ProgramExchangeFileStorageName));
            return new ValidationError([.. errors]);
        }

        IIProjectsManagerWithFileStorage? agentClient =
            await ProjectManagersFactory.CreateAgentClientWithFileStorage(_application.AppName, _logger,
                installerSettings, fileStorageForUpload, false, _messagesDataManager, request.UserName,
                cancellationToken);

        if (agentClient is null)
        {
            errors.Add(ProjectsErrors.AgentClientDoesNotCreated);
            return new ValidationError([.. errors]);
        }

        if (errors.Count > 0 || request.ProjectName is null || request.EnvironmentName is null ||
            request.ServiceUserName is null || request.AppSettingsFileName is null || programArchiveDateMask is null ||
            programArchiveExtension is null || parametersFileDateMask is null || parametersFileExtension is null)
        {
            return new ValidationError([.. errors]);
        }

        Result<string> installServiceResult = await agentClient.InstallService(request.ProjectName,
            request.EnvironmentName, request.ServiceUserName, request.AppSettingsFileName, programArchiveDateMask,
            programArchiveExtension, parametersFileDateMask, parametersFileExtension,
            request.ServiceDescriptionSignature, request.ProjectDescription, request.PortsToOpen ?? [],
            cancellationToken);
        if (installServiceResult.IsSuccess)
        {
            return installServiceResult.Value;
        }

        Error err = ProjectsErrors.CannotBeUpdatedProject(request.ProjectName);

        _logger.LogError("Error: {Description}", err.Description);
        return new ValidationError([.. installServiceResult.Error.ToErrorArray(), err]);
    }
}
