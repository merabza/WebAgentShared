using MediatRMessagingAbstractions;

namespace LibProjectsApi.CommandRequests;

public sealed class ProjectUpdateRequestCommand : ICommand<string>
{
    public string? ProjectName { get; set; }
    public string? EnvironmentName { get; set; }
    public string? ProgramArchiveDateMask { get; set; }
    public string? ProgramArchiveExtension { get; set; }
    public string? ParametersFileDateMask { get; set; }
    public string? ParametersFileExtension { get; set; }
    public string? UserName { get; set; }
}