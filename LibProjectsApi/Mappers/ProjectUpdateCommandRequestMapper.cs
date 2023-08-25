using LibProjectsApi.CommandRequests;
using WebAgentProjectsApiContracts.V1.Requests;

namespace LibProjectsApi.Mappers;

public static class ProjectUpdateCommandRequestMapper
{
    public static ProjectUpdateCommandRequest AdaptTo(this ProjectUpdateRequest projectUpdateRequest, string? userName)
    {
        return new ProjectUpdateCommandRequest
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