using MessagingAbstractions;

namespace LibProjectsApi.CommandRequests;

public sealed class StartServiceCommandRequest : ICommand
{
    public string? ServiceName { get; set; }


    public static StartServiceCommandRequest Create(string? serviceName)
    {
        return new StartServiceCommandRequest
        {
            ServiceName = serviceName
        };
    }
}