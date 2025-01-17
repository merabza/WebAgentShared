using MessagingAbstractions;

namespace LibProjectsApi.QueryRequests;

public sealed class GetAppSettingsVersionQueryRequest : IQuery<string>
{
    public int ServerSidePort { get; set; }
    public string? ApiVersionId { get; set; }
    public string? UserName { get; set; }


    public static GetAppSettingsVersionQueryRequest Create(int serverSidePort, string? apiVersionId, string? userName)
    {
        return new GetAppSettingsVersionQueryRequest
        {
            ServerSidePort = serverSidePort,
            ApiVersionId = apiVersionId,
            UserName = userName
        };
    }
}