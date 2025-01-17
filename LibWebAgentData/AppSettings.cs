using System.Collections.Generic;
using LibApiClientParameters;
using LibDatabaseParameters;
using LibFileParameters.Models;
using LibWebAgentData.Models;
using Microsoft.Extensions.Configuration;

namespace LibWebAgentData;

public sealed class AppSettings
{
    public string? BaseBackupsLocalPatch { get; set; }
    public string? BackupsExchangeStorage { get; set; }
    public string? BackupsExchangeStorageSmartSchemaName { get; set; }

    public string? LocalSmartSchemaName { get; set; }

    //public Dictionary<string, DatabaseServerData> DatabaseServers { get; set; } = new();
    public DatabaseServerData? DatabaseServerData { get; set; } = new();

    public Dictionary<string, ApiClientSettings> ApiClients { get; set; } = new();
    public Dictionary<string, DatabaseServerConnectionData> DatabaseServerConnections { get; set; } = new();
    public Dictionary<string, SmartSchema> SmartSchemas { get; set; } = new();

    public static AppSettings? Create(IConfiguration configuration)
    {
        var appSettingsSection = configuration.GetSection("AppSettings");
        return appSettingsSection.Get<AppSettings>();
    }
}