using MessagingAbstractions;

namespace LibProjectsApi.CommandRequests;

public sealed class StopServiceCommandRequest : ICommand
{
    public string? ServiceName { get; set; }
    public string? UserName { get; set; }


    public static StopServiceCommandRequest Create(string? serviceName, string? userName)
    {
        return new StopServiceCommandRequest
        {
            ServiceName = serviceName,
            UserName = userName
        };
    }
}