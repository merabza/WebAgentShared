using SystemToolsShared.Errors;

namespace LibProjectsApi;

public static class ProjectsErrors
{
    public static readonly Err AppSettingsIsNotCreated = new()
    {
        ErrorCode = nameof(AppSettingsIsNotCreated), ErrorMessage = "appSettings is not created"
    };

    public static readonly Err SameParametersAreEmpty = new()
    {
        ErrorCode = nameof(SameParametersAreEmpty), ErrorMessage = "Same Parameters are Empty"
    };

    public static readonly Err ProgramExchangeFileStorageNameDoesNotSpecified = new()
    {
        ErrorCode = nameof(ProgramExchangeFileStorageNameDoesNotSpecified),
        ErrorMessage = "ProgramExchangeFileStorageName does not specified"
    };

    public static readonly Err AgentClientDoesNotCreated = new()
    {
        ErrorCode = nameof(AgentClientDoesNotCreated), ErrorMessage = "AgentClient does not created"
    };

    //public static readonly Err ServiceNameIsEmpty = new()
    //    { ErrorCode = nameof(ServiceNameIsEmpty), ErrorMessage = "Service Name is Empty" };

    public static Err FileStorageDoesNotExists(string fileStorageName)
    {
        return new Err
        {
            ErrorCode = nameof(FileStorageDoesNotExists),
            ErrorMessage = $"FileStorage with name {fileStorageName} does not exists"
        };
    }

    public static Err SettingsCannotBeUpdated(string projectName)
    {
        return new Err
        {
            ErrorCode = nameof(SettingsCannotBeUpdated),
            ErrorMessage = $"Settings cannot be updated for Project {projectName} => service can not stopped"
        };
    }

    public static Err CannotBeUpdatedProject(string projectName)
    {
        return new Err
        {
            ErrorCode = nameof(CannotBeUpdatedProject), ErrorMessage = $"cannot be updated Project {projectName}"
        };
    }

    public static Err CannotBeStoppedService(string projectName)
    {
        return new Err
        {
            ErrorCode = nameof(CannotBeStoppedService), ErrorMessage = $"{projectName} service can not stopped"
        };
    }

    public static Err CannotBeStartedService(string projectName)
    {
        return new Err
        {
            ErrorCode = nameof(CannotBeStartedService), ErrorMessage = $"{projectName} service can not started"
        };
    }

    public static Err ProjectServiceCannotBeRemoved(string projectName)
    {
        return new Err
        {
            ErrorCode = nameof(ProjectServiceCannotBeRemoved),
            ErrorMessage = $"Project {projectName} can not be removed"
        };
    }
}