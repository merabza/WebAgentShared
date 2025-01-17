using MessagingAbstractions;

namespace LibProjectsApi.QueryRequests;

public sealed class GetVersionQueryRequest : IQuery<string?>
{
    public int ServerSidePort { get; set; }
    public string? ApiVersionId { get; set; }
    public string? UserName { get; set; }

    public static GetVersionQueryRequest Create(int serverSidePort, string? apiVersionId, string? userName)
    {
        return new GetVersionQueryRequest
        {
            ServerSidePort = serverSidePort, ApiVersionId = apiVersionId, UserName = userName
        };
    }
}