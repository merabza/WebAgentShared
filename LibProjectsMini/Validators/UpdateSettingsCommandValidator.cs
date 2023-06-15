using FluentValidation;
using LibProjectsMini.CommandRequests;

namespace LibProjectsMini.Validators;

public sealed class UpdateSettingsCommandValidator : AbstractValidator<UpdateSettingsCommandRequest>
{
    public UpdateSettingsCommandValidator()
    {
        RuleFor(x => x.ProjectName).FileName();
        RuleFor(x => x.ServiceName).FileName();
        RuleFor(x => x.AppSettingsFileName).FileName();
        RuleFor(x => x.ParametersFileDateMask).DateMask();
        RuleFor(x => x.ParametersFileExtension).FileExtension();
    }
}