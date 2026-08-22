using SystemTools.SystemToolsShared.Errors;

namespace WebAgentShared.LibWebAgentData.ErrorModels;

public static class DbApiErrors
{
    public static readonly ErrorOmd CommandTextIsEmpty = new()
    {
        Code = nameof(CommandTextIsEmpty), Name = "Command Text Is Empty"
    };

    public static readonly ErrorOmd ErrorCreateDatabaseConnection = new()
    {
        Code = nameof(ErrorCreateDatabaseConnection), Name = "Database connection does not created"
    };

    public static readonly ErrorOmd CannotDetectRemoteAddress = new()
    {
        Code = nameof(CannotDetectRemoteAddress), Name = "Cannot detect Remote address"
    };

    public static readonly ErrorOmd ApiKeyIsInvalid = new()
    {
        Code = nameof(ApiKeyIsInvalid), Name = "API Key is invalid"
    };

    public static readonly ErrorOmd InvalidServerName = new()
    {
        Code = nameof(InvalidServerName),
        Name = "Source database settings with name {serverName} does not specified"
    };

    public static readonly ErrorOmd BaseBackupsLocalPatchIsEmpty = new()
    {
        Code = nameof(BaseBackupsLocalPatchIsEmpty), Name = "BaseBackupsLocalPatch does not specified in settings"
    };

    public static readonly ErrorOmd InvalidDatabaseBackupParameters = new()
    {
        Code = nameof(InvalidDatabaseBackupParameters), Name = "Invalid DatabaseBackupParameters"
    };

    public static readonly ErrorOmd DatabaseManagementClientDoesNotCreated = new()
    {
        Code = nameof(DatabaseManagementClientDoesNotCreated), Name = "databaseManagementClient Does Not Created"
    };

    public static readonly ErrorOmd CreateBackupRequestIsInvalid = new()
    {
        Code = nameof(CreateBackupRequestIsInvalid), Name = "Create Backup Request is invalid"
    };

    public static readonly ErrorOmd DatabaseBackupsFileStorageDoesNotCreated = new()
    {
        Code = nameof(DatabaseBackupsFileStorageDoesNotCreated),
        Name = "Database Backups File Storage does not created"
    };

    public static readonly ErrorOmd DatabaseBackupsFileManagerDoesNotCreated = new()
    {
        Code = nameof(DatabaseBackupsFileManagerDoesNotCreated),
        Name = "Database Backups File Manager does not created"
    };

    public static readonly ErrorOmd LocalFileStorageDoesNotCreated = new()
    {
        Code = nameof(LocalFileStorageDoesNotCreated), Name = "Local File Storage does not created"
    };

    public static readonly ErrorOmd LocalFileManagerDoesNotCreated = new()
    {
        Code = nameof(LocalFileManagerDoesNotCreated), Name = "Local File Manager does not created"
    };

    public static readonly ErrorOmd ExchangeFileManagerDoesNotCreated = new()
    {
        Code = nameof(ExchangeFileManagerDoesNotCreated), Name = "Exchange File Manager does not created"
    };

    public static readonly ErrorOmd DatabaseSettingsDoesNotSpecified = new()
    {
        Code = nameof(DatabaseSettingsDoesNotSpecified), Name = "Database settings does not specified"
    };

    public static readonly ErrorOmd BackupDoesNotCreated = new()
    {
        Code = nameof(BackupDoesNotCreated), Name = "Backup not Created"
    };

    public static readonly ErrorOmd CanNotReceiveBackupFromExchangeStorage = new()
    {
        Code = nameof(CanNotReceiveBackupFromExchangeStorage), Name = "can not receive backup from exchange storage"
    };

    public static ErrorOmd CannotCheckAndRepairDatabase(string databaseName)
    {
        return new ErrorOmd
        {
            Code = nameof(CannotCheckAndRepairDatabase), Name = $"Cannot check and repair Database {databaseName}"
        };
    }

    public static ErrorOmd TestConnectionFailed(string? databaseName)
    {
        return new ErrorOmd
        {
            Code = nameof(TestConnectionFailed),
            Name =
                $"Test Connection Failed{(string.IsNullOrWhiteSpace(databaseName) ? "" : $" for Database {databaseName} on")}"
        };
    }

    public static ErrorOmd CannotRecompileProcedures(string databaseName)
    {
        return new ErrorOmd
        {
            Code = nameof(CannotRecompileProcedures), Name = $"Cannot Recompile Procedures Database {databaseName}"
        };
    }

    public static ErrorOmd CouldNotExecuteCommand(string databaseName)
    {
        return new ErrorOmd
        {
            Code = nameof(CouldNotExecuteCommand), Name = $"Could not Execute Command for Database {databaseName}"
        };
    }

    public static ErrorOmd CanNotDownloadFile(string backupFileName)
    {
        return new ErrorOmd { Code = nameof(CanNotDownloadFile), Name = $"Can not Download File {backupFileName}" };
    }

    public static ErrorOmd CanNotUploadFile(string backupFileName)
    {
        return new ErrorOmd { Code = nameof(CanNotUploadFile), Name = $"Can not Upload File {backupFileName}" };
    }

    public static ErrorOmd CannotRestoreDatabase(string databaseName, string fileName)
    {
        return new ErrorOmd
        {
            Code = nameof(CannotRestoreDatabase),
            Name = $"Cannot restore database {databaseName} from file {fileName}"
        };
    }
}
