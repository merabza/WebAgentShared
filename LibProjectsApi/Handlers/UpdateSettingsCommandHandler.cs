using Installer.AgentClients;
using Installer.Models;
using LibFileParameters.Models;
using LibProjectsApi.CommandRequests;
using MediatR;
using MessagingAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneOf;
using SystemToolsShared;

namespace LibProjectsApi.Handlers;

// ReSharper disable once UnusedType.Global
public sealed class UpdateSettingsCommandHandler : ICommandHandler<UpdateSettingsCommandRequest>
{
    private readonly IConfiguration _config;
    private readonly ILogger<UpdateSettingsCommandHandler> _logger;

    public UpdateSettingsCommandHandler(IConfiguration config, ILogger<UpdateSettingsCommandHandler> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<OneOf<Unit, IEnumerable<Err>>> Handle(UpdateSettingsCommandRequest request,
        CancellationToken cancellationToken)
    {
        var installerSettings = InstallerSettings.Create(_config);

        var parametersFileDateMask =
            request.ParametersFileDateMask ?? installerSettings.ParametersFileDateMask;

        var parametersFileExtension =
            request.ParametersFileExtension ?? installerSettings.ParametersFileExtension;

        if (request.ProjectName is null || request.EnvironmentName is null || request.ServiceName is null ||
            request.AppSettingsFileName is null || parametersFileDateMask is null || parametersFileExtension is null)
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
            AgentClientsFabric.CreateAgentClientWithFileStorage(_logger, installerSettings, fileStorageForUpload,
                false);

        if (agentClient is null)
            return await Task.FromResult(new[] { ProjectsErrors.AgentClientDoesNotCreated });


        if (agentClient.UpdateAppParametersFile(request.ProjectName, request.EnvironmentName, request.ServiceName,
                request.AppSettingsFileName, parametersFileDateMask, parametersFileExtension))
            return new Unit();

        return await Task.FromResult(new[]
            { ProjectsErrors.SettingsCannotBeUpdated(request.ProjectName, request.ServiceName) });
    }
}