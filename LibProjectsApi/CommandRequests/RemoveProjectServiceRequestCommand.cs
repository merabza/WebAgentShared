using MediatRMessagingAbstractions;

namespace LibProjectsApi.CommandRequests;

public sealed class RemoveProjectServiceRequestCommand : ICommand
{
    private RemoveProjectServiceRequestCommand(string projectName, string environmentName, bool isService,
        string? userName)
    {
        ProjectName = projectName;
        IsService = isService;
        EnvironmentName = environmentName;
        UserName = userName;
    }

    public string ProjectName { get; set; }
    public bool IsService { get; set; }
    public string EnvironmentName { get; set; }
    public string? UserName { get; }

    public static RemoveProjectServiceRequestCommand Create(string projectName, string environmentName, bool isService,
        string? userName)
    {
        return new RemoveProjectServiceRequestCommand(projectName, environmentName, isService, userName);
    }
}