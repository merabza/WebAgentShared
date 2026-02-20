using SystemTools.MediatRMessagingAbstractions;

namespace LibProjectsApi.CommandRequests;

public sealed class StartServiceRequestCommand : ICommand
{
    private StartServiceRequestCommand(string? projectName, string environmentName, string? userName)
    {
        ProjectName = projectName;
        EnvironmentName = environmentName;
        UserName = userName;
    }

    public string? ProjectName { get; set; }
    public string EnvironmentName { get; set; }
    public string? UserName { get; set; }

    public static StartServiceRequestCommand Create(string? projectName, string environmentName, string? userName)
    {
        return new StartServiceRequestCommand(projectName, environmentName, userName);
    }
}