using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Installer.AgentClients;
using Installer.Models;
using LibFileParameters.Models;
using LibProjectsApi.CommandRequests;
using MediatR;
using MessagingAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneOf;
using SignalRContracts;
using SystemToolsShared;

// ReSharper disable ConvertToPrimaryConstructor

namespace LibProjectsApi.Handlers;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class UpdateSettingsCommandHandler : ICommandHandler<UpdateSettingsCommandRequest>
{
    private readonly IConfiguration _config;
    private readonly ILogger<UpdateSettingsCommandHandler> _logger;
    private readonly IMessagesDataManager _messagesDataManager;

    public UpdateSettingsCommandHandler(IConfiguration config, ILogger<UpdateSettingsCommandHandler> logger,
        IMessagesDataManager messagesDataManager)
    {
        _config = config;
        _logger = logger;
        _messagesDataManager = messagesDataManager;
    }

    public async Task<OneOf<Unit, IEnumerable<Err>>> Handle(UpdateSettingsCommandRequest request,
        CancellationToken cancellationToken)
    {
        await _messagesDataManager.SendMessage(request.UserName, "Creating installer settings", cancellationToken);

        var installerSettings = InstallerSettings.Create(_config);

        var parametersFileDateMask =
            request.ParametersFileDateMask ?? installerSettings.ParametersFileDateMask;

        var parametersFileExtension =
            request.ParametersFileExtension ?? installerSettings.ParametersFileExtension;

        if (request.ProjectName is null || request.EnvironmentName is null || request.AppSettingsFileName is null ||
            parametersFileDateMask is null || parametersFileExtension is null)
            return await Task.FromResult(new[] { ProjectsErrors.SameParametersAreEmpty });

        if (string.IsNullOrWhiteSpace(installerSettings.ProgramExchangeFileStorageName))
            return await Task.FromResult(new[] { ProjectsErrors.ProgramExchangeFileStorageNameDoesNotSpecified });

        await _messagesDataManager.SendMessage(request.UserName, "Creating File Storages", cancellationToken);

        var fileStorages = FileStorages.Create(_config);

        var fileStorageForUpload =
            fileStorages.GetFileStorageDataByKey(installerSettings.ProgramExchangeFileStorageName);

        if (fileStorageForUpload is null)
            return await Task.FromResult(new[]
                { ProjectsErrors.FileStorageDoesNotExists(installerSettings.ProgramExchangeFileStorageName) });

        var agentClient = await AgentClientsFabric.CreateAgentClientWithFileStorage(_logger, installerSettings,
            fileStorageForUpload, false, _messagesDataManager, request.UserName, cancellationToken);

        if (agentClient is null)
            return await Task.FromResult(new[] { ProjectsErrors.AgentClientDoesNotCreated });


        var updateAppParametersFileResult = await agentClient.UpdateAppParametersFile(request.ProjectName,
            request.EnvironmentName, request.AppSettingsFileName, parametersFileDateMask, parametersFileExtension,
            cancellationToken);
        if (updateAppParametersFileResult.IsNone)
            return new Unit();

        var err = ProjectsErrors.SettingsCannotBeUpdated(request.ProjectName);

        _logger.LogError(err.ErrorMessage);
        return await Task.FromResult(new[] { err });
    }
}