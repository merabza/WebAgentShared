using MessagingAbstractions;

namespace LibProjectsApi.QueryRequests;

public sealed class GetVersionQueryRequest : IQuery<string?>
{
    public int ServerSidePort { get; set; }
    public string? ApiVersionId { get; set; }


    public static GetVersionQueryRequest Create(int serverSidePort, string? apiVersionId)
    {
        return new GetVersionQueryRequest
        {
            ServerSidePort = serverSidePort,
            ApiVersionId = apiVersionId
        };
    }
}