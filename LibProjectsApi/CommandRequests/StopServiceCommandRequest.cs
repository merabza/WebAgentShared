using MessagingAbstractions;

namespace LibProjectsApi.CommandRequests;

public sealed class StopServiceCommandRequest : ICommand
{
    private StopServiceCommandRequest(string? projectName, string environmentName, string? userName)
    {
        ProjectName = projectName;
        EnvironmentName = environmentName;
        UserName = userName;
    }

    public string? ProjectName { get; set; }
    public string EnvironmentName { get; set; }
    public string? UserName { get; set; }

    public static StopServiceCommandRequest Create(string? projectName, string environmentName, string? userName)
    {
        return new StopServiceCommandRequest(projectName, environmentName, userName);
    }
}