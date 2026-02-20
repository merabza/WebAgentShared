using SystemTools.MediatRMessagingAbstractions;

namespace WebAgentShared.LibProjectsApi.CommandRequests;

public sealed class UpdateSettingsRequestCommand : ICommand
{
    public string? ProjectName { get; set; }
    public string? EnvironmentName { get; set; }
    public string? AppSettingsFileName { get; set; }
    public string? ParametersFileDateMask { get; set; }
    public string? ParametersFileExtension { get; set; }
    public string? UserName { get; set; }
}
