using SystemTools.SharedKernel;

namespace WebAgentShared.LibProjectsApi;

public static class ProjectsErrors
{
    public static Error AppSettingsIsNotCreated =>
        Error.Problem(nameof(AppSettingsIsNotCreated), "appSettings is not created");

    public static Error SameParametersAreEmpty =>
        Error.Problem(nameof(SameParametersAreEmpty), "Same Parameters are Empty");

    public static Error ProgramExchangeFileStorageNameDoesNotSpecified =>
        Error.Problem(nameof(ProgramExchangeFileStorageNameDoesNotSpecified),
            "ProgramExchangeFileStorageName does not specified");

    public static Error AgentClientDoesNotCreated =>
        Error.Problem(nameof(AgentClientDoesNotCreated), "AgentClient does not created");

    public static Error GetVersionUnknownError =>
        Error.Problem(nameof(GetVersionUnknownError), "Unknown Error returned GetVersion");

    public static Error ParametersIsEmpty(string parameterName)
    {
        return Error.Problem(nameof(ParametersIsEmpty), $"Parameter {parameterName} is Empty");
    }

    //public static readonly ErrorOmd ServiceNameIsEmpty = new()
    //    { Code = nameof(ServiceNameIsEmpty), Name = "Service Name is Empty" };

    public static Error FileStorageDoesNotExists(string fileStorageName)
    {
        return Error.Problem(nameof(FileStorageDoesNotExists),
            $"FileStorage with name {fileStorageName} does not exists");
    }

    public static Error SettingsCannotBeUpdated(string projectName)
    {
        return Error.Problem(nameof(SettingsCannotBeUpdated),
            $"Settings cannot be updated for Project {projectName} => service can not stopped");
    }

    public static Error CannotBeUpdatedProject(string projectName)
    {
        return Error.Problem(nameof(CannotBeUpdatedProject), $"cannot be updated Project {projectName}");
    }

    public static Error CannotBeStoppedService(string projectName)
    {
        return Error.Problem(nameof(CannotBeStoppedService), $"{projectName} service can not stopped");
    }

    public static Error CannotBeStartedService(string projectName)
    {
        return Error.Problem(nameof(CannotBeStartedService), $"{projectName} service can not started");
    }

    public static Error ProjectServiceCannotBeRemoved(string projectName)
    {
        return Error.Problem(nameof(ProjectServiceCannotBeRemoved), $"Project {projectName} can not be removed");
    }
}
