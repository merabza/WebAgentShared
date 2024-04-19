using MessagingAbstractions;

namespace LibProjectsApi.CommandRequests;

public sealed class StartServiceCommandRequest : ICommand
{
    private StartServiceCommandRequest(string? projectName, string environmentName, string? userName)
    {
        ProjectName = projectName;
        EnvironmentName = environmentName;
        UserName = userName;
    }

    public string? ProjectName { get; set; }
    public string EnvironmentName { get; set; }
    public string? UserName { get; set; }

    public static StartServiceCommandRequest Create(string? projectName, string environmentName, string? userName)
    {
        return new StartServiceCommandRequest(projectName, environmentName, userName);
    }
}