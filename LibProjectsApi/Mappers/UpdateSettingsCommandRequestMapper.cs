using LibProjectsApi.CommandRequests;
using WebAgentProjectsApiContracts.V1.Requests;

namespace LibProjectsApi.Mappers;

public static class UpdateSettingsCommandRequestMapper
{
    public static UpdateSettingsCommandRequest AdaptTo(this UpdateSettingsRequest updateSettingsRequest)
    {
        return new UpdateSettingsCommandRequest
        {
            ProjectName = updateSettingsRequest.ProjectName,
            EnvironmentName = updateSettingsRequest.EnvironmentName,
            AppSettingsFileName = updateSettingsRequest.AppSettingsFileName,
            ParametersFileDateMask = updateSettingsRequest.ParametersFileDateMask,
            ParametersFileExtension = updateSettingsRequest.ParametersFileExtension
        };
    }
}