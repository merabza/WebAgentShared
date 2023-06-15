using FluentValidation;
using LibProjectsMini.CommandRequests;

namespace LibProjectsMini.Validators;

// ReSharper disable once UnusedType.Global
public sealed class ProjectUpdateCommandValidator : AbstractValidator<ProjectUpdateCommandRequest>
{
    public ProjectUpdateCommandValidator()
    {
        RuleFor(x => x.ProjectName).FileName();
        RuleFor(x => x.ProgramArchiveDateMask).DateMask();
        RuleFor(x => x.ProgramArchiveExtension).FileExtension();
        RuleFor(x => x.ParametersFileDateMask).DateMask();
        RuleFor(x => x.ParametersFileExtension).FileExtension();
    }
}