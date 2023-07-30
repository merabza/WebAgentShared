using MessagingAbstractions;

namespace LibProjectsApi.CommandRequests;

public sealed class RemoveProjectServiceCommandRequest : ICommand
{
    public RemoveProjectServiceCommandRequest(string projectName, string? serviceName)
    {
        ServiceName = serviceName;
        ProjectName = projectName;
    }

    public string? ServiceName { get; set; }
    public string ProjectName { get; set; }

    public static RemoveProjectServiceCommandRequest Create(string projectName, string? serviceName)
    {
        return new RemoveProjectServiceCommandRequest(projectName, serviceName);
    }
}