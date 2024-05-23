using ApiToolsShared;
using Installer.Models;
using Installer.ProjectManagers;
using LibFileParameters.Models;
using LibProjectsApi.CommandRequests;
using MessagingAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneOf;
using SignalRContracts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SystemToolsShared;

// ReSharper disable ConvertToPrimaryConstructor

namespace LibProjectsApi.Handlers;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class ProjectUpdateCommandHandler : ICommandHandler<ProjectUpdateCommandRequest, string>
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

    public async Task<OneOf<string, IEnumerable<Err>>> Handle(ProjectUpdateCommandRequest request,
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

        if (request.ProjectName is null || request.EnvironmentName is null || programArchiveDateMask is null ||
            programArchiveExtension is null || parametersFileDateMask is null || parametersFileExtension is null)
            return new[] { ApiErrors.SomeRequestParametersAreNotValid };

        if (string.IsNullOrWhiteSpace(installerSettings.ProgramExchangeFileStorageName))
            return new[] { ProjectsErrors.ProgramExchangeFileStorageNameDoesNotSpecified };

        var fileStorages = FileStorages.Create(_config);

        var fileStorageForUpload =
            fileStorages.GetFileStorageDataByKey(installerSettings.ProgramExchangeFileStorageName);
        if (fileStorageForUpload is null)
            return new[] { ProjectsErrors.FileStorageDoesNotExists(installerSettings.ProgramExchangeFileStorageName) };

        var agentClient = await ProjectManagersFabric.CreateAgentClientWithFileStorage(_logger, installerSettings,
            fileStorageForUpload, false, _messagesDataManager, request.UserName, cancellationToken);

        if (agentClient is null)
            return new[] { ProjectsErrors.AgentClientDoesNotCreated };

        var installProgramResult = await agentClient.InstallProgram(request.ProjectName, request.EnvironmentName,
            programArchiveDateMask, programArchiveExtension, parametersFileDateMask, parametersFileExtension,
            cancellationToken);

        if (installProgramResult.IsT1)
            return installProgramResult.AsT1;
        var assemblyVersion = installProgramResult.AsT0;

        if (assemblyVersion != null)
            return assemblyVersion;

        var err = ProjectsErrors.CannotBeUpdatedProject(request.ProjectName);

        _logger.LogError(err.ErrorMessage);
        return new[] { err };
    }
}