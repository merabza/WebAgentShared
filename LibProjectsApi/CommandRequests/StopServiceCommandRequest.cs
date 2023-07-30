using MessagingAbstractions;

namespace LibProjectsApi.CommandRequests;

public sealed class StopServiceCommandRequest : ICommand
{
    public string? ServiceName { get; set; }


    public static StopServiceCommandRequest Create(string? serviceName)
    {
        return new StopServiceCommandRequest
        {
            ServiceName = serviceName
        };
    }
}