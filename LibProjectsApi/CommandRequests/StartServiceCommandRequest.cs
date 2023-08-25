using MessagingAbstractions;

namespace LibProjectsApi.CommandRequests;

public sealed class StartServiceCommandRequest : ICommand
{
    public string? ServiceName { get; set; }
    public string? UserName { get; set; }

    public static StartServiceCommandRequest Create(string? serviceName, string? userName)
    {
        return new StartServiceCommandRequest
        {
            ServiceName = serviceName,
            UserName = userName
        };
    }
}