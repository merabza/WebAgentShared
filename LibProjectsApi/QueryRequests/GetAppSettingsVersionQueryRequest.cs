using MessagingAbstractions;

namespace LibProjectsApi.QueryRequests;

public sealed class GetAppSettingsVersionQueryRequest : IQuery<string?>
{
    public int ServerSidePort { get; set; }
    public string? ApiVersionId { get; set; }


    public static GetAppSettingsVersionQueryRequest Create(int serverSidePort, string? apiVersionId)
    {
        return new GetAppSettingsVersionQueryRequest
        {
            ServerSidePort = serverSidePort,
            ApiVersionId = apiVersionId
        };
    }
}