using FluentValidation;
using LibProjectsApi.CommandRequests;

namespace LibProjectsApi.Validators;

// ReSharper disable once UnusedType.Global
public sealed class StartServiceCommandValidator : AbstractValidator<StartServiceCommandRequest>
{
    public StartServiceCommandValidator()
    {
        RuleFor(x => x.ServiceName).FileName();
        RuleFor(x => x.EnvironmentName).Name();
    }
}