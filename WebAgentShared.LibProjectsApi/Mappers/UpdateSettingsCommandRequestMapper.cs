using WebAgentContracts.WebAgentProjectsApiContracts.V1.Requests;
using WebAgentShared.LibProjectsApi.CommandRequests;

namespace WebAgentShared.LibProjectsApi.Mappers;

public static class UpdateSettingsCommandRequestMapper
{
    public static UpdateSettingsRequestCommand AdaptTo(this UpdateSettingsRequest updateSettingsRequest,
        string? userName)
    {
        return new UpdateSettingsRequestCommand
        {
            ProjectName = updateSettingsRequest.ProjectName,
            EnvironmentName = updateSettingsRequest.EnvironmentName,
            AppSettingsFileName = updateSettingsRequest.AppSettingsFileName,
            ParametersFileDateMask = updateSettingsRequest.ParametersFileDateMask,
            ParametersFileExtension = updateSettingsRequest.ParametersFileExtension,
            UserName = userName
        };
    }
}
