using LibProjectsApi.CommandRequests;
using WebAgentProjectsApiContracts.V1.Requests;

namespace LibProjectsApi.Mappers;

public static class UpdateServiceCommandRequestMapper
{
    public static UpdateServiceCommandRequest AdaptTo(this UpdateServiceRequest updateServiceRequest, string? userName)
    {
        return new UpdateServiceCommandRequest
        {
            ProjectName = updateServiceRequest.ProjectName,
            EnvironmentName = updateServiceRequest.EnvironmentName,
            ServiceName = updateServiceRequest.ServiceName,
            ServiceUserName = updateServiceRequest.ServiceUserName,
            AppSettingsFileName = updateServiceRequest.AppSettingsFileName,
            ProgramArchiveDateMask = updateServiceRequest.ProgramArchiveDateMask,
            ProgramArchiveExtension = updateServiceRequest.ProgramArchiveExtension,
            ParametersFileDateMask = updateServiceRequest.ParametersFileDateMask,
            ParametersFileExtension = updateServiceRequest.ParametersFileExtension,
            ServiceDescriptionSignature = updateServiceRequest.ServiceDescriptionSignature,
            ProjectDescription = updateServiceRequest.ProjectDescription,
            UserName = userName
        };
    }
}