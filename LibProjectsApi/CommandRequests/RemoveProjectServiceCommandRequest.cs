using MessagingAbstractions;

namespace LibProjectsApi.CommandRequests;

public sealed class RemoveProjectServiceCommandRequest : ICommand
{
    public RemoveProjectServiceCommandRequest(string projectName, string? serviceName, string? userName)
    {
        ServiceName = serviceName;
        ProjectName = projectName;
        UserName = userName;
    }

    public string? ServiceName { get; set; }
    public string ProjectName { get; set; }
    public string? UserName { get; }

    public static RemoveProjectServiceCommandRequest Create(string projectName, string? serviceName, string? userName)
    {
        return new RemoveProjectServiceCommandRequest(projectName, serviceName, userName);
    }
}