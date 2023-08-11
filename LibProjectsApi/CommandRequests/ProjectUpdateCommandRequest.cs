using MessagingAbstractions;

namespace LibProjectsApi.CommandRequests;

public sealed class ProjectUpdateCommandRequest : ICommand<string>
{
    public string? ProjectName { get; set; }
    public string? EnvironmentName { get; set; }
    public string? ProgramArchiveDateMask { get; set; }
    public string? ProgramArchiveExtension { get; set; }
    public string? ParametersFileDateMask { get; set; }
    public string? ParametersFileExtension { get; set; }
}