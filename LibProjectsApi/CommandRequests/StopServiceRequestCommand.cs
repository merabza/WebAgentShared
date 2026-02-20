using SystemTools.MediatRMessagingAbstractions;

namespace LibProjectsApi.CommandRequests;

public sealed class StopServiceRequestCommand : ICommand
{
    private StopServiceRequestCommand(string? projectName, string environmentName, string? userName)
    {
        ProjectName = projectName;
        EnvironmentName = environmentName;
        UserName = userName;
    }

    public string? ProjectName { get; set; }
    public string EnvironmentName { get; set; }
    public string? UserName { get; set; }

    public static StopServiceRequestCommand Create(string? projectName, string environmentName, string? userName)
    {
        return new StopServiceRequestCommand(projectName, environmentName, userName);
    }
}