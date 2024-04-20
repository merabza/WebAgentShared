using MessagingAbstractions;

namespace LibProjectsApi.CommandRequests;

public sealed class RemoveProjectServiceCommandRequest : ICommand
{
    private RemoveProjectServiceCommandRequest(string projectName, string environmentName, bool isService,
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

    public static RemoveProjectServiceCommandRequest Create(string projectName, string environmentName, bool isService,
        string? userName)
    {
        return new RemoveProjectServiceCommandRequest(projectName, environmentName, isService, userName);
    }
}