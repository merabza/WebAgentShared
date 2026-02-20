using System.Collections.Generic;
using LibWebAgentData.Models;
using Microsoft.Extensions.Configuration;
using ParametersManagement.LibApiClientParameters;
using ParametersManagement.LibDatabaseParameters;
using ParametersManagement.LibFileParameters.Models;

namespace LibWebAgentData;

public sealed class AppSettings
{
    public DatabasesBackupFilesExchangeParameters? DatabasesBackupFilesExchangeParameters { get; init; }
    public DatabaseServerData? DatabaseServerData { get; init; } = new();
    public Dictionary<string, ApiClientSettings> ApiClients { get; init; } = new();
    public Dictionary<string, FileStorageData> FileStorages { get; init; } = new();
    public Dictionary<string, DatabaseServerConnectionData> DatabaseServerConnections { get; init; } = new();
    public Dictionary<string, SmartSchema> SmartSchemas { get; init; } = new();

    public static AppSettings? Create(IConfiguration configuration)
    {
        var appSettingsSection = configuration.GetSection("AppSettings");
        return appSettingsSection.Get<AppSettings>();
    }
}