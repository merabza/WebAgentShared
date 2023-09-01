using FluentValidation;
using LibProjectsApi.CommandRequests;

namespace LibProjectsApi.Validators;

// ReSharper disable once UnusedType.Global
public sealed class UpdateServiceCommandValidator : AbstractValidator<UpdateServiceCommandRequest>
{
    public UpdateServiceCommandValidator()
    {
        RuleFor(x => x.ProjectName).FileName();
        RuleFor(x => x.EnvironmentName).Name();
        RuleFor(x => x.ServiceName).FileName();
        RuleFor(x => x.ServiceUserName).FileName();
        RuleFor(x => x.AppSettingsFileName).FileName();
        RuleFor(x => x.ProgramArchiveDateMask).DateMask();
        RuleFor(x => x.ProgramArchiveExtension).FileExtension();
        RuleFor(x => x.ParametersFileDateMask).DateMask();
        RuleFor(x => x.ParametersFileExtension).FileExtension();
    }
}