using LibProjectsApi.CommandRequests;
using WebAgentContracts.V1.Requests;

namespace LibProjectsApi.Mappers;

public static class UpdateServiceCommandRequestMapper
{
    public static UpdateServiceCommandRequest AdaptTo(this UpdateServiceRequest updateServiceRequest)
    {
        return new UpdateServiceCommandRequest
        {
            ProjectName = updateServiceRequest.ProjectName,
            ServiceName = updateServiceRequest.ServiceName,
            ServiceUserName = updateServiceRequest.ServiceUserName,
            AppSettingsFileName = updateServiceRequest.AppSettingsFileName,
            ProgramArchiveDateMask = updateServiceRequest.ProgramArchiveDateMask,
            ProgramArchiveExtension = updateServiceRequest.ProgramArchiveExtension,
            ParametersFileDateMask = updateServiceRequest.ParametersFileDateMask,
            ParametersFileExtension = updateServiceRequest.ParametersFileExtension
        };
    }
}