using SystemTools.SharedKernel;

namespace WebAgentShared.LibWebAgentData.ErrorModels;

public static class DbApiErrors
{
    public static Error CommandTextIsEmpty => Error.Problem(nameof(CommandTextIsEmpty), "Command Text Is Empty");

    public static Error ErrorCreateDatabaseConnection =>
        Error.Problem(nameof(ErrorCreateDatabaseConnection), "Database connection does not created");

    public static Error CannotDetectRemoteAddress =>
        Error.Problem(nameof(CannotDetectRemoteAddress), "Cannot detect Remote address");

    public static Error ApiKeyIsInvalid => Error.Problem(nameof(ApiKeyIsInvalid), "API Key is invalid");

    public static Error InvalidServerName => Error.Problem(nameof(InvalidServerName),
        "Source database settings with name {serverName} does not specified");

    public static Error BaseBackupsLocalPatchIsEmpty => Error.Problem(nameof(BaseBackupsLocalPatchIsEmpty),
        "BaseBackupsLocalPatch does not specified in settings");

    public static Error InvalidDatabaseBackupParameters =>
        Error.Problem(nameof(InvalidDatabaseBackupParameters), "Invalid DatabaseBackupParameters");

    public static Error DatabaseManagementClientDoesNotCreated =>
        Error.Problem(nameof(DatabaseManagementClientDoesNotCreated), "databaseManagementClient Does Not Created");

    public static Error CreateBackupRequestIsInvalid =>
        Error.Problem(nameof(CreateBackupRequestIsInvalid), "Create Backup Request is invalid");

    public static Error DatabaseBackupsFileStorageDoesNotCreated =>
        Error.Problem(nameof(DatabaseBackupsFileStorageDoesNotCreated),
            "Database Backups File Storage does not created");

    public static Error DatabaseBackupsFileManagerDoesNotCreated =>
        Error.Problem(nameof(DatabaseBackupsFileManagerDoesNotCreated),
            "Database Backups File Manager does not created");

    public static Error LocalFileStorageDoesNotCreated =>
        Error.Problem(nameof(LocalFileStorageDoesNotCreated), "Local File Storage does not created");

    public static Error LocalFileManagerDoesNotCreated =>
        Error.Problem(nameof(LocalFileManagerDoesNotCreated), "Local File Manager does not created");

    public static Error ExchangeFileManagerDoesNotCreated =>
        Error.Problem(nameof(ExchangeFileManagerDoesNotCreated), "Exchange File Manager does not created");

    public static Error DatabaseSettingsDoesNotSpecified =>
        Error.Problem(nameof(DatabaseSettingsDoesNotSpecified), "Database settings does not specified");

    public static Error BackupDoesNotCreated => Error.Problem(nameof(BackupDoesNotCreated), "Backup not Created");

    public static Error CanNotReceiveBackupFromExchangeStorage =>
        Error.Problem(nameof(CanNotReceiveBackupFromExchangeStorage),
            "can not receive backup from exchange storage");

    public static Error CannotCheckAndRepairDatabase(string databaseName)
    {
        return Error.Problem(nameof(CannotCheckAndRepairDatabase), $"Cannot check and repair Database {databaseName}");
    }

    public static Error TestConnectionFailed(string? databaseName)
    {
        return Error.Problem(nameof(TestConnectionFailed),
            $"Test Connection Failed{(string.IsNullOrWhiteSpace(databaseName) ? "" : $" for Database {databaseName} on")}");
    }

    public static Error CannotRecompileProcedures(string databaseName)
    {
        return Error.Problem(nameof(CannotRecompileProcedures), $"Cannot Recompile Procedures Database {databaseName}");
    }

    public static Error CannotUpdateStatistics(string databaseName)
    {
        return Error.Problem(nameof(CannotUpdateStatistics), $"Cannot Update Statistics for Database {databaseName}");
    }

    public static Error CouldNotExecuteCommand(string databaseName)
    {
        return Error.Problem(nameof(CouldNotExecuteCommand), $"Could not Execute Command for Database {databaseName}");
    }

    public static Error CanNotDownloadFile(string backupFileName)
    {
        return Error.Problem(nameof(CanNotDownloadFile), $"Can not Download File {backupFileName}");
    }

    public static Error CanNotUploadFile(string backupFileName)
    {
        return Error.Problem(nameof(CanNotUploadFile), $"Can not Upload File {backupFileName}");
    }

    public static Error CannotRestoreDatabase(string databaseName, string fileName)
    {
        return Error.Problem(nameof(CannotRestoreDatabase),
            $"Cannot restore database {databaseName} from file {fileName}");
    }
}
