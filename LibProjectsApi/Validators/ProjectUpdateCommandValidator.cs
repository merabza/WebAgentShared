using FluentValidation;
using LibProjectsApi.CommandRequests;

namespace LibProjectsApi.Validators;

// ReSharper disable once UnusedType.Global
public sealed class ProjectUpdateCommandValidator : AbstractValidator<ProjectUpdateRequestCommand>
{
    public ProjectUpdateCommandValidator()
    {
        RuleFor(x => x.ProjectName).FileName();
        RuleFor(x => x.EnvironmentName).Name();
        RuleFor(x => x.ProgramArchiveDateMask).DateMask();
        RuleFor(x => x.ProgramArchiveExtension).FileExtension();
        RuleFor(x => x.ParametersFileDateMask).DateMask();
        RuleFor(x => x.ParametersFileExtension).FileExtension();
    }
}