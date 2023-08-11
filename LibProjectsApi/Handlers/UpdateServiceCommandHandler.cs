﻿using Installer.AgentClients;
using Installer.Models;
using LibFileParameters.Models;
using LibProjectsApi.CommandRequests;
using MessagingAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneOf;
using SystemToolsShared;

namespace LibProjectsApi.Handlers;

// ReSharper disable once UnusedType.Global
public sealed class UpdateServiceCommandHandler : ICommandHandler<UpdateServiceCommandRequest, string>
{
    private readonly IConfiguration _config;
    private readonly ILogger<UpdateServiceCommandHandler> _logger;

    public UpdateServiceCommandHandler(IConfiguration config, ILogger<UpdateServiceCommandHandler> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<OneOf<string, IEnumerable<Err>>> Handle(UpdateServiceCommandRequest request,
        CancellationToken cancellationToken)
    {
        var installerSettings = InstallerSettings.Create(_config);

        var programArchiveDateMask =
            request.ProgramArchiveDateMask ?? installerSettings.ProgramArchiveDateMask;

        var programArchiveExtension =
            request.ProgramArchiveExtension ?? installerSettings.ProgramArchiveExtension;

        var parametersFileDateMask =
            request.ParametersFileDateMask ?? installerSettings.ParametersFileDateMask;

        var parametersFileExtension =
            request.ParametersFileExtension ?? installerSettings.ParametersFileExtension;

        if (request.ProjectName is null || request.EnvironmentName is null || request.ServiceName is null ||
            request.AppSettingsFileName is null ||
            request.ServiceUserName is null || programArchiveDateMask is null ||
            programArchiveExtension is null || parametersFileDateMask is null ||
            parametersFileExtension is null)
            return await Task.FromResult(new[] { ProjectsErrors.SameParametersAreEmpty });

        if (string.IsNullOrWhiteSpace(installerSettings.ProgramExchangeFileStorageName))
            return await Task.FromResult(new[] { ProjectsErrors.ProgramExchangeFileStorageNameDoesNotSpecified });

        var fileStorages = FileStorages.Create(_config);

        var fileStorageForUpload =
            fileStorages.GetFileStorageDataByKey(installerSettings.ProgramExchangeFileStorageName);
        if (fileStorageForUpload is null)
            return await Task.FromResult(new[]
                { ProjectsErrors.FileStorageDoesNotExists(installerSettings.ProgramExchangeFileStorageName) });

        var agentClient =
            AgentClientsFabric.CreateAgentClientWithFileStorage(_logger, installerSettings,
                fileStorageForUpload, false);

        if (agentClient is null)
            return await Task.FromResult(new[] { ProjectsErrors.AgentClientDoesNotCreated });

        var assemblyVersion = agentClient.InstallService(request.ProjectName, request.EnvironmentName,
            request.ServiceName,
            request.ServiceUserName, request.AppSettingsFileName, programArchiveDateMask, programArchiveExtension,
            parametersFileDateMask, parametersFileExtension);

        if (assemblyVersion != null)
            return assemblyVersion;

        var err = ProjectsErrors.CannotBeUpdatedProject(request.ProjectName);
        _logger.LogError(err.ErrorMessage);
        return await Task.FromResult(new[] { err });
    }
}