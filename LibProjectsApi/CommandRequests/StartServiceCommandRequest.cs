using MessagingAbstractions;

namespace LibProjectsApi.CommandRequests;

public sealed class StartServiceCommandRequest : ICommand
{
    private StartServiceCommandRequest(string? serviceName, string environmentName, string? userName)
    {
        ServiceName = serviceName;
        EnvironmentName = environmentName;
        UserName = userName;
    }

    public string? ServiceName { get; set; }
    public string EnvironmentName { get; set; }
    public string? UserName { get; set; }

    public static StartServiceCommandRequest Create(string? serviceName, string environmentName, string? userName)
    {
        return new StartServiceCommandRequest(serviceName, environmentName, userName);
    }
}