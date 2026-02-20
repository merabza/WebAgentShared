using LibProjectsApi.CommandRequests;
using WebAgentContracts.WebAgentProjectsApiContracts.V1.Requests;

namespace LibProjectsApi.Mappers;

public static class ProjectUpdateCommandRequestMapper
{
    public static ProjectUpdateRequestCommand AdaptTo(this ProjectUpdateRequest projectUpdateRequest, string? userName)
    {
        return new ProjectUpdateRequestCommand
        {
            ProjectName = projectUpdateRequest.ProjectName,
            EnvironmentName = projectUpdateRequest.EnvironmentName,
            ProgramArchiveDateMask = projectUpdateRequest.ProgramArchiveDateMask,
            ProgramArchiveExtension = projectUpdateRequest.ProgramArchiveExtension,
            ParametersFileDateMask = projectUpdateRequest.ParametersFileDateMask,
            ParametersFileExtension = projectUpdateRequest.ParametersFileExtension,
            UserName = userName
        };
    }
}