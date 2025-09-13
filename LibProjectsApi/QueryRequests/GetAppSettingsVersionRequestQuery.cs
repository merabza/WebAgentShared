using MediatRMessagingAbstractions;

namespace LibProjectsApi.QueryRequests;

public sealed class GetAppSettingsVersionRequestQuery : IQuery<string>
{
    public int ServerSidePort { get; set; }
    public string? ApiVersionId { get; set; }
    public string? UserName { get; set; }

    public static GetAppSettingsVersionRequestQuery Create(int serverSidePort, string? apiVersionId, string? userName)
    {
        return new GetAppSettingsVersionRequestQuery
        {
            ServerSidePort = serverSidePort, ApiVersionId = apiVersionId, UserName = userName
        };
    }
}