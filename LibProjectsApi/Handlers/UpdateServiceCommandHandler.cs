using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Installer.Models;
using Installer.ProjectManagers;
using LibFileParameters.Models;
using LibProjectsApi.CommandRequests;
using LibWebAgentData;
using MediatRMessagingAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneOf;
using SystemToolsShared;
using SystemToolsShared.Errors;

// ReSharper disable ConvertToPrimaryConstructor

namespace LibProjectsApi.Handlers;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class UpdateServiceCommandHandler : ICommandHandler<UpdateServiceRequestCommand, string>
{
    private readonly IConfiguration _config;
    private readonly ILogger<UpdateServiceCommandHandler> _logger;
    private readonly IMessagesDataManager _messagesDataManager;

    public UpdateServiceCommandHandler(IConfiguration config, ILogger<UpdateServiceCommandHandler> logger,
        IMessagesDataManager messagesDataManager)
    {
        _config = config;
        _logger = logger;
        _messagesDataManager = messagesDataManager;
    }

    public async Task<OneOf<string, Err[]>> Handle(UpdateServiceRequestCommand request,
        CancellationToken cancellationToken = default)
    {
        var installerSettings = InstallerSettings.Create(_config);

        var programArchiveDateMask = request.ProgramArchiveDateMask ?? installerSettings.ProgramArchiveDateMask;

        var programArchiveExtension = request.ProgramArchiveExtension ?? installerSettings.ProgramArchiveExtension;

        var parametersFileDateMask = request.ParametersFileDateMask ?? installerSettings.ParametersFileDateMask;

        var parametersFileExtension = request.ParametersFileExtension ?? installerSettings.ParametersFileExtension;

        var errors = new List<Err>();

        if (request.ProjectName is null)
            errors.Add(ProjectsErrors.ParametersIsEmpty(nameof(request.ProjectName)));

        if (request.EnvironmentName is null)
            errors.Add(ProjectsErrors.ParametersIsEmpty(nameof(request.EnvironmentName)));

        if (request.AppSettingsFileName is null)
            errors.Add(ProjectsErrors.ParametersIsEmpty(nameof(request.AppSettingsFileName)));

        if (request.ServiceUserName is null)
            errors.Add(ProjectsErrors.ParametersIsEmpty(nameof(request.ServiceUserName)));

        if (programArchiveDateMask is null)
            errors.Add(ProjectsErrors.ParametersIsEmpty(nameof(programArchiveDateMask)));

        if (programArchiveExtension is null)
            errors.Add(ProjectsErrors.ParametersIsEmpty(nameof(programArchiveExtension)));

        if (parametersFileDateMask is null)
            errors.Add(ProjectsErrors.ParametersIsEmpty(nameof(parametersFileDateMask)));

        if (parametersFileExtension is null)
            errors.Add(ProjectsErrors.ParametersIsEmpty(nameof(parametersFileExtension)));

        if (string.IsNullOrWhiteSpace(installerSettings.ProgramExchangeFileStorageName))
            errors.Add(ProjectsErrors.ProgramExchangeFileStorageNameDoesNotSpecified);

        var appSettings = AppSettings.Create(_config);
        FileStorages? fileStorages = null;
        if (appSettings is null)
            errors.Add(ProjectsErrors.AppSettingsIsNotCreated);
        else
            fileStorages = new FileStorages(appSettings.FileStorages);

        if (string.IsNullOrWhiteSpace(installerSettings.ProgramExchangeFileStorageName) || fileStorages == null)
            return await Task.FromResult(errors.ToArray());

        var fileStorageForUpload =
            fileStorages.GetFileStorageDataByKey(installerSettings.ProgramExchangeFileStorageName);
        if (fileStorageForUpload is null)
        {
            errors.Add(ProjectsErrors.FileStorageDoesNotExists(installerSettings.ProgramExchangeFileStorageName));
            return await Task.FromResult(errors.ToArray());
        }

        var agentClient = await ProjectManagersFactory.CreateAgentClientWithFileStorage(_logger, installerSettings,
            fileStorageForUpload, false, _messagesDataManager, request.UserName, cancellationToken);

        if (agentClient is null)
        {
            errors.Add(ProjectsErrors.AgentClientDoesNotCreated);
            return await Task.FromResult(errors.ToArray());
        }

        if (errors.Count > 0 || request.ProjectName is null || request.EnvironmentName is null ||
            request.ServiceUserName is null || request.AppSettingsFileName is null || programArchiveDateMask is null ||
            programArchiveExtension is null || parametersFileDateMask is null || parametersFileExtension is null)
            return await Task.FromResult(errors.ToArray());

        var installServiceResult = await agentClient.InstallService(request.ProjectName, request.EnvironmentName,
            request.ServiceUserName, request.AppSettingsFileName, programArchiveDateMask, programArchiveExtension,
            parametersFileDateMask, parametersFileExtension, request.ServiceDescriptionSignature,
            request.ProjectDescription, cancellationToken);
        if (installServiceResult.IsT1)
            return installServiceResult.AsT1;
        var assemblyVersion = installServiceResult.AsT0;

        if (assemblyVersion != null)
            return assemblyVersion;

        var err = ProjectsErrors.CannotBeUpdatedProject(request.ProjectName);

        _logger.LogError(err.ErrorMessage);
        return await Task.FromResult(new[] { err });
    }
}