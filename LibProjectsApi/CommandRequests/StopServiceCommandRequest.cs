using MessagingAbstractions;

namespace LibProjectsApi.CommandRequests;

public sealed class StopServiceCommandRequest : ICommand
{
    private StopServiceCommandRequest(string? serviceName, string environmentName, string? userName)
    {
        ServiceName = serviceName;
        EnvironmentName = environmentName;
        UserName = userName;
    }

    public string? ServiceName { get; set; }
    public string EnvironmentName { get; set; }
    public string? UserName { get; set; }

    public static StopServiceCommandRequest Create(string? serviceName, string environmentName, string? userName)
    {
        return new StopServiceCommandRequest(serviceName, environmentName, userName);
    }
}