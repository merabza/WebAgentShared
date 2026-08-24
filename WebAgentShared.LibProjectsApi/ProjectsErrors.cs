using SystemTools.SystemToolsShared.Errors;

namespace WebAgentShared.LibProjectsApi;

public static class ProjectsErrors
{
    public static readonly ErrorOmd AppSettingsIsNotCreated = new()
    {
        Code = nameof(AppSettingsIsNotCreated), Name = "appSettings is not created"
    };

    public static readonly ErrorOmd SameParametersAreEmpty = new()
    {
        Code = nameof(SameParametersAreEmpty), Name = "Same Parameters are Empty"
    };

    public static readonly ErrorOmd ProgramExchangeFileStorageNameDoesNotSpecified = new()
    {
        Code = nameof(ProgramExchangeFileStorageNameDoesNotSpecified),
        Name = "ProgramExchangeFileStorageName does not specified"
    };

    public static readonly ErrorOmd AgentClientDoesNotCreated = new()
    {
        Code = nameof(AgentClientDoesNotCreated), Name = "AgentClient does not created"
    };

    public static ErrorOmd ParametersIsEmpty(string parameterName)
    {
        return new ErrorOmd { Code = nameof(ParametersIsEmpty), Name = $"Parameter {parameterName} is Empty" };
    }

    //public static readonly ErrorOmd ServiceNameIsEmpty = new()
    //    { Code = nameof(ServiceNameIsEmpty), Name = "Service Name is Empty" };

    public static ErrorOmd FileStorageDoesNotExists(string fileStorageName)
    {
        return new ErrorOmd
        {
            Code = nameof(FileStorageDoesNotExists),
            Name = $"FileStorage with name {fileStorageName} does not exists"
        };
    }

    public static ErrorOmd SettingsCannotBeUpdated(string projectName)
    {
        return new ErrorOmd
        {
            Code = nameof(SettingsCannotBeUpdated),
            Name = $"Settings cannot be updated for Project {projectName} => service can not stopped"
        };
    }

    public static ErrorOmd CannotBeUpdatedProject(string projectName)
    {
        return new ErrorOmd
        {
            Code = nameof(CannotBeUpdatedProject), Name = $"cannot be updated Project {projectName}"
        };
    }

    public static ErrorOmd CannotBeStoppedService(string projectName)
    {
        return new ErrorOmd { Code = nameof(CannotBeStoppedService), Name = $"{projectName} service can not stopped" };
    }

    public static ErrorOmd CannotBeStartedService(string projectName)
    {
        return new ErrorOmd { Code = nameof(CannotBeStartedService), Name = $"{projectName} service can not started" };
    }

    public static ErrorOmd ProjectServiceCannotBeRemoved(string projectName)
    {
        return new ErrorOmd
        {
            Code = nameof(ProjectServiceCannotBeRemoved), Name = $"Project {projectName} can not be removed"
        };
    }
}
