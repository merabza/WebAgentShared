using LibProjectsApi.CommandRequests;
using WebAgentContracts.WebAgentProjectsApiContracts.V1.Requests;

namespace LibProjectsApi.Mappers;

public static class UpdateServiceCommandRequestMapper
{
    public static UpdateServiceRequestCommand AdaptTo(this UpdateServiceRequest updateServiceRequest, string? userName)
    {
        return new UpdateServiceRequestCommand
        {
            ProjectName = updateServiceRequest.ProjectName,
            EnvironmentName = updateServiceRequest.EnvironmentName,
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