﻿using MessagingAbstractions;

namespace LibProjectsApi.CommandRequests;

public sealed class UpdateSettingsCommandRequest : ICommand
{
    public string? ProjectName { get; set; }
    public string? EnvironmentName { get; set; }
    public string? ServiceName { get; set; }
    public string? AppSettingsFileName { get; set; }
    public string? ParametersFileDateMask { get; set; }
    public string? ParametersFileExtension { get; set; }
}