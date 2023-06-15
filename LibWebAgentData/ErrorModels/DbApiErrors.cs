using SystemToolsShared;

namespace LibWebAgentData.ErrorModels;

public static class DbApiErrors
{
    public static readonly Err CommandTextIsEmpty = new()
        { ErrorCode = nameof(CommandTextIsEmpty), ErrorMessage = "Command Text Is Empty" };

    public static readonly Err ErrorCreateDatabaseConnection = new()
        { ErrorCode = nameof(ErrorCreateDatabaseConnection), ErrorMessage = "Database connection does not created" };

    public static readonly Err CannotDetectRemoteAddress = new()
        { ErrorCode = nameof(CannotDetectRemoteAddress), ErrorMessage = "Cannot detect Remote address" };

    public static readonly Err ApiKeyIsInvalid = new()
        { ErrorCode = nameof(ApiKeyIsInvalid), ErrorMessage = "API Key is invalid" };

    public static readonly Err InvalidServerName = new()
    {
        ErrorCode = nameof(InvalidServerName),
        ErrorMessage = "Source database settings with name {serverName} does not specified"
    };

    public static readonly Err BaseBackupsLocalPatchIsEmpty = new()
    {
        ErrorCode = nameof(BaseBackupsLocalPatchIsEmpty),
        ErrorMessage = "BaseBackupsLocalPatch does not specified in settings"
    };

    public static readonly Err InvalidDatabaseBackupParameters = new()
        { ErrorCode = nameof(InvalidDatabaseBackupParameters), ErrorMessage = "Invalid DatabaseBackupParameters" };

    public static readonly Err DatabaseManagementClientDoesNotCreated = new()
    {
        ErrorCode = nameof(DatabaseManagementClientDoesNotCreated),
        ErrorMessage = "databaseManagementClient Does Not Created"
    };

    public static readonly Err CreateBackupRequestIsInvalid = new()
        { ErrorCode = nameof(CreateBackupRequestIsInvalid), ErrorMessage = "Create Backup Request is invalid" };

    public static readonly Err DatabaseBackupsFileStorageDoesNotCreated = new()
    {
        ErrorCode = nameof(DatabaseBackupsFileStorageDoesNotCreated),
        ErrorMessage = "Database Backups File Storage does not created"
    };

    public static readonly Err DatabaseBackupsFileManagerDoesNotCreated = new()
    {
        ErrorCode = nameof(DatabaseBackupsFileManagerDoesNotCreated),
        ErrorMessage = "Database Backups File Manager does not created"
    };

    public static readonly Err LocalFileStorageDoesNotCreated = new()
        { ErrorCode = nameof(LocalFileStorageDoesNotCreated), ErrorMessage = "Local File Storage does not created" };

    public static readonly Err LocalFileManagerDoesNotCreated = new()
        { ErrorCode = nameof(LocalFileManagerDoesNotCreated), ErrorMessage = "Local File Manager does not created" };

    public static readonly Err ExchangeFileManagerDoesNotCreated = new()
    {
        ErrorCode = nameof(ExchangeFileManagerDoesNotCreated), ErrorMessage = "Exchange File Manager does not created"
    };

    public static Err DatabaseSettingsDoesNotSpecified = new()
        { ErrorCode = nameof(DatabaseSettingsDoesNotSpecified), ErrorMessage = "Database settings does not specified" };

    public static readonly Err BackupDoesNotCreated = new()
        { ErrorCode = nameof(BackupDoesNotCreated), ErrorMessage = "Backup not Created" };

    public static readonly Err CanNotReceiveBackupFromExchangeStorage = new()
    {
        ErrorCode = nameof(CanNotReceiveBackupFromExchangeStorage),
        ErrorMessage = "can not receive backup from exchange storage"
    };

    public static Err CannotCheckAndRepairDatabase(string databaseName)
    {
        return new Err
        {
            ErrorCode = nameof(CannotCheckAndRepairDatabase),
            ErrorMessage = $"Cannot check and repair Database {databaseName}"
        };
    }

    public static Err TestConnectionFailed(string? databaseName)
    {
        return new Err
        {
            ErrorCode = nameof(TestConnectionFailed),
            ErrorMessage =
                $"Test Connection Failed{(string.IsNullOrWhiteSpace(databaseName) ? "" : $" for Database {databaseName} on")}"
        };
    }

    public static Err CannotRecompileProcedures(string databaseName)
    {
        return new Err
        {
            ErrorCode = nameof(CannotRecompileProcedures),
            ErrorMessage = $"Cannot Recompile Procedures Database {databaseName}"
        };
    }

    public static Err CouldNotExecuteCommand(string databaseName)
    {
        return new Err
        {
            ErrorCode = nameof(CouldNotExecuteCommand),
            ErrorMessage = $"Could not Execute Command for Database {databaseName}"
        };
    }

    public static Err CanNotDownloadFile(string backupFileName)
    {
        return new Err
        {
            ErrorCode = nameof(CanNotDownloadFile),
            ErrorMessage = $"Can not Download File {backupFileName}"
        };
    }

    public static Err CanNotUploadFile(string backupFileName)
    {
        return new Err
        {
            ErrorCode = nameof(CanNotUploadFile),
            ErrorMessage = $"Can not Upload File {backupFileName}"
        };
    }

    public static Err CannotRestoreDatabase(string databaseName, string fileName)
    {
        return new Err
        {
            ErrorCode = nameof(CannotRestoreDatabase),
            ErrorMessage = $"Cannot restore database {databaseName} from file {fileName}"
        };
    }
}