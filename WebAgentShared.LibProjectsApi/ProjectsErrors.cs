using SystemTools.SystemToolsShared.Errors;

namespace WebAgentShared.LibProjectsApi;

public static class ProjectsErrors
{
    public static readonly Error AppSettingsIsNotCreated = new()
    {
        Code = nameof(AppSettingsIsNotCreated), Name = "appSettings is not created"
    };

    public static readonly Error SameParametersAreEmpty = new()
    {
        Code = nameof(SameParametersAreEmpty), Name = "Same Parameters are Empty"
    };

    public static readonly Error ProgramExchangeFileStorageNameDoesNotSpecified = new()
    {
        Code = nameof(ProgramExchangeFileStorageNameDoesNotSpecified),
        Name = "ProgramExchangeFileStorageName does not specified"
    };

    public static readonly Error AgentClientDoesNotCreated = new()
    {
        Code = nameof(AgentClientDoesNotCreated), Name = "AgentClient does not created"
    };

    public static Error ParametersIsEmpty(string parameterName)
    {
        return new Error { Code = nameof(ParametersIsEmpty), Name = $"Parameter {parameterName} is Empty" };
    }

    //public static readonly Error ServiceNameIsEmpty = new()
    //    { Code = nameof(ServiceNameIsEmpty), Name = "Service Name is Empty" };

    public static Error FileStorageDoesNotExists(string fileStorageName)
    {
        return new Error
        {
            Code = nameof(FileStorageDoesNotExists),
            Name = $"FileStorage with name {fileStorageName} does not exists"
        };
    }

    public static Error SettingsCannotBeUpdated(string projectName)
    {
        return new Error
        {
            Code = nameof(SettingsCannotBeUpdated),
            Name = $"Settings cannot be updated for Project {projectName} => service can not stopped"
        };
    }

    public static Error CannotBeUpdatedProject(string projectName)
    {
        return new Error { Code = nameof(CannotBeUpdatedProject), Name = $"cannot be updated Project {projectName}" };
    }

    public static Error CannotBeStoppedService(string projectName)
    {
        return new Error { Code = nameof(CannotBeStoppedService), Name = $"{projectName} service can not stopped" };
    }

    public static Error CannotBeStartedService(string projectName)
    {
        return new Error { Code = nameof(CannotBeStartedService), Name = $"{projectName} service can not started" };
    }

    public static Error ProjectServiceCannotBeRemoved(string projectName)
    {
        return new Error
        {
            Code = nameof(ProjectServiceCannotBeRemoved), Name = $"Project {projectName} can not be removed"
        };
    }
}
