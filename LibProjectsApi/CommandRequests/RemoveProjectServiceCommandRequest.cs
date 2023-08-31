using MessagingAbstractions;

namespace LibProjectsApi.CommandRequests;

public sealed class RemoveProjectServiceCommandRequest : ICommand
{
    private RemoveProjectServiceCommandRequest(string projectName, string? serviceName, string environmentName,
        string? userName)
    {
        ProjectName = projectName;
        ServiceName = serviceName;
        EnvironmentName = environmentName;
        UserName = userName;
    }

    public string ProjectName { get; set; }
    public string? ServiceName { get; set; }
    public string EnvironmentName { get; set; }
    public string? UserName { get; }

    public static RemoveProjectServiceCommandRequest Create(string projectName, string? serviceName,
        string environmentName, string? userName)
    {
        return new RemoveProjectServiceCommandRequest(projectName, serviceName, environmentName, userName);
    }
}