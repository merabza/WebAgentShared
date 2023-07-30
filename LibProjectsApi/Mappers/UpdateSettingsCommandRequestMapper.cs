using LibProjectsApi.CommandRequests;
using WebAgentContracts.V1.Requests;

namespace LibProjectsApi.Mappers;

public static class UpdateSettingsCommandRequestMapper
{
    public static UpdateSettingsCommandRequest AdaptTo(this UpdateSettingsRequest updateSettingsRequest)
    {
        return new UpdateSettingsCommandRequest
        {
            ProjectName = updateSettingsRequest.ProjectName,
            ServiceName = updateSettingsRequest.ServiceName,
            AppSettingsFileName = updateSettingsRequest.AppSettingsFileName,
            ParametersFileDateMask = updateSettingsRequest.ParametersFileDateMask,
            ParametersFileExtension = updateSettingsRequest.ParametersFileExtension
        };
    }
}