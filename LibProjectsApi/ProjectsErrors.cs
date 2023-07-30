using SystemToolsShared;

namespace LibProjectsApi;

public static class ProjectsErrors
{
    public static readonly Err SameParametersAreEmpty = new()
        { ErrorCode = nameof(SameParametersAreEmpty), ErrorMessage = "Same Parameters are Empty" };

    public static readonly Err ProgramExchangeFileStorageNameDoesNotSpecified = new()
    {
        ErrorCode = nameof(ProgramExchangeFileStorageNameDoesNotSpecified),
        ErrorMessage = "ProgramExchangeFileStorageName does not specified"
    };

    public static readonly Err AgentClientDoesNotCreated = new()
        { ErrorCode = nameof(AgentClientDoesNotCreated), ErrorMessage = "AgentClient does not created" };

    public static readonly Err ServiceNameIsEmpty = new()
        { ErrorCode = nameof(ServiceNameIsEmpty), ErrorMessage = "Service Name is Empty" };

    public static Err FileStorageDoesNotExists(string fileStorageName)
    {
        return new Err
        {
            ErrorCode = nameof(FileStorageDoesNotExists),
            ErrorMessage = $"FileStorage with name {fileStorageName} does not exists"
        };
    }

    public static Err SettingsCannotBeUpdated(string projectName, string serviceName)
    {
        return new Err
        {
            ErrorCode = nameof(SettingsCannotBeUpdated),
            ErrorMessage =
                $"Settings cannot be updated for Project {projectName} => service {serviceName} can not stopped"
        };
    }

    public static Err CannotBeUpdatedProject(string projectName)
    {
        return new Err
            { ErrorCode = nameof(CannotBeUpdatedProject), ErrorMessage = $"cannot be updated Project {projectName}" };
    }

    public static Err CannotBeStoppedService(string serviceName)
    {
        return new Err
            { ErrorCode = nameof(CannotBeStoppedService), ErrorMessage = $"{serviceName} service can not stopped" };
    }

    public static Err CannotBeStartedService(string serviceName)
    {
        return new Err
            { ErrorCode = nameof(CannotBeStartedService), ErrorMessage = $"{serviceName} service can not started" };
    }

    public static Err ProjectServiceCannotBeRemoved(string projectName, string? serviceName)
    {
        return new Err
        {
            ErrorCode = nameof(ProjectServiceCannotBeRemoved),
            ErrorMessage =
                $"Project {projectName}{(serviceName is null ? "" : $" => service {serviceName}")} can not be removed"
        };
    }
}