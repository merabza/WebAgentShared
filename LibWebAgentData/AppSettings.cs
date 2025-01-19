using System.Collections.Generic;
using LibApiClientParameters;
using LibDatabaseParameters;
using LibFileParameters.Models;
using LibWebAgentData.Models;
using Microsoft.Extensions.Configuration;

namespace LibWebAgentData;

public sealed class AppSettings
{
    public DatabasesBackupFilesExchangeParameters? DatabasesBackupFilesExchangeParameters { get; init; }
    //public string? BaseBackupsLocalPatch { get; init; }
    //public string? BackupsExchangeStorage { get; init; }
    //public string? BackupsExchangeStorageSmartSchemaName { get; init; }
    //public string? LocalSmartSchemaName { get; init; }

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