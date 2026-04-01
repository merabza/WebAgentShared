using SystemTools.SystemToolsShared.Errors;

namespace WebAgentShared.LibWebAgentData.ErrorModels;

public static class DbApiErrors
{
    public static readonly Error CommandTextIsEmpty = new()
    {
        Code = nameof(CommandTextIsEmpty), Name = "Command Text Is Empty"
    };

    public static readonly Error ErrorCreateDatabaseConnection = new()
    {
        Code = nameof(ErrorCreateDatabaseConnection), Name = "Database connection does not created"
    };

    public static readonly Error CannotDetectRemoteAddress = new()
    {
        Code = nameof(CannotDetectRemoteAddress), Name = "Cannot detect Remote address"
    };

    public static readonly Error ApiKeyIsInvalid = new()
    {
        Code = nameof(ApiKeyIsInvalid), Name = "API Key is invalid"
    };

    public static readonly Error InvalidServerName = new()
    {
        Code = nameof(InvalidServerName),
        Name = "Source database settings with name {serverName} does not specified"
    };

    public static readonly Error BaseBackupsLocalPatchIsEmpty = new()
    {
        Code = nameof(BaseBackupsLocalPatchIsEmpty), Name = "BaseBackupsLocalPatch does not specified in settings"
    };

    public static readonly Error InvalidDatabaseBackupParameters = new()
    {
        Code = nameof(InvalidDatabaseBackupParameters), Name = "Invalid DatabaseBackupParameters"
    };

    public static readonly Error DatabaseManagementClientDoesNotCreated = new()
    {
        Code = nameof(DatabaseManagementClientDoesNotCreated), Name = "databaseManagementClient Does Not Created"
    };

    public static readonly Error CreateBackupRequestIsInvalid = new()
    {
        Code = nameof(CreateBackupRequestIsInvalid), Name = "Create Backup Request is invalid"
    };

    public static readonly Error DatabaseBackupsFileStorageDoesNotCreated = new()
    {
        Code = nameof(DatabaseBackupsFileStorageDoesNotCreated),
        Name = "Database Backups File Storage does not created"
    };

    public static readonly Error DatabaseBackupsFileManagerDoesNotCreated = new()
    {
        Code = nameof(DatabaseBackupsFileManagerDoesNotCreated),
        Name = "Database Backups File Manager does not created"
    };

    public static readonly Error LocalFileStorageDoesNotCreated = new()
    {
        Code = nameof(LocalFileStorageDoesNotCreated), Name = "Local File Storage does not created"
    };

    public static readonly Error LocalFileManagerDoesNotCreated = new()
    {
        Code = nameof(LocalFileManagerDoesNotCreated), Name = "Local File Manager does not created"
    };

    public static readonly Error ExchangeFileManagerDoesNotCreated = new()
    {
        Code = nameof(ExchangeFileManagerDoesNotCreated), Name = "Exchange File Manager does not created"
    };

    public static readonly Error DatabaseSettingsDoesNotSpecified = new()
    {
        Code = nameof(DatabaseSettingsDoesNotSpecified), Name = "Database settings does not specified"
    };

    public static readonly Error BackupDoesNotCreated = new()
    {
        Code = nameof(BackupDoesNotCreated), Name = "Backup not Created"
    };

    public static readonly Error CanNotReceiveBackupFromExchangeStorage = new()
    {
        Code = nameof(CanNotReceiveBackupFromExchangeStorage), Name = "can not receive backup from exchange storage"
    };

    public static Error CannotCheckAndRepairDatabase(string databaseName)
    {
        return new Error
        {
            Code = nameof(CannotCheckAndRepairDatabase), Name = $"Cannot check and repair Database {databaseName}"
        };
    }

    public static Error TestConnectionFailed(string? databaseName)
    {
        return new Error
        {
            Code = nameof(TestConnectionFailed),
            Name =
                $"Test Connection Failed{(string.IsNullOrWhiteSpace(databaseName) ? "" : $" for Database {databaseName} on")}"
        };
    }

    public static Error CannotRecompileProcedures(string databaseName)
    {
        return new Error
        {
            Code = nameof(CannotRecompileProcedures), Name = $"Cannot Recompile Procedures Database {databaseName}"
        };
    }

    public static Error CouldNotExecuteCommand(string databaseName)
    {
        return new Error
        {
            Code = nameof(CouldNotExecuteCommand), Name = $"Could not Execute Command for Database {databaseName}"
        };
    }

    public static Error CanNotDownloadFile(string backupFileName)
    {
        return new Error { Code = nameof(CanNotDownloadFile), Name = $"Can not Download File {backupFileName}" };
    }

    public static Error CanNotUploadFile(string backupFileName)
    {
        return new Error { Code = nameof(CanNotUploadFile), Name = $"Can not Upload File {backupFileName}" };
    }

    public static Error CannotRestoreDatabase(string databaseName, string fileName)
    {
        return new Error
        {
            Code = nameof(CannotRestoreDatabase),
            Name = $"Cannot restore database {databaseName} from file {fileName}"
        };
    }
}
