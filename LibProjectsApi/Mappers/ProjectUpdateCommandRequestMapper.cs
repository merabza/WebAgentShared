using LibProjectsApi.CommandRequests;
using WebAgentContracts.V1.Requests;

namespace LibProjectsApi.Mappers;

public static class ProjectUpdateCommandRequestMapper
{
    public static ProjectUpdateCommandRequest AdaptTo(this ProjectUpdateRequest projectUpdateRequest)
    {
        return new ProjectUpdateCommandRequest
        {
            ProjectName = projectUpdateRequest.ProjectName,
            ProgramArchiveDateMask = projectUpdateRequest.ProgramArchiveDateMask,
            ProgramArchiveExtension = projectUpdateRequest.ProgramArchiveExtension,
            ParametersFileDateMask = projectUpdateRequest.ParametersFileDateMask,
            ParametersFileExtension = projectUpdateRequest.ParametersFileExtension
        };
    }
}