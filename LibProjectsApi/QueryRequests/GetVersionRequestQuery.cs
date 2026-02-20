using SystemTools.MediatRMessagingAbstractions;

namespace LibProjectsApi.QueryRequests;

public sealed class GetVersionRequestQuery : IQuery<string?>
{
    public int ServerSidePort { get; set; }
    public string? ApiVersionId { get; set; }
    public string? UserName { get; set; }

    public static GetVersionRequestQuery Create(int serverSidePort, string? apiVersionId, string? userName)
    {
        return new GetVersionRequestQuery
        {
            ServerSidePort = serverSidePort, ApiVersionId = apiVersionId, UserName = userName
        };
    }
}