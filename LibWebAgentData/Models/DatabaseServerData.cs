namespace LibWebAgentData.Models;

public sealed class DatabaseServerData
{
    public string? DatabaseBackupsFileStorageName { get; set; }
    public string? DbConnectionName { get; set; }
    public string? DbWebAgentName { get; set; }
    public string? DbSmartSchemaName { get; set; }
}